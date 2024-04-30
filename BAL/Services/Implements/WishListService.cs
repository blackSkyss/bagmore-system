using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class WishListService : IWishListService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public WishListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<WishListViewModel>> GetWishListByUserIdAsync(String username)
        {
            try
            {
                User user = await this._unitOfWork.UserRepository.GetUserAsync(username);
                List<WishListViewModel> wishLists = new List<WishListViewModel>();
                IEnumerable<WishList> wl = await ((WishListRepository)_unitOfWork.WishListRepository).Get();
                List<Product> products = new List<Product>();
                List<DescribeProduct> describeProducts = new List<DescribeProduct>();
                List<Size> sizes = new List<Size>();
                List<Color> colors = new List<Color>();
                wl = wl.Where(w => w.UserId == user.Id);
                if (wl.Count() == 0)
                {
                    throw new Exception("WishList Empty");
                }
                foreach (var wlist in wl)
                {
                    Product product = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(wlist.ProductId);
                    products.Add(product);
                }

                foreach (var product in products)
                {
                    DescribeProduct describeProduct = await ((DescribeProductRepository)_unitOfWork.DescribeProductRepository).GetByID(product.Id);
                    describeProducts.Add(describeProduct);
                }

                foreach (var ds in describeProducts)
                {
                    Size s = await ((SizeRepository)_unitOfWork.SizeRepository).GetByID(ds.SizeId);
                    sizes.Add(s);
                    Color c = await ((ColorRepository)_unitOfWork.ColorRepository).GetByID(ds.ColorId);
                    colors.Add(c);
                }
                var combinedList = new List<CombinedItemViewModel>();


                foreach (var describeProduct in describeProducts)
                {
                    var combinedItem = new CombinedItemViewModel
                    {
                        Price = describeProduct.Price
                    };
                    combinedList.Add(combinedItem);
                }

                for (int i = 0; i < products.Count; i++)
                {
                    var combinedItem = combinedList[i];
                    var product = products[i];
                    combinedItem.Name = product.Name;
                }
                for (int i = 0; i < sizes.Count; i++)
                {
                    var combinedItem = combinedList[i];
                    var size = sizes[i];
                    combinedItem.Size = size.Name;
                }
                for (int i = 0; i < colors.Count; i++)
                {
                    var combinedItem = combinedList[i];
                    var color = colors[i];
                    combinedItem.Color = color.Name;
                }

                _mapper.Map(combinedList, wishLists);
                return wishLists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<bool> AddWishListAsync(WishListCreateNewViewModel wishList)
        {
            try
            {
                User user = await ((UserRepository)_unitOfWork.UserRepository).GetByID(wishList.UserId);
                Product product = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(wishList.ProductId);

                if (product == null)
                {
                    throw new Exception("Product Id does not exist!");
                }

                if (user == null)
                {
                    throw new Exception("User Id does not exist!");
                }
                WishList wl = new WishList();
                _mapper.Map(wishList, wl);
                await ((WishListRepository)_unitOfWork.WishListRepository).Insert(wl);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteWishListAsync(int idWishList)
        {
            try
            {
                WishList wishList = await ((WishListRepository)_unitOfWork.WishListRepository).GetByID(idWishList);
                if (wishList == null)
                {
                    throw new Exception("Id wish list does not exist");
                }
                await ((WishListRepository)_unitOfWork.WishListRepository).Delete(idWishList);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
