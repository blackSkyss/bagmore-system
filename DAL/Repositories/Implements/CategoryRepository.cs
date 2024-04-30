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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public CategoryRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        #region Get category and product
        public IQueryable<Category> GetCategoriesAsync()
        {
            return _bagMoreDbContext.Categories.AsQueryable();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                return await this._bagMoreDbContext.Categories.Include(x => x.Products).ThenInclude(x => x.DescribeProducts)
                    .Include(x => x.Products).ThenInclude(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Category>> GetCategoriesByKeyWord(string keyword)
        {
            return await GetCategoriesAsync().Where(c => c.Products.Any(p => p.Name.Trim().ToLower().Contains(keyword.Trim().ToLower())) && c.Status == 1)
                                             .Include(c => c.Products)
                                             .ThenInclude(p => p.DescribeProducts)
                                             .Include(c => c.Products)
                                             .ThenInclude(p => p.ProductImages)
                                             .ToListAsync();
        }
        #endregion
    }
}
