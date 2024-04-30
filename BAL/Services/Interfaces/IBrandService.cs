using BAL.Models;

namespace BAL.Services.Interfaces
{
    public interface IBrandService
    {
        public Task<bool> CreateBrand(BrandCreatedViewModel brandViewModel);
        public Task<List<BrandGetInforViewModel>> GetAllBrands();
        public Task<BrandGetInforViewModel> GetBrandDetail(int brandId, int itemsPerPage, int currentPage);
        public Task<bool> UpdateBrand(int brandId, BrandCreatedViewModel brandViewModel);
        public Task<bool> DeleteBrand(int brandId);
        public Task<Tuple<List<BrandViewModel>, int, int?, int?>> GetBrandsAsync(string? keySearchName,
                                                                                 string? keySortStatus, 
                                                                                 string? keySortTTProduct,
                                                                                 string? keySortName,
                                                                                 int? PAGE_NUMBER, 
                                                                                 int? PAGE_SIZE);

    };
}
