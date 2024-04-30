using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<SearchCategoryViewModel>> GetCategoryByKeywordAsync(string keyword);

        public Task<List<CategoryViewModel>> GetAllCategoriesAsync();

        public Task<Tuple<List<CategoryViewModel>, int, int?, int?>> GetCategoriesAsync(string? keysearch, string? keysortname, string? keysortstatus, string? keySortTTProduct, int? PAGE_SIZE, int? PAGE_NUMBER);

        public Task<CategoryViewModel> GetCategoryDetailAsync(int id, int? currentPage,int?  itemPerPages);

        public Task<bool> InsertCategoryAsync(EditCategoryViewModel category);

        public Task<bool> UpdateCategoryAsync(int id, EditCategoryViewModel category);

        public Task<bool> DeleteCategoryAsync(int id);
    }
}
