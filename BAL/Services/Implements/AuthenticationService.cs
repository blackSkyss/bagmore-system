using AutoMapper;
using BAL.Helpers;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class AuthenticationService : IAuthenticationService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private HttpClient _httpClient;
        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
            this._httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            this._httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<UserViewModel> LoginAsync(AccountViewModel account)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account.Email))
                {
                    throw new Exception("Email is required");
                }
                if (string.IsNullOrWhiteSpace(account.Password))
                {
                    throw new Exception("Password is required");
                }
                account.Password = StringHelper.EncryptData(account.Password);
                UserViewModel userViewModel = _mapper.Map<UserViewModel>(await this._unitOfWork.UserRepository.LoginAsync(account.Email, account.Password));
                if (userViewModel == null)
                {
                    throw new Exception("Email or Password are invalid");
                }
                return userViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TokenViewModel> GenerateToken(UserViewModel account, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            User user = (await ((UserRepository)this._unitOfWork.UserRepository).Get(x => x.Email.Equals(account.Email))).FirstOrDefault();
            account.Role = await ((RoleRepository)this._unitOfWork.RoleRepository).GetByID(user.RoleId);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", account.Email),
                    new Claim("Role", account.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            string accessToken = jwtTokenHandler.WriteToken(token);
            string refreshToken = GenerateRefreshToken();

            UserToken refreshTokenEntity = new UserToken
            {
                JwtId = token.Id,
                Token = refreshToken,
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(10)
            };

            await this._unitOfWork.UserTokenRepository.InsertUserToken(refreshTokenEntity);
            await this._unitOfWork.SaveChangesAsync();

            TokenViewModel tokenModel = new TokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            return tokenModel;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<TokenViewModel> ReNewUserToken(TokenViewModel oldToken, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                //Tự cấp token nên phần này bỏ qua
                ValidateIssuer = false,
                ValidateAudience = false,
                //Ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateLifetime = false, //khong kiem tra token het han
                ClockSkew = TimeSpan.Zero // thoi gian expired dung voi thoi gian chi dinh
            };
            try
            {
                //Check 1: Access token is valid format
                var tokenVerification = jwtTokenHandler.ValidateToken(oldToken.AccessToken, tokenValidationParameters, out var validatedToken);
                //Check 2: Check Alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        throw new Exception("Invalid Token");
                    }
                }
                //Check 3: check accessToken expried?
                var utcExpiredDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiredDate = ConvertUnixTimeToDateTime(utcExpiredDate);
                if (expiredDate > DateTime.UtcNow)
                {
                    throw new Exception("Access Token has not yet expired");
                }
                //Check 4: Check refresh token exist in Db
                UserToken existedRefreshToken = await this._unitOfWork.UserTokenRepository.GetRefreshToken(oldToken.RefreshToken);
                if (existedRefreshToken == null)
                {
                    throw new Exception("Refresh Token does not exist");
                }
                //Check 5: Refresh Token is used / revoked?
                if (existedRefreshToken.IsUsed)
                {
                    throw new Exception("Refresh Token is used");
                }
                if (existedRefreshToken.IsRevoked)
                {
                    throw new Exception("Refresh Token is revoked");
                }
                //Check 6: Id of refresh token == id of access token
                var jwtId = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (existedRefreshToken.JwtId.Equals(jwtId) == false)
                {
                    throw new Exception("Refresh Token is not match with Access Token");
                }
                //Check 7: refresh token is expired
                if (existedRefreshToken.ExpiredAt < DateTime.UtcNow)
                {
                    throw new Exception("Refresh Token expired");
                }

                existedRefreshToken.IsRevoked = true;
                existedRefreshToken.IsUsed = true;
                this._unitOfWork.UserTokenRepository.UpdateUserToken(existedRefreshToken);
                await this._unitOfWork.SaveChangesAsync();

                //Create new refreshToken
                var newRefreshToken = await GenerateToken(_mapper.Map<UserViewModel>(existedRefreshToken.User), secretKey);
                return newRefreshToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private DateTime ConvertUnixTimeToDateTime(long utcExpiredDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpiredDate).ToUniversalTime();
            return dateTimeInterval;
        }
        public async Task RegisterAccountAsync(UserAccountViewModel account)
        {
            try
            {
                #region Email
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (string.IsNullOrWhiteSpace(account.Email))
                {
                    throw new Exception("Email is required");
                }
                if (regex.IsMatch(account.Email) == false)
                {
                    throw new Exception("Email is invalid format");
                }
                if (account.Email.Length > 100)
                {
                    throw new Exception("Email length is required less than or equal 100 characters");
                }
                User existedUser = (await ((UserRepository)this._unitOfWork.UserRepository).Get(x => x.Email.Equals(account.Email))).FirstOrDefault();
                if (existedUser != null)
                {
                    throw new Exception("Email is existed in the system");
                }
                #endregion
                #region Password
                if (string.IsNullOrWhiteSpace(account.Password))
                {
                    throw new Exception("Password is required");
                }
                if (account.Password.Length > 50 || account.Password.Length < 8)
                {
                    throw new Exception("Password length is required more than 8 characters and less than or equal 50 characters");
                }
                account.Password = StringHelper.EncryptData(account.Password);
                #endregion
                #region FirstName
                if (string.IsNullOrWhiteSpace(account.FirstName))
                {
                    throw new Exception("First name is required");
                }
                if (account.FirstName.Length > 15)
                {
                    throw new Exception("First name length is required less than or equal 15 characters");
                }
                #endregion
                #region LastName
                if (string.IsNullOrWhiteSpace(account.LastName))
                {
                    throw new Exception("Last Name name is required");
                }
                if (account.FirstName.Length > 35)
                {
                    throw new Exception("Last name length is required less than or equal 15 characters");
                }
                #endregion
                #region Gender
                if (account.Gender == null)
                {
                    throw new Exception("Gender is required");
                }
                #endregion
                #region  Phone
                if (string.IsNullOrWhiteSpace(account.Phone))
                {
                    throw new Exception("Phone is required");
                }
                if (account.Phone.Length != 10)
                {
                    throw new Exception("Phone length is required 10 digits");
                }
                #endregion
                #region DateOfBirth
                if (account.BirthDay == null)
                {
                    throw new Exception("Date of Birth is required");
                }
                int old = DateTime.UtcNow.Year - account.BirthDay.Value.Year;
                if (old < 13)
                {
                    throw new Exception("Date of birth is required that user must be more than 13 years old");
                }
                #endregion
                #region Avatar
                if (account.Image != null)
                {


                    if (account.Image.Length > StringHelper.MAX_FILE_LENGTH)
                    {
                        throw new Exception("File is required the length 5MB");
                    }
                    string[] extension = new string[]
                    {
                    ".png", ".jpg"
                    };
                    if (extension.Contains(Path.GetExtension(account.Image.FileName).ToLower()) == false)
                    {
                        throw new Exception("Image is required format with extension .png .jpg");
                    }
                }
                #endregion
                #region First Address
                if (string.IsNullOrWhiteSpace(account.FirstAddress))
                {
                    throw new Exception("First Address is required");
                }
                if (account.FirstAddress.Length > 100)
                {
                    throw new Exception("First Address is required less than or equal 100 characters");
                }
                #endregion
                #region Second Address
                if (string.IsNullOrWhiteSpace(account.SecondAddress))
                {
                    throw new Exception("Second Address is required");
                }
                if (account.FirstAddress.Length > 100)
                {
                    throw new Exception("Second Address is required less than or equal 100 characters");
                }
                #endregion
                User user = _mapper.Map<User>(account);
                #region createdDate
                user.CreatedDate = DateTime.Now;
                #endregion
                user.Status = 1;
                user.RoleId = 2;
                if(account.Image != null)
                {
                    user.Avatar = await ImageHelper.ImageToBase64(account.Image);
                }
                await this._unitOfWork.UserRepository.RegisterUserAsync(user);
                await this._unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(AuthenticationRequest data, GoogleViewModel google)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

                settings.Audience = new List<string>()
                {
                    google.ClientId
                };

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(data.IdToken);
                return payload;
            }
            catch
            {
                throw new Exception("Invalid External Authentication");
            }
        }
        public async Task<FacebookUserResource> VerifyFacebookToken(AuthenticationRequest data)
        {
            if (string.IsNullOrEmpty(data.IdToken))
            {
                throw new Exception("Token is null or empty");
            }
            var result = await GetAsync<dynamic>(data.IdToken, "me", "fields=first_name,last_name,email");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }
            var account = new FacebookUserResource()
            {
                Email = result.email,
                FirstName = result.first_name,
                LastName = result.last_name,
            };
            return account;
        }
        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
        public async Task<UserViewModel> RegisterAccountWithEmailAsync(UserViewModel newUser)
        {
            try
            {
                //check account exited?
                User user = (await ((UserRepository)this._unitOfWork.UserRepository).Get(x => x.Email.Equals(newUser.Email))).FirstOrDefault();
                //existed - > error
                if (user != null)
                {
                    throw new Exception("User's Email is existed");
                }
                newUser.RoleId = (int)RoleEnum.Role.USER;
                newUser.CreatedDate = DateTime.Now;
                newUser.Status = (int)UserEnum.Status.ACTIVE;
                //Create
                await this._unitOfWork.UserRepository.RegisterUserAsync(_mapper.Map<User>(newUser));
                await this._unitOfWork.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> LogoutAsyc(string jwtId)
        {
            try
            {
                UserToken userToken = await this._unitOfWork.UserTokenRepository.GetRefreshTokenByJwtId(jwtId);
                if (userToken != null)
                {
                    userToken.IsRevoked = true;
                    this._unitOfWork.UserTokenRepository.UpdateUserToken(userToken);
                    await this._unitOfWork.SaveChangesAsync();
                    return true;
                }
                throw new Exception("Access token is invalid!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
