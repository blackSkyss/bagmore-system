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
    public class ProductOrderProfile: Profile
    {
        public ProductOrderProfile()
        {
            CreateMap<ProductOrder, OrderProductsViewModel>()
                .ForMember(dept => dept.ProductId, opt => opt.MapFrom(src => src.IdProduct))
                .ForMember(dept => dept.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dept => dept.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dept => dept.Amout, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dept => dept.ProductColor, opt => opt.MapFrom(src => src.Color))
                .ForMember(dept => dept.ProductSize, opt => opt.MapFrom(src => src.Size));
        }
    }
}
