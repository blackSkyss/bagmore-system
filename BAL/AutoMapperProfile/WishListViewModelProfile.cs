using AutoMapper;
using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.AutoMapperProfile
{
    public class WishListViewModelProfile :Profile
    {
        public WishListViewModelProfile()
        {
            CreateMap<CombinedItemViewModel, WishListViewModel>().ReverseMap();
        }
    }
}
