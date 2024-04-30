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
    public class ShippingAddressProfile:Profile
    {
        public ShippingAddressProfile()
        {
            CreateMap<ShippingAddressViewModel, ShippingAddress> ().ReverseMap();
            CreateMap<ShippingAddressViewModel, DescribeProduct>().ReverseMap();
        }
    }
}
