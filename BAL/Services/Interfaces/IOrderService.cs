using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderUserDetailViewModel> getOrderUserDetailAsync(int orderId);
        public Task<List<OrderViewModel>> GetOrders(string username);
        public Task<List<UserOrderViewModel>> GetUserOrders(SearchingUserOrderViewModel searchingUserOrder);
        public Task UpdateOrderStatus(int idOrder, int orderStatus);
    }
}
