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
    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public ColorRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }
    }
}
