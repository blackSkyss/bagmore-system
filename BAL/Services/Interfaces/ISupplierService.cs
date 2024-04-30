using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface ISupplierService
    {
        public Task<Tuple<List<SupplierViewModel>, int, int?, int?>> GetSuppliersAsync(string? keysearch, string? keysortname, string? keysortemail, string? keysortphone, string? keySortTTProduct, string? keysortstatus, int? PAGE_SIZE, int? PAGE_NUMBER);

        public Task<SupplierViewModel> GetSupplierDetailAsync(int id);

        public Task<bool> InsertSupplierAsync(EditSupplierViewModel model);
        public Task<bool> UpdateSupplierAsync(int id, EditSupplierViewModel model);
        public Task<bool> DeleteSupplierAsync(int id);
        public Task<List<SupplierViewModel>> GetSuppliers();
    }
}
