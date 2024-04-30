using BAL.Authorization;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private IColorService colorService;
        private IBrandService brandService;
        private ICategoryService categoryService;
        private ISizeService sizeService;
        public ISupplierService supplierService;
        public MetaDataController(IColorService colorService ,IBrandService brandService, ICategoryService categoryService, ISizeService sizeService,
            ISupplierService supplierService)
        {
            this.colorService = colorService;
            this.brandService = brandService;
            this.categoryService = categoryService;
            this.sizeService = sizeService;
            this.supplierService = supplierService;
        }

        [HttpGet("MetaDatas")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetMetaData()
        {
            try
            {
                List<ColorViewModel> colorViewModels = await colorService.GetColorsAsync();
                List<BrandGetInforViewModel> brandViewModels = await brandService.GetAllBrands();
                List<CategoryViewModel> categoryViewModels = await categoryService.GetAllCategoriesAsync();
                List<SizeViewModel> sizeViewModels = await sizeService.GetSizesAsync();
                List<SupplierViewModel> suppliersViewModel = await supplierService.GetSuppliers();
                return Ok(new
                {
                    Success = true,
                    Colors = colorViewModels,
                    Sizes = sizeViewModels,
                    Brands = brandViewModels,
                    Categories = categoryViewModels,
                    Suppliers = suppliersViewModel
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            
        }
    }
}
