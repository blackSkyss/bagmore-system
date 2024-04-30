using AutoMapper;
using BAL.Helpers;
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
    public class OrderService : IOrderService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task<List<OrderViewModel>> GetOrders(string username)
        {
            try
            {
                User existedUser = await this._unitOfWork.UserRepository.GetUserAsync(username);
                List<Order> orders = await this._unitOfWork.OrderRepository.GetOrders(existedUser.Id);
                List<OrderViewModel> orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);
                foreach (var order in orders)
                {
                    foreach (var orderViewModel in orderViewModels)
                    {
                        if (order.Id == orderViewModel.Id)
                        {
                            orderViewModel.products = _mapper.Map<List<OrderProductsViewModel>>(order.ProductOrders);
                            foreach (var orderProduct in order.ProductOrders)
                            {
                                foreach (var product in orderViewModel.products)
                                {
                                    if(orderProduct.IdProduct == product.ProductId)
                                    {
                                        product.productImages = _mapper.Map<List<ProductImageViewModel>>(orderProduct.Product.ProductImages);
                                    }
                                }
                            }
                        }
                    }
                }

                return orderViewModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderUserDetailViewModel> getOrderUserDetailAsync(int orderId)
        {
            OrderUserDetailViewModel orderUserDetailViewModel = new OrderUserDetailViewModel();
            List<Product> products = new List<Product>();
            List<Product> getAllproducts = (List<Product>)await ((ProductRepository)_unitOfWork.ProductRepository).Get();
            List<DescribeProduct> describeProducts = new List<DescribeProduct>();
            IEnumerable<Size> sizes = await ((SizeRepository)_unitOfWork.SizeRepository).Get();
            IEnumerable<Color> colors = await ((ColorRepository)_unitOfWork.ColorRepository).Get();
            Order order = await ((OrderRepository)_unitOfWork.OrderRepository).GetByID(orderId);
            if(order == null)
            {
                throw new Exception("OrderId does not exist");
            }
            User user = await ((UserRepository)_unitOfWork.UserRepository).GetByID(order.UserID);
            DeliveryMethod deliveryMethod = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).GetByID(order.DeliveryMethodId);
            IEnumerable<ProductOrder> productOrders = await ((ProductOrderRepository)_unitOfWork.ProductOrderRepository).Get();
            List<DescribeProduct> describeProductsGetAll = (List<DescribeProduct>)await ((DescribeProductRepository)_unitOfWork.DescribeProductRepository).Get();
            productOrders = productOrders.Where(p => p.IdOrder == order.Id);
            foreach (var productOd in productOrders)

            {
                Size s = sizes.SingleOrDefault(s => s.Name.Equals(productOd.Size));
                Color c = colors.SingleOrDefault(c => c.Name.Equals(productOd.Color));
                DescribeProduct describeProductWithInfor = describeProductsGetAll.SingleOrDefault(p => p.ProductId == productOd.IdProduct && p.ColorId == c.Id && p.SizeId == s.Id);
                describeProducts.Add(describeProductWithInfor);
                Product proWithId = getAllproducts.SingleOrDefault(p => p.Id == productOd.IdProduct);
                products.Add(proWithId);
            }

            var combinedListOrderDetail = new List<CombinedOrderDetailItemViewModel>();
            List<OrderUserDetailItemViewModel> orderUserDetailItemViewModels = new List<OrderUserDetailItemViewModel>();

            foreach (var productOd in productOrders)
            {
                var combinedItem = new CombinedOrderDetailItemViewModel
                {
                    Amount = (int)productOd.Amount,
                    DeliveryPrice = productOd.Order.DeliveryMethodPrice
                };
                combinedListOrderDetail.Add(combinedItem);
            }

            for (int i = 0; i < products.Count; i++)
            {
                var combinedItem = combinedListOrderDetail[i];
                var pro = products[i];
                combinedItem.ProductName = pro.Name;
                combinedItem.Discount = pro.Discount.Value;
            }

            for (int i = 0; i < describeProducts.Count; i++)
            {
                var combinedItem = combinedListOrderDetail[i];
                var des = describeProducts[i];
                combinedItem.Price = des.Price.Value;
            }

            foreach (var cbl in combinedListOrderDetail)
            {
                orderUserDetailViewModel.SubTotal += (cbl.Price * cbl.Amount) - (cbl.Price * cbl.Amount * (decimal)cbl.Discount);
                orderUserDetailViewModel.Total += (cbl.Price * cbl.Amount) - (cbl.Price * cbl.Amount * (decimal)cbl.Discount) + cbl.DeliveryPrice;
            }
            _mapper.Map(combinedListOrderDetail, orderUserDetailItemViewModels);

            orderUserDetailViewModel.FullName = user.FirstName + user.LastName;
            orderUserDetailViewModel.Email = user.Email;
            orderUserDetailViewModel.Phone = user.Phone;
            orderUserDetailViewModel.OrderId = order.Id;
            orderUserDetailViewModel.DeliveryStatus = order.DeliveryStatus;
            orderUserDetailViewModel.DeliveryMethod = order.DeliveryMethod.Name;
            orderUserDetailViewModel.OrderedDate = order.OrderedDate.Value;
            orderUserDetailViewModel.ShippingAddress = order.ShippingAddress;
            orderUserDetailViewModel.ShippingPrice = order.DeliveryMethodPrice;
            orderUserDetailViewModel.orderUserDetailItemViewModels = orderUserDetailItemViewModels;
            return orderUserDetailViewModel;
        }

        public async Task<List<UserOrderViewModel>> GetUserOrders(SearchingUserOrderViewModel searchingUserOrder)
        {
            try
            {
                List<Order> orders = await this._unitOfWork.OrderRepository.GetUserOrders();
                if(string.IsNullOrWhiteSpace(searchingUserOrder.SearchValue) == false)
                {
                    orders = orders.Where(x => StringHelper.ConvertToUnSign(x.User.FirstName).Contains(StringHelper.ConvertToUnSign(searchingUserOrder.SearchValue)) || StringHelper.ConvertToUnSign(x.User.LastName).Contains(StringHelper.ConvertToUnSign(searchingUserOrder.SearchValue))).ToList();
                }
                if(string.IsNullOrWhiteSpace(searchingUserOrder.SortFullName) == false)
                {
                    switch (searchingUserOrder.SortFullName)
                    {
                        case "fullname_desc":
                            orders = orders.OrderByDescending(x => x.User.LastName).ToList();
                            break;
                        case "fullname_asc":
                            orders = orders.OrderBy(x => x.User.FirstName).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortEmail) == false)
                {
                    switch (searchingUserOrder.SortEmail)
                    {
                        case "email_desc":
                            orders = orders.OrderByDescending(x => x.User.Email).ToList();
                            break;
                        case "email_asc":
                            orders = orders.OrderBy(x => x.User.Email).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortOrderDate) == false)
                {
                    switch (searchingUserOrder.SortOrderDate)
                    {
                        case "orderedDate_desc":
                            orders = orders.OrderByDescending(x => x.OrderedDate).ToList();
                            break;
                        case "orderedDate_asc":
                            orders = orders.OrderBy(x => x.OrderedDate).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortDeliveryMethod) == false)
                {
                    switch (searchingUserOrder.SortDeliveryMethod)
                    {
                        case "sortDeliveryMethod_desc":
                            orders = orders.OrderByDescending(x => x.DeliveryMethod.Name).ToList();
                            break;
                        case "sortDeliveryMethod_asc":
                            orders = orders.OrderBy(x => x.DeliveryMethod.Name).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortTotalProduct) == false)
                {
                    switch (searchingUserOrder.SortTotalProduct)
                    {
                        case "sortTotalProduct_desc":
                            orders = orders.OrderByDescending(x => x.ProductOrders.Count()).ToList();
                            break;
                        case "sortTotalProduct_asc":
                            orders = orders.OrderBy(x => x.ProductOrders.Count()).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortTotal) == false)
                {
                    switch (searchingUserOrder.SortTotal)
                    {
                        case "sortTotal_desc":
                            orders = orders.OrderByDescending(x => x.ProductOrders.Sum(po => po.Price)+x.DeliveryMethod.Price).ToList();
                            break;
                        case "sortTotal_asc":
                            orders = orders.OrderBy(x => x.ProductOrders.Sum(po => po.Price) + x.DeliveryMethod.Price).ToList();
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(searchingUserOrder.SortStatus) == false)
                {
                    switch (searchingUserOrder.SortStatus)
                    {
                        case "sortStatus_desc":
                            orders = orders.OrderByDescending(x => x.DeliveryStatus).ToList();
                            break;
                        case "sortStatus_asc":
                            orders = orders.OrderBy(x => x.DeliveryStatus).ToList();
                            break;
                    }
                }
                int totalPage = (int)Math.Ceiling((double)orders.Count / searchingUserOrder.ItemPerPage.Value);
                if (searchingUserOrder.CurrentPage != null && searchingUserOrder.ItemPerPage != null)
                {
                    orders = orders.Skip((searchingUserOrder.CurrentPage.Value - 1) * searchingUserOrder.ItemPerPage.Value)
                        .Take(searchingUserOrder.ItemPerPage.Value).ToList();
                } else
                {
                    if(searchingUserOrder.CurrentPage == null)
                    {
                        throw new Exception("Current page is required!");
                    }
                    if(searchingUserOrder.ItemPerPage == null)
                    {
                        throw new Exception("Items per page are required!");
                    }
                }
                searchingUserOrder.totalPage = totalPage;
                return _mapper.Map<List<UserOrderViewModel>>(orders);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderStatus(int idOrder, int orderStatus)
        {
            try
            {
                Order order = (await ((OrderRepository)this._unitOfWork.OrderRepository).Get(x => x.Id == idOrder)).First();
                if(order == null)
                {
                    throw new Exception("Order does not exist!");
                }
                order.DeliveryStatus = orderStatus;
                this._unitOfWork.OrderRepository.UpdateOrder(order);
                await this._unitOfWork.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
