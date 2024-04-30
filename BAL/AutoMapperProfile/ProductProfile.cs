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
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            
            CreateMap<ProductDetailViewModel, Product>().ReverseMap();
            CreateMap<CreatedProductViewModel, Product>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModelAdmin>().ForMember(dept => dept.Name, opt => opt.MapFrom(src => src.Name))
                                                         .ForMember(dept => dept.DescribeProducts, opt => opt.MapFrom(src => src.DescribeProducts))
                                                         .ForMember(dept => dept.SuppliedProducts, opt => opt.MapFrom(src => src.SuppliedProducts))
                                                         .ForMember(dept => dept.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                                                         .ForMember(dept => dept.Description, opt => opt.MapFrom(src => src.Description))
                                                         .ForMember(dept => dept.Composition, opt => opt.MapFrom(src => src.Composition))
                                                         .ForMember(dept => dept.DescribeProducts, opt => opt.MapFrom(src => src.DescribeProducts))
                                                         .ForMember(dept => dept.Discount, opt => opt.MapFrom(src => src.Discount))
                                                         .ForMember(dept => dept.Brand, opt => opt.MapFrom(src => src.Brand))
                                                         .ForMember(dept => dept.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Product, TotalProductViewModel>().ReverseMap();
            CreateMap<Product, WishListViewModel>().ReverseMap();

        }
    }
}
