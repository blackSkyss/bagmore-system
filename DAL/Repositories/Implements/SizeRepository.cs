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
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public SizeRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            this._bagMoreDbContext = dbContext;
        }
    }
}
