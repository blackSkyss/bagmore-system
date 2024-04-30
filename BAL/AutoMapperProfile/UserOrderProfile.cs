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
    public class UserOrderProfile: Profile
    {
        public UserOrderProfile()
        {
            CreateMap<Order, UserOrderViewModel>()
                .ForMember(dept => dept.OrderID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dept => dept.UserFullName, opt => opt.MapFrom(src => (src.User.FirstName + " " + src.User.LastName)))
                .ForMember(dept => dept.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dept => dept.OrderedDate, opt => opt.MapFrom(src => src.OrderedDate))
                .ForMember(dept => dept.DeliveryStatus, opt => opt.MapFrom(src => src.DeliveryStatus))
                .ForMember(dept => dept.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.Name))
                .ForMember(dept => dept.TotalProduct, opt => opt.MapFrom(src => src.ProductOrders.Count()))
                .ForMember(dept => dept.TotalPrice, opt => opt.MapFrom(src => (src.ProductOrders.Sum(x => x.Price) + src.DeliveryMethod.Price)));
        }
    }
}
