using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class SuppliedProductRepository : RepositoryBase<SuppliedProduct>, ISuppliedProductRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public SuppliedProductRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            this._bagMoreDbContext = dbContext;
        }
    }
}
