using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IDeliveryMethodService
    {
        public Task<Tuple<List<DeliveryMethodViewModel>, int, int?, int?>> GetDeliveryMethodsAsync(string? keysearch, string? keysortname, string? keysortprice, string? keysortstatus, int? PAGE_SIZE, int? PAGE_NUMBER);

        public Task<DeliveryMethodViewModel> GetDeliveryMethodDetailAsync(int id);

        public Task<bool> InsertDeliveryMethodAsync(EditDeliveryMethodViewModel model);
        public Task<bool> UpdateDeliveryMethodAsync(int id, EditDeliveryMethodViewModel model);
        public Task<bool> DeleteDeliveryMethodAsync(int id);

        public Task<List<DeliveryMethodUserViewModel>> GetDeliveryUser();


    }
}
