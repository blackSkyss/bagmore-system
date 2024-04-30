using AutoMapper;
using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.AutoMapperProfile
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, UserAccountViewModel>().ReverseMap();
            CreateMap<User, AccountViewModel>().ReverseMap();
            CreateMap<User, UserProfileViewModel>().ReverseMap();
        }
    }
}
