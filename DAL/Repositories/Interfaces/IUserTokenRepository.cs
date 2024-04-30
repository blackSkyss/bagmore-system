using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        public Task InsertUserToken(UserToken userToken);
        public Task<UserToken> GetRefreshToken(string refreshToken);
        public void UpdateUserToken(UserToken userToken);
        public Task<UserToken> GetRefreshTokenByJwtId(string jwtId);
    }
}
