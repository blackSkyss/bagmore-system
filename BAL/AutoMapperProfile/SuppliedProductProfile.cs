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
    public class SuppliedProductProfile : Profile
    {
        public SuppliedProductProfile()
        {
            CreateMap<SuppliedProduct, SuppliedProductViewModel>().ForMember(dept => dept.Supplier, opt => opt.MapFrom(src => src.Supplier));
            CreateMap<SuppliedProduct, SuppliedProductCreatedViewModel>().ReverseMap();
            CreateMap<SuppliedProduct, SupplierCreatedProductViewModel>().ReverseMap();
        }
    }
}
