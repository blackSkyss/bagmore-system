using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BAL.Authorization;

namespace BagMoreSystem.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;

        public ProductController(IProductService productService,
                                 ICategoryService categoryService,
                                 IColorService colorService,
                                 ISizeService sizeService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sizeService = sizeService;
        }

        #region Get products
        /// <summary>
        /// Get list product and include sort, search, filter
        /// </summary>
        /// 
        /// <param name="sortBy"> Sort by name ASC || name DESC || Price ASC || Price DES </param>
        /// <param name="filterbycategory"> Filter by list id of category </param>
        /// <param name="filterbycolor"> Filter by list id of color</param>
        /// <param name="filterbysize"> Filter by id of size</param>
        /// <returns>Return list of product</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            {
        ///            "sortBy": NAMEDESC
        ///            "filterbycategory": 1
        ///            "filterbycolor": 1
        ///            "filterbysize": 1
        ///            }
        ///     
        /// </remarks>
        /// <response code="200">Trả về list of product Model</response>
        /// <response code="404">nếu list null</response>

        [HttpGet("Products")]
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetProducts([FromQuery] SearchingProductFromUserViewModel? searchingProductViewModel)
        {

            try
            {
                List<ProductViewModel> productViewModel = new List<ProductViewModel>();
                productViewModel = await _productService.GetProductsAsync(searchingProductViewModel?.sortby, searchingProductViewModel?.filterbycategory, searchingProductViewModel?.filterbycolor, searchingProductViewModel?.filterbysize);

                if (productViewModel == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List product is empty!"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Sortby = searchingProductViewModel?.sortby,
                    FilterByCategory = searchingProductViewModel?.filterbycategory,
                    FilterByColor = searchingProductViewModel?.filterbycolor,
                    FilterBySize = searchingProductViewModel?.filterbysize,
                    Data = productViewModel,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        #endregion

        #region Search product
        /// <summary>
        /// Search product
        /// </summary>
        /// 
        /// <param name="keyword">keyword to search</param>
        /// <returns>Return list categori</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keyword": haha
        ///            
        ///     
        /// </remarks>
        /// <response code="200">Trả về list of category Model</response>
        /// <response code="404">nếu list null</response>
        /// 
        [HttpGet("SearchProduct")]
        [ProducesResponseType(typeof(List<SearchCategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? keyword)
        {

            try
            {
                if (keyword == null)
                {
                    return Ok(new
                    {
                        Success = false,
                        Data = "No keysearch!"
                    });
                }

                List<SearchCategoryViewModel> categoryViewModels = new List<SearchCategoryViewModel>();
                categoryViewModels = await _categoryService.GetCategoryByKeywordAsync(keyword);

                if (categoryViewModels.Count() == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "Category not found!"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Keyword = keyword,
                    Data = categoryViewModels,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Get categories
        /// <summary>
        /// Get list category
        /// </summary>
        /// 
        /// <returns>Return list category</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///  
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of category Model</response>
        /// <response code="404"> Nếu list null</response>
        /// 
        [HttpGet("Categories")]
        [ProducesResponseType(typeof(List<CategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetCategories()
        {

            try
            {
                List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
                categoryViewModels = await _categoryService.GetAllCategoriesAsync();

                if (categoryViewModels.Count() == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List category is empty!"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = categoryViewModels,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Get colors
        /// <summary>
        /// Get list color
        /// </summary>
        /// 
        /// <returns>Return list color</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///  
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of color Model</response>
        /// <response code="404"> Nếu list null</response>
        /// 
        [HttpGet("Colors")]
        [ProducesResponseType(typeof(List<ColorViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetColors()
        {

            try
            {
                List<ColorViewModel> colorViewModels = new List<ColorViewModel>();
                colorViewModels = await _colorService.GetColorsAsync();

                if (colorViewModels.Count() == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List color is empty!"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = colorViewModels,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Get sizes
        /// <summary>
        /// Get list size
        /// </summary>
        /// 
        /// <returns>Return list size</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///  
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of size Model</response>
        /// <response code="404"> Nếu list null</response>
        /// 
        [HttpGet("Sizes")]
        [ProducesResponseType(typeof(List<SizeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetSizes()
        {

            try
            {
                List<SizeViewModel> sizeViewModels = new List<SizeViewModel>();
                sizeViewModels = await _sizeService.GetSizesAsync();

                if (sizeViewModels.Count() == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List size is empty!"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = sizeViewModels,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion
        #region GetProductDetail
        /// <summary>
        /// Get detail of product
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>View product detail</returns>
        /// <response code="400">If Id product doesn't exist</response>
        /// <response code="200">View product detail</response>

        [ProducesResponseType(typeof(ProductDetailViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("Get/{productId}")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetProductDetail([FromRoute] int productId)
        {
            //Coding session
            try
            {
                ProductDetailViewModel productDetail = await _productService.GetProductInformation(productId);
                return Ok(new
                {
                    productDetail,
                    success = true,
                });

            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message,
                });
            }
            //End coding session
        }
        #endregion
    }
}
