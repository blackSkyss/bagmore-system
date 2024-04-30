using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategoriesAsync();

        Task<List<Category>> GetCategoriesByKeyWord(string keyword);
        public Task<Category> GetCategoryById(int id);
    }
}
