using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDeliveryMethodRepository
    {
        public Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int deliveryMethodId);
    }
}
