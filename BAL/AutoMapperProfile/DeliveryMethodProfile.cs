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
    public class DeliveryMethodProfile : Profile
    {
        public DeliveryMethodProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodViewModel>().ReverseMap();
            CreateMap<DeliveryMethod, EditDeliveryMethodViewModel>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodUserViewModel>().ReverseMap();
        }
    }
}
