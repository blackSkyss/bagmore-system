using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public OrderRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task CreateOrderAsync(Order order)
        {
            try
            {
                await this._bagMoreDbContext.Orders.AddAsync(order);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Order>> GetOrders(int userId)
        {
            try
            {
                return await this._bagMoreDbContext.Orders
                                    .Include(x => x.DeliveryMethod)
                                    .Include(x => x.ProductOrders).ThenInclude(x => x.Product).ThenInclude(x => x.DescribeProducts)
                                    .Include(x => x.ProductOrders).ThenInclude(x => x.Product).ThenInclude(x => x.DescribeProducts).ThenInclude(x => x.Color)
                                    .Include(x => x.ProductOrders).ThenInclude(x => x.Product).ThenInclude(x => x.DescribeProducts).ThenInclude(x => x.Size)
                                    .Include(x => x.ProductOrders).ThenInclude(x => x.Product).ThenInclude(x => x.ProductImages)
                                    .Where(x => x.UserID == userId && x.Status == (int)OrderEnum.Status.ACTIVE).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Order>> GetUserOrders()
        {
            try
            {
                return await this._bagMoreDbContext.Orders
                                .Include(x => x.DeliveryMethod)
                                .Include(x => x.User)
                                .Include(x => x.ProductOrders).OrderBy(x => x.Id).ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                this._bagMoreDbContext.Entry<Order>(order).State = EntityState.Modified;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
