using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateOrderAsync(Order order);
        public Task<List<Order>> GetOrders(int userId);
        public Task<List<Order>> GetUserOrders();
        public void UpdateOrder(Order order);
    }
}
