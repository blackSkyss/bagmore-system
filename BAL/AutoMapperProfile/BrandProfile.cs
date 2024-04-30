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
    public class BrandProfile:Profile
    {
        public BrandProfile()
        {
            
            CreateMap<Brand, BrandNameViewModel>().ReverseMap();
            CreateMap<Brand, BrandViewModel>().ReverseMap();
            CreateMap<Brand, BrandGetInforViewModel>().ReverseMap();
            CreateMap<Brand, BrandCreatedViewModel>().ReverseMap();

        }
    }
}
