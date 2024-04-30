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
    public class DescribeProductProfile:Profile
    {
        public DescribeProductProfile()
        {
            CreateMap<DescribeProduct, DescribeProductsViewModel>().ReverseMap();
            CreateMap<DescribeProduct, ProductViewModelAdmin>().ReverseMap();
            CreateMap<DescribeProduct, DescribleProductForCreateNewViewModel>().ReverseMap();
            CreateMap<DescribeProduct, WishListViewModel>().ReverseMap();
            CreateMap<DescribeProduct, AdminDescribeProductViewModel>()
                .ForMember(dept => dept.ProviderId, opt => opt.MapFrom(src => src.providerId))
                .ForMember(dept => dept.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dept => dept.OriginalPrice, opt => opt.MapFrom(src => src.OriginalPrice))
                .ForMember(dept => dept.SizeId, opt => opt.MapFrom(src => src.SizeId))
                .ForMember(dept => dept.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dept => dept.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dept => dept.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dept => dept.SizeName, opt => opt.MapFrom(src => src.Size.Name));
        }
    }
}
