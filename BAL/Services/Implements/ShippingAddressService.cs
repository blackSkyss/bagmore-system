using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
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
    public class ShippingAddressService : IShippingAddressService
    {
        Regex regexCharacter = new Regex(@"[\\~!@#$%^&*()_+={}\[\]|\\;:'""<>,.?/-]");
        Regex regexLetter = new Regex(@"[a-zA-Z]");
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ShippingAddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddShippingAddressesAsync(string userName ,ShippingAddressViewModel shippingAddressViewModel)
        {
            string message = "";
            if (shippingAddressViewModel.FirstName.Equals("") ||
                shippingAddressViewModel.LastName.Equals("") ||
                shippingAddressViewModel.Address.Equals("") ||
                shippingAddressViewModel.Phone.Equals("")
                )
            {
               message+= "You need fill all field\n";
            }
            if (shippingAddressViewModel.FirstName.Length > 15)
            {
                message += "First name must smaller than 15 character\n";
            }
             if (shippingAddressViewModel.LastName.Length > 35)
            {
                message +="Last name must smaller than 35 character\n";
            }
             if (regexCharacter.IsMatch(shippingAddressViewModel.Phone.ToString()))
            {
                message += "You need input number, not characters\n";
            }
             if (regexLetter.IsMatch(shippingAddressViewModel.Phone.ToString()))
            {
                message += "You need input number, not letter\n";
            }
             if (shippingAddressViewModel.Phone.Length != 10)
            {
                message += "Phone number must be 10 number\n";
            }

             if (shippingAddressViewModel.Address.Length > 100)
            {
                message += "Address must smaller than 100 character\n";
            }
             
            if(message != "")
            {
                throw new Exception(message);
            }
           
            try
            {

                User user = await this._unitOfWork.UserRepository.GetUserAsync(userName);
                if (user != null)
                {

                    ShippingAddress shippingAddress = new ShippingAddress();
                    _mapper.Map(shippingAddressViewModel, shippingAddress);
                    shippingAddress.IdUser = user.Id;
                    shippingAddress.Status = 1;
                    await ((ShippingAddressRepository)_unitOfWork.ShippingAddressRepository).Insert(shippingAddress);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("userId doesn't exists");
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public async Task<List<ShippingAddressViewModel>> GetShippingAddressAsync(string name)
        {


            User user = await this._unitOfWork.UserRepository.GetUserAsync(name);
            int userId = user.Id;

            if (regexCharacter.IsMatch(userId.ToString()))
            {
                throw new Exception("You need input number, not characters");
            }
            if (regexLetter.IsMatch(userId.ToString()))
            {
                throw new Exception("You need input number, not letters");
            }
            try
            {
                User userIdCheck = await ((UserRepository)
                    _unitOfWork.UserRepository).GetByID(userId);
                if (userIdCheck == null)
                {
                    throw new Exception("userId doesn't exists in System");
                }
                List<ShippingAddressViewModel> shippingAddressViewModel = _mapper.Map<List<ShippingAddressViewModel>>
                    (await ((ShippingAddressRepository)_unitOfWork.ShippingAddressRepository).GetShippingAddressAsync(userId));
                if (shippingAddressViewModel.Count == 0)
                {
                    throw new Exception("User don't have shipping address");
                }
                return shippingAddressViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
