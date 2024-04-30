using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IWishListService
    {
        public Task<List<WishListViewModel>> GetWishListByUserIdAsync(String username);
        public Task<bool> AddWishListAsync(WishListCreateNewViewModel wishList);
        public Task<bool> DeleteWishListAsync(int idWishList);
    }
}
