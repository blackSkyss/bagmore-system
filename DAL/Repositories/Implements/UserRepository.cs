using DAL.Entities;
using DAL.Enums;
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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public UserRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            this._bagMoreDbContext = dbContext;
        }

        public async Task<User> GetUserAsync(string email)
        {
            try
            {
                return await this._bagMoreDbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email)
                                                                    && x.Status == (int)UserEnum.Status.ACTIVE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await this._bagMoreDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId && x.RoleId == (int)RoleEnum.Role.USER);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await this._bagMoreDbContext.Users.Where(x => x.RoleId == (int)RoleEnum.Role.USER).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                return await this._bagMoreDbContext.Users.Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Email.Equals(email) && x.Password.Equals(password));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RegisterUserAsync(User user)
        {
            try
            {
                await this._bagMoreDbContext.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                this._bagMoreDbContext.Entry<User>(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
