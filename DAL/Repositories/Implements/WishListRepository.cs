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
    public class WishListRepository : RepositoryBase<WishList>, IWishListRepository
    {
        private BagMoreDbContext _dbContext;
        public WishListRepository(BagMoreDbContext dbContext): base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
