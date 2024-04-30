using BagMoreSystem.Helpers;
using BAL.Authorization;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BagMoreSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private AppSetting _appSetting;
        private GoogleViewModel _google;
        private IUserService _userService;
        private FacebookViewModel _facebook;
        public AuthenticationController(IAuthenticationService authenticationService, IOptionsMonitor<AppSetting> appSetting, 
            IOptionsMonitor<GoogleViewModel> google, IUserService userService, IOptionsMonitor<FacebookViewModel> facebook)
        {
            this._authenticationService = authenticationService;
            this._appSetting = appSetting.CurrentValue;
            this._google = google.CurrentValue;
            this._userService = userService;
            this._facebook = facebook.CurrentValue;
        }

        [HttpPost("user-login")]
        public async Task<IActionResult> LoginAsync([FromBody] AccountViewModel account)
        {
            try
            {
                //1. login
                UserViewModel userViewModel = await this._authenticationService.LoginAsync(account);
                //2. generate token
                TokenViewModel userToken = await this._authenticationService.GenerateToken(userViewModel, this._appSetting.SecretKey);
                return Ok(new
                {
                    Success = true,
                    Message = "Authenticate Success",
                    Email = userViewModel.Email,
                    Role = userViewModel.Role.Name,
                    Data = userToken
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> ReNewToken([FromBody] TokenViewModel token)
        {
            try
            {
                TokenViewModel newToken = await this._authenticationService.ReNewUserToken(token, this._appSetting.SecretKey);
                if (newToken != null)
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = newToken
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Re-Create token failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserAccountViewModel account)
        {
            try
            {
                await this._authenticationService.RegisterAccountAsync(account);
                return Ok(new
                {
                    Success = true,
                    Message = "Register Account Successful"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> AuthenticateGoogle([FromBody] AuthenticationRequest data)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = await this._authenticationService.VerifyGoogleToken(data, _google);
                UserViewModel user = await this._userService.GetUserByEmailAsync(payload.Email);
                if (user == null)
                {
                    UserViewModel newUser = new UserViewModel()
                    {
                        Email = payload.Email,
                        FirstName = payload.FamilyName,
                        LastName = payload.GivenName
                    };
                    newUser = await this._authenticationService.RegisterAccountWithEmailAsync(newUser);
                    TokenViewModel token = await this._authenticationService.GenerateToken(newUser, this._appSetting.SecretKey);
                    return Ok(new
                    {
                        Success = true,
                        Email = newUser.Email,
                        Role = newUser.Role.Name,
                        Data = token
                    });
                }
                else
                {
                    var token = await this._authenticationService.GenerateToken(user, _appSetting.SecretKey);
                    return Ok(new
                    {
                        Success = true,
                        Email = payload.Email,
                        Role = user.Role.Name,
                        Data = token
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login-facebook")]
        public async Task<IActionResult> AuthenticateFacebook([FromBody] AuthenticationRequest data)
        {
            try
            {
                FacebookUserResource facebookUser = await this._authenticationService.VerifyFacebookToken(data);
                UserViewModel user = await this._userService.GetUserByEmailAsync(facebookUser.Email);
                if (user == null)
                {
                    UserViewModel newUser = new UserViewModel()
                    {
                        Email = facebookUser.Email,
                        FirstName = facebookUser.FirstName,
                        LastName = facebookUser.LastName
                    };
                    newUser = await this._authenticationService.RegisterAccountWithEmailAsync(newUser);
                    TokenViewModel token = await this._authenticationService.GenerateToken(newUser, this._appSetting.SecretKey);
                    return Ok(new
                    {
                        Success = true,
                        Email = newUser.Email,
                        Role = newUser.Role.Name,
                        Data = token,
                    });
                } else
                {
                    var token = await this._authenticationService.GenerateToken(user, _appSetting.SecretKey);
                    return Ok(new
                    {
                        Success = true,
                        Email = user.Email,
                        Role = user.Role.Name,
                        Data = token
                    });
                }
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                if (jwtSecurityToken != null)
                {
                    string jwtId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                    bool result = await this._authenticationService.LogoutAsyc(jwtId);
                    if (result)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Logout successfully"
                        }); ;
                    }
                }
                return BadRequest(new
                {
                    Success = false,
                    Message = "Logout failed"
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
