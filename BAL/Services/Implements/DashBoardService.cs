using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class DashBoardService : IDashBoardService
    {
        private UnitOfWork _unitOfWork;
        public DashBoardService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task<DashBoardInformationViewModel> GetDashBoardInformation()
        {
            try
            {
                IEnumerable<User> user = await ((UserRepository)_unitOfWork.UserRepository).Get();
                int usersCount = user.Where(u => u.RoleId == (int)RoleEnum.Role.USER && u.Status == (int)UserEnum.Status.ACTIVE).Count();

                IEnumerable<Supplier> suppliers = await ((SupplierRepository)_unitOfWork.SupplierRepository).Get();
                int suppliersCount = suppliers.Where(u => u.Status == 1).Count();

                IEnumerable<Brand> brands = await ((BrandRepository)_unitOfWork.BrandRepository).Get();
                int brandsCount = brands.Where(u => u.Status == 1).Count();

                IEnumerable<Category> categories = await ((CategoryRepository)_unitOfWork.CategoryRepository).Get();
                int categoriesCount = categories.Where(u => u.Status == 1).Count();

                IEnumerable<DeliveryMethod> deliveryMethods = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Get();
                int deliveryMethodsCount = deliveryMethods.Where(u => u.Status == 1).Count();

                IEnumerable<Order> orders = (await ((OrderRepository)_unitOfWork.OrderRepository).Get()).OrderBy(x => x.OrderedDate);
                int ordersCount = orders.Where(u => u.Status == 1).Count();

                List<int> OrderdSoldYear = new List<int>();
                foreach (var order in orders)
                {
                    if(OrderdSoldYear.Contains(order.OrderedDate.Value.Year) == false)
                    {
                        OrderdSoldYear.Add(order.OrderedDate.Value.Year);
                    }
                }

                DashBoardInformationViewModel dashBoardInformationViewModel = new DashBoardInformationViewModel
                {
                    TotalUsers = usersCount,
                    TotalBrands = brandsCount,
                    TotalCategories = categoriesCount,
                    TotalDeliveryMethods = deliveryMethodsCount,
                    TotalSuppliers = suppliersCount,
                    TotalUserOrders = ordersCount,
                    Years = OrderdSoldYear

                };
                return dashBoardInformationViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<List<OrderSoldViewModel>> GetNumberOfOrdersSoldAsync(int? year)
        {
            try
            {
                List<OrderSoldViewModel> orderSoldViewModels = new List<OrderSoldViewModel>();
                IEnumerable<Order> orders = (await ((OrderRepository)_unitOfWork.OrderRepository).Get()).OrderBy(x => x.OrderedDate);
                if (year == null)
                {
                    Order order = orders.FirstOrDefault();
                    if(order != null)
                    {
                        OrderSoldViewModel orderSold = new OrderSoldViewModel();
                        orderSold.Month = order.OrderedDate.Value.Month;
                        orderSold.TotalOrder += orders.Where(o => o.OrderedDate.Value.Month == order.OrderedDate.Value.Month).Count();
                        orderSoldViewModels.Add(orderSold);
                        return orderSoldViewModels.GroupBy(p => p.Month)
                                              .Select(g => g.First())
                                              .ToList();
                    }
                    return orderSoldViewModels;
                }
                else
                {
                    if (orders.Where(u => u.OrderedDate.Value.Year == year).Count() != 0)
                    {
                        orders = orders.Where(u => u.OrderedDate.Value.Year == year);

                        foreach (var order in orders)
                        {
                            OrderSoldViewModel orderSold = new OrderSoldViewModel();
                            orderSold.Month = order.OrderedDate.Value.Month;
                            orderSold.TotalOrder += orders.Where(o => o.OrderedDate.Value.Month == order.OrderedDate.Value.Month).Count();
                            orderSoldViewModels.Add(orderSold);
                        }
                        return orderSoldViewModels.GroupBy(p => p.Month)
                                          .Select(g => g.First())
                                          .ToList();
                    }
                    else
                    {
                        throw new Exception("Year not valid");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<List<NumberOfProfitViewModel>> GetNumberOfProfitAsync(int? year)
        {
            try
            {
                List<NumberOfProfitViewModel> numberOfProfitViewModels = new List<NumberOfProfitViewModel>();
                IEnumerable<Order> orders = (await ((OrderRepository)_unitOfWork.OrderRepository).Get()).OrderBy(x => x.OrderedDate);
                IEnumerable<ProductOrder> productOrder = await ((ProductOrderRepository)_unitOfWork.ProductOrderRepository).Get();
                IEnumerable<DescribeProduct> describeProducts = await ((DescribeProductRepository)_unitOfWork.DescribeProductRepository).Get();
                IEnumerable<Size> sizes = await ((SizeRepository)_unitOfWork.SizeRepository).Get();
                IEnumerable<Color> colors = await ((ColorRepository)_unitOfWork.ColorRepository).Get();
                if (year == null)
                {
                    Order order = orders.FirstOrDefault();
                    if(order != null)
                    {
                        NumberOfProfitViewModel numberOfProfit = new NumberOfProfitViewModel();
                        IEnumerable<ProductOrder> prd = new List<ProductOrder>();
                        prd = productOrder.Where(p => p.IdOrder == order.Id);
                        numberOfProfit.Month = order.OrderedDate.Value.Month;
                        foreach (var productOd in prd)
                        {
                            Size s = sizes.SingleOrDefault(s => s.Name.Equals(productOd.Size));
                            Color c = colors.SingleOrDefault(c => c.Name.Equals(productOd.Color));
                            IEnumerable<DescribeProduct> describeP = new List<DescribeProduct>();
                            describeP = describeProducts.Where(p => p.ProductId == productOd.IdProduct && p.ColorId == c.Id && p.SizeId == s.Id);
                            foreach (var des in describeP)
                            {
                                numberOfProfit.Profit = (decimal)(productOd.Price * productOd.Amount - (des.OriginalPrice * productOd.Amount));
                                foreach (var num in numberOfProfitViewModels)
                                {
                                    if (num.Month == numberOfProfit.Month)
                                    {
                                        numberOfProfit.Profit += num.Profit;
                                    }
                                }
                            }
                        }
                        numberOfProfitViewModels.Add(numberOfProfit);
                        return numberOfProfitViewModels.GroupBy(p => p.Month)
                                          .Select(g => g.Last())
                                          .ToList();
                    }
                    return numberOfProfitViewModels;
                }
                else
                {

                    if (orders.Where(u => u.OrderedDate.Value.Year == year).Count() != 0)
                    {
                        orders = orders.Where(u => u.OrderedDate.Value.Year == year);

                        foreach (var order in orders)
                        {
                            NumberOfProfitViewModel numberOfProfit = new NumberOfProfitViewModel();
                            IEnumerable<ProductOrder> prd = new List<ProductOrder>();
                            prd = productOrder.Where(p => p.IdOrder == order.Id);
                            numberOfProfit.Month = order.OrderedDate.Value.Month;
                            foreach (var productOd in prd)
                            {
                                Size s = sizes.SingleOrDefault(s => s.Name.Equals(productOd.Size));
                                Color c = colors.SingleOrDefault(c => c.Name.Equals(productOd.Color));
                                IEnumerable<DescribeProduct> describeP = new List<DescribeProduct>();
                                describeP = describeProducts.Where(p => p.ProductId == productOd.IdProduct && p.ColorId == c.Id && p.SizeId == s.Id);
                                foreach (var des in describeP)
                                {
                                    numberOfProfit.Profit = (decimal)(productOd.Price * productOd.Amount - (des.OriginalPrice * productOd.Amount));
                                    foreach (var num in numberOfProfitViewModels)
                                    {
                                        if (num.Month == numberOfProfit.Month)
                                        {
                                            numberOfProfit.Profit += num.Profit;
                                        }
                                    }
                                }
                            }
                            numberOfProfitViewModels.Add(numberOfProfit);
                        }
                        return numberOfProfitViewModels.GroupBy(p => p.Month)
                                          .Select(g => g.Last())
                                          .ToList();
                    }
                    else
                    {
                        throw new Exception("Year not valid");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
