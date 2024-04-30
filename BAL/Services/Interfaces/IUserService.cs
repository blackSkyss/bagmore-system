using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileViewModel> GetUserProfileAsync(string email);
        Task UpdateUserProfileAsync (string userEmail, UserProfileViewModel userViewModel);
        public Task<UserViewModel> GetUserByEmailAsync(string email);
        public Task<List<UserViewModel>> GetUsers(SearchingUserViewModel searchingUserViewModel);
        public Task BanUser(int userId);
    }
}
