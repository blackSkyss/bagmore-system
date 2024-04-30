using AutoMapper;
using BAL.Helpers;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class UserService : IUserService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        Regex regexLetter = new Regex(@"[a-zA-Z]");
        Regex regexCharacter = new Regex(@"[\\~!@#$%^&*()_+={}\[\]|\\;:'""<>,.?/-]");
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task<UserProfileViewModel> GetUserProfileAsync(string email)
        {
           
            try
            {
                UserProfileViewModel userProfileViewModel = new UserProfileViewModel();
                   List <User> user = (List<User>)await ((UserRepository)_unitOfWork.UserRepository).Get();
                  User user2 = user.SingleOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new Exception("User does not exist");
                }
                _mapper.Map(user2, userProfileViewModel);
                return userProfileViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserProfileAsync(string userEmail, UserProfileViewModel userProfileViewModel)
        {
            string message = "";
            if (userProfileViewModel.FirstAddress.Equals("") ||
                userProfileViewModel.SecondAddress.Equals("") ||
                userProfileViewModel.FirstName.Equals("") ||
                userProfileViewModel.LastName.Equals("") ||
                userProfileViewModel.Phone.Equals("") ||
                userProfileViewModel.BirthDay == null
                )
            {
                message += "You need fill all field\n";
            }
            else if (userProfileViewModel.FirstName.Length > 15)
            {
                message += "First name must smaller than 15 character\n";
            }
            else if (userProfileViewModel.LastName.Length > 35)
            {
                message += "Last name must smaller than 35 character\n";
            }
            else if (userProfileViewModel.FirstAddress.Length > 100)
            {
                message += "First address must smaller than 100 character\n";
            }
            else if (userProfileViewModel.SecondAddress.Length > 100)
            {
                message += "Second address must smaller than 100 character\n";
            }
            else if (userProfileViewModel.Phone.Length != 10)
            {
                message += "Phone number must be 10 number\n";
            }
            else if (regexCharacter.IsMatch(userProfileViewModel.Phone.ToString()))
            {
                message += "You need input number, not characters\n";
            }
            else if (regexCharacter.IsMatch(userProfileViewModel.Phone.ToString()))
            {
                message += "You need input number, not characters\n";
            }
            else if (regexLetter.IsMatch(userProfileViewModel.Phone.ToString()))
            {
                message += "You need input number, not letter\n";
            }
            else if (DateTime.Now.Year - userProfileViewModel.BirthDay.Value.Year < 13)
            {
                message += "You don't enough age for use this app\n";
            }/*else if (DateTime.ParseExact(userProfileViewModel.BirthDay, "yyyy-MM-dd", null))
            {
                 //chua check ngay
            }*/
            if (message != "")
            {
                throw new Exception(message);
            }
            try
            {
                IEnumerable<User> user = await ((UserRepository)this._unitOfWork.UserRepository).Get();
                User user1 = user.SingleOrDefault(u => u.Email == userEmail);
                if (user1 == null)
                {
                    throw new Exception("User doesn't exist in the system");
                }
                else
                {
                    _mapper.Map(userProfileViewModel, user1);
                    ((UserRepository)_unitOfWork.UserRepository).Update(user1);
                    await this._unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //check xem user co ton tai hay khong
            // neu ton tai thi update(Do su lieu tu viewModel sang user da tim thay)
            //goi update va savechange
            // k ton tai thrown new Exception
        }
        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            try
            {
                User user = (await ((UserRepository)this._unitOfWork.UserRepository).Get(x => x.Email.Equals(email))).FirstOrDefault();
                if (user != null)
                {
                    return _mapper.Map<UserViewModel>(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserViewModel>> GetUsers(SearchingUserViewModel searchingUserViewModel)
        {
            try
            {
                List<User> users = await this._unitOfWork.UserRepository.GetUsersAsync();
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SearchValue) == false)
                {
                    users = users.Where(x => StringHelper.ConvertToUnSign(x.FirstName + " " +x.LastName).ToLower()
                    .Contains(StringHelper.ConvertToUnSign(searchingUserViewModel.SearchValue).ToLower())).ToList();
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortName) == false)
                {
                    switch (searchingUserViewModel.SortName)
                    {
                        case "name_desc":
                            users = users.OrderByDescending(x => x.LastName).ToList();
                            break;
                        case "name_asc":
                            users = users.OrderBy(x => x.LastName).ToList();
                            break;  
                    }
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortEmail) == false)
                {
                    switch (searchingUserViewModel.SortEmail)
                    {
                        case "email_desc":
                            users = users.OrderByDescending(x => x.Email).ToList();
                            break;
                        case "email_asc":
                            users = users.OrderBy(x => x.Email).ToList();
                            break;
                    }
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortGender) == false)
                {
                    switch (searchingUserViewModel.SortGender)
                    {
                        case "gender_desc":
                            users = users.OrderByDescending(x => x.Gender).ToList();
                            break;
                        case "gender_asc":
                            users = users.OrderBy(x => x.Gender).ToList();
                            break;
                    }
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortBirthday) == false)
                {
                    switch (searchingUserViewModel.SortBirthday)
                    {
                        case "birthday_desc":
                            users = users.OrderByDescending(x => x.BirthDay).ToList();
                            break;
                        case "birthday_asc":
                            users=users.OrderBy(x => x.BirthDay).ToList();
                            break;
                    }
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortStatus) == false)
                {
                    switch (searchingUserViewModel.SortStatus)
                    {
                        case "status_desc":
                            users = users.OrderByDescending(x => x.Status).ToList();
                            break;
                        case "stats_asc":
                            users = users.OrderBy(x => x.Status).ToList();
                            break;
                    }
                }
                if(string.IsNullOrWhiteSpace(searchingUserViewModel.SortJoinDate) == false)
                {
                    switch (searchingUserViewModel.SortJoinDate)
                    {
                        case "joinDate_desc":
                            users = users.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        case "joinDate_asc":
                            users = users.OrderBy(x => x.CreatedDate).ToList();
                            break;
                    }
                }
                if(searchingUserViewModel.CurrentPage != null && searchingUserViewModel.ItemsPerPage != null)
                {
                    users = users.Skip((searchingUserViewModel.CurrentPage.Value - 1) * searchingUserViewModel.ItemsPerPage.Value)
                             .Take(searchingUserViewModel.ItemsPerPage.Value).ToList();
                } else
                {
                    if(searchingUserViewModel.CurrentPage == null)
                    {
                        throw new Exception("Current page is required!");
                    } 
                    if(searchingUserViewModel.ItemsPerPage == null)
                    {
                        throw new Exception("Items per page are required!");
                    }
                }
                return _mapper.Map<List<UserViewModel>>(users);

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task BanUser(int userId)
        {
            try
            {
                User user = await this._unitOfWork.UserRepository.GetUserByIdAsync(userId);
                if(user == null)
                {
                    throw new Exception("User does not exist!");
                }
                user.Status = (int)UserEnum.Status.INACTIVE;
                this._unitOfWork.UserRepository.UpdateUser(user);
                await this._unitOfWork.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
