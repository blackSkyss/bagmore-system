using AutoMapper;
using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.AutoMapperProfile
{
    public class OrderUserDetailItemProfile : Profile
    {
        public OrderUserDetailItemProfile()
        {
            CreateMap<CombinedOrderDetailItemViewModel, OrderUserDetailItemViewModel>().ReverseMap();

        }
    }
}
