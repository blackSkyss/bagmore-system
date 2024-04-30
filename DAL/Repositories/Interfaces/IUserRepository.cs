using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> LoginAsync(string email, string password);
        public Task RegisterUserAsync(User user);
        public Task<User> GetUserAsync(string email);
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int userId);
        public void UpdateUser(User user);
    }
}
