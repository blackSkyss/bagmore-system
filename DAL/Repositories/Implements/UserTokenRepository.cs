using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class UserTokenRepository : RepositoryBase<UserToken>, IUserTokenRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public UserTokenRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            this._bagMoreDbContext = dbContext;
        }

        public async Task<UserToken> GetRefreshToken(string refreshToken)
        {
            try
            {
                return await this._bagMoreDbContext.UserTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token.Equals(refreshToken));
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<UserToken> GetRefreshTokenByJwtId(string jwtId)
        {
            try
            {
                return await this._bagMoreDbContext.UserTokens.FirstOrDefaultAsync(x => x.JwtId.Equals(jwtId));
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertUserToken(UserToken userToken)
        {
            try
            {
                await this._bagMoreDbContext.UserTokens.AddAsync(userToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateUserToken(UserToken userToken)
        {
            try
            {
                this._bagMoreDbContext.Entry<UserToken>(userToken).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
