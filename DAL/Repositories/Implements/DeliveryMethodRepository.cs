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
    public class DeliveryMethodRepository : RepositoryBase<DeliveryMethod>, IDeliveryMethodRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public DeliveryMethodRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int deliveryMethodId)
        {
            try
            {
                return await this._bagMoreDbContext.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == deliveryMethodId
                && x.Status == (int)DeliveryMethodEnum.Status.ACTIVE);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
