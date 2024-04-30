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
    public class SizeProfile:Profile
    {
        public SizeProfile()
        {
            CreateMap<Size, SizeViewModel>().ReverseMap();
            CreateMap<Size, WishListViewModel>().ReverseMap();
        }
    }
}
