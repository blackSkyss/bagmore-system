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
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderViewModel>().ForMember(dept => dept.Id, opt => opt.MapFrom(src => src.Id))
                                              .ForMember(dept => dept.DeliveryStatus, opt => opt.MapFrom(src => src.DeliveryStatus))
                                              .ForMember(dept => dept.OrderedDate, opt => opt.MapFrom(src => src.OrderedDate))
                                              .ForMember(dept => dept.Status, opt => opt.MapFrom(src => src.Status))
                                              .ForMember(dept => dept.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.Name));
        }
    }
}
