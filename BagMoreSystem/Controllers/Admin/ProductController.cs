using BAL.Authorization;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #region CreateProduct
        /// <summary>
        /// create new product
        /// </summary>
        /// <param name="productViewModel">productViewModel </param>
        /// <returns>create success</returns>
        /// <response code="400">if information not valid</response>
        /// <response code="200">create new product success</response>

        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost("Create")]
        /*[PermissionAuthorize("Admin")]*/
        public async Task<IActionResult> CreateProduct([FromBody] CreatedProductViewModel productViewModel)
        {
            ProductValidator validator = new();
            var validation = validator.Validate(productViewModel);
            if (!validation.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validation.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return BadRequest(new
                {
                    status = false,
                    errors = errors
                });
            }
            //Coding session
            try
            {
                bool result = await _productService.CreateProductAsync(productViewModel);
                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "create fail",
                    });
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message,
                });
            }
        }
        #endregion

        #region UpdateProduct
        /// <summary>
        /// Update information of product
        /// </summary>
        /// <param name="productId">productId </param>
        /// <param name="productViewModel">productViewModel</param>
        /// <returns>Update success</returns>
        /// <response code="400">if information not valid</response>
        /// <response code="200">Update product success</response>

        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPut("Update/{productId}")]
        /*[PermissionAuthorize("Admin")]*/
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromBody] CreatedProductViewModel productViewModel)
        {
            ProductValidator validator = new();
            var validation = validator.Validate(productViewModel);
            if (!validation.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validation.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return BadRequest(new
                {
                    status = false,
                    errors = errors
                });
            }

            try
            {
                bool result = await _productService.UpdateProductAsync(productId, productViewModel);
                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Update success"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Updaet fail",
                    });
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message,
                });
            }
            return Ok(new
            {
                success = true,
            });

        }
        #endregion

        #region GetAllProduct
        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="productId">productId </param>
        /// <param name="productViewModel">productViewModel </param>
        /// <returns>products</returns>
        /// <response code="400">if information not valid</response>
        /// <response code="200">update product success</response>

        [ProducesResponseType(typeof(ProductViewModelAdmin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("ViewAll")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetAllProducts()
        {

            //Coding session
            try
            {
                List<ProductViewModelAdmin> result = await _productService.GetAllProductsAsyncAdmin();
                if (result.Count != 0)
                {
                    return Ok(new
                    {
                        result,
                        success = true,
                    });
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message,
                });
            }
            return Ok(new
            {
                success = true,
            });

            //End coding session
        }
        #endregion

        #region GetProductDetailAdmin
        /// <summary>
        /// Get product detail 
        /// </summary>
        /// <param name="productId">productId </param>
        /// <returns>Product detail</returns>
        /// <response code="400">if product id doesn't exist</response>
        /// <response code="200">Get product detail success</response>
        /// 
        [ProducesResponseType(typeof(ProductViewModelAdmin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("ViewDetail/{productId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetProductDetailAdmin([FromRoute] int productId)
        {

            try
            {
                ProductViewModelAdmin result = await _productService.GetProductDetailAsyncAdmin(productId);
                if (result == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Get product fail",
                    });

                }
                return Ok(new
                {
                    Data = result,
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
        }
        #endregion

        #region DeleteProduct
        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="productId">productId </param>
        /// <returns>Delete product success</returns>
        /// <response code="400">if product id not exist</response>
        /// <response code="200">Delete product success</response>
        /// 
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpDelete("Delete/{productId}")]
        /*[PermissionAuthorize("Admin")]*/
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int productId)
        {

            //Coding session
            try
            {
                bool result = await _productService.DeleteProduct(productId);
                if (result == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "delete product fail",
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "delete product success",
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

        #region Get products
        /// <summary>
        /// Get list product and include sort, search, paging
        /// </summary>
        /// <param name="keysearch"> Search by keyword </param>
        /// <param name="keysort"> Sort by NAMEASC || NAMEDESC </param>
        /// <param name="PAGE_NUMBER"> Number page want to go </param>
        ///  <param name="PAGE_SIZE"> amount of product display on screen</param>
        /// <returns> Return list of products</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keysearch": haha
        ///            "keysort": NAMEDESC
        ///            "PAGE_SIZE": 5
        ///            "PAGE_NUMBER": 2
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of products</response>
        /// <response code="404"> Nếu list null</response>

        [HttpGet("Products")]
        [ProducesResponseType(typeof(List<ProductViewModelAdmin>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetProducts([FromQuery] SearchingProductViewModel? searchingViewModel)
        {
            try
            {
                int totalPage;
                int pageSize;
                int pageNumber;
                List<ProductViewModelAdmin> productViewModel = new List<ProductViewModelAdmin>();
                Tuple<List<ProductViewModelAdmin>, int, int?, int?> result = await _productService.GetProductsAsync(searchingViewModel?.keySearchName,
                                                                              searchingViewModel?.keySortStatus, searchingViewModel?.keySortOriginalPrice, searchingViewModel?.keySortName,
                                                                              searchingViewModel?.keySortDiscount, searchingViewModel?.keySortTTProduct,
                                                                              searchingViewModel?.PAGE_NUMBER, searchingViewModel?.PAGE_SIZE);

                productViewModel = result.Item1;

                totalPage = result.Item2;
                pageNumber = (int)result.Item3!;
                pageSize = (int)result.Item4!;
                return Ok(new
                {
                    Success = true,
                    KeySearchName = searchingViewModel?.keySearchName,
                    KeySortName = searchingViewModel?.keySortName,
                    KeySortOriginalPrice = searchingViewModel?.keySortOriginalPrice,
                    KeySortTTProduct = searchingViewModel?.keySortTTProduct,
                    KeySortStatus = searchingViewModel?.keySortStatus,
                    KeySortDisCount = searchingViewModel?.keySortDiscount,
                    PageNumber = pageNumber,
                    TotalPage = totalPage,
                    PageSize = pageSize,
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
    }
}
