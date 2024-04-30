using DAL.Entities;
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
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public SupplierRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            this._bagMoreDbContext = dbContext;
        }

        public async Task<Supplier> GetSupplierById(int id)
        {
            try
            {
                return await this._bagMoreDbContext.Suppliers.Include(x => x.SuppliedProducts)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.DescribeProducts)
                    .Include(x => x.SuppliedProducts)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
