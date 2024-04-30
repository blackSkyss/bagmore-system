using BAL.Models;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<TokenViewModel> GenerateToken(UserViewModel account, string secretKey);
        public Task<UserViewModel> LoginAsync(AccountViewModel account);
        public Task<TokenViewModel> ReNewUserToken(TokenViewModel oldToken, string secretKey);
        public Task RegisterAccountAsync(UserAccountViewModel account);
        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(AuthenticationRequest data, GoogleViewModel google);
        public Task<UserViewModel> RegisterAccountWithEmailAsync(UserViewModel newUser);
        public Task<FacebookUserResource> VerifyFacebookToken(AuthenticationRequest data);
        public Task<bool> LogoutAsyc(string jwtId);
    }
}
