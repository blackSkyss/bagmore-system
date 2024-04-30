using BAL.Authorization;
using BAL.Helpers;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Validator;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        //Để get byte[]
        [HttpPost("image")]
        public async Task<IActionResult> getBinary(IFormFile image)
        {
            byte[] bytes = await ImageHelper.ImageToBase64(image);
            return Ok(new
            {
                data = bytes
            });
        }

        #region CreateBrand
        /// <summary>
        /// CreateBrandAsync
        /// </summary>
        /// <param name="brandViewModel">brandViewModel </param>
        /// <returns>create success</returns>
        /// <response code="400">if information not valid</response>
        /// <response code="200">create new brand success</response>

        [ProducesResponseType(typeof(BrandCreatedViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost("Create")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandCreatedViewModel brandViewModel)
        {
            
            BrandValidator validator = new();
            var validation = validator.Validate(brandViewModel);
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
                bool result = await _brandService.CreateBrand(brandViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        message = "Create fail",
                        success = false,
                    });

                }
                return Ok(new
                {
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

        #region UpdateBrand
        /// <summary>
        /// UpdateBrand
        /// </summary>
        /// <param name="brandViewModel">brandViewModel</param>
        /// <param name="brandId">brandId</param>
        /// <returns>update success</returns>
        /// <response code="400">if brand id doesn't exist not valid</response>
        /// <response code="200">update brand success</response>

        [ProducesResponseType(typeof(BrandViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPut("Update/{brandId}")]
        /*[PermissionAuthorize("Admin")]*/
        public async Task<IActionResult> UpdateBrand([FromRoute] int brandId, [FromBody] BrandCreatedViewModel brandViewModel)
        {
            BrandValidator validator = new();
            var validation = validator.Validate(brandViewModel);
            if (!validation.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validation.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return BadRequest(new
                {
                    Success = false,
                    Message = errors
                });
            }

            //Coding session
            try
            {
                bool result = await _brandService.UpdateBrand(brandId, brandViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Update brand fail",
                    });
                }
                return Ok(new
                {
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

        #region GetAllBrands
        /// <summary>
        /// GetAllBrandsAsync
        /// </summary>
        /// <returns>Brands</returns>
        /// <response code="400">if don't have brand</response>
        /// <response code="200"> Get all brands</response>
        [ProducesResponseType(typeof(BrandGetInforViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("GetAll")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetAllBrandsAsync()
        {
            //Coding session
            try
            {
                List<BrandGetInforViewModel> brands = await _brandService.GetAllBrands();
                if (brands.Count != 0)
                {
                    return Ok(new
                    {
                        Data = brands,
                        success = true,
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Can't load brand",
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
            //End coding session
        }
        #endregion

        #region GetBrandDetail
        /// <summary>
        /// GetBrandDetail
        /// </summary>
        /// <returns>Brand detail</returns>
        /// <response code="400">if id brand doesn't exits</response>
        /// <response code="200">Get brand detail</response>
       
        [ProducesResponseType(typeof(BrandGetInforViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("Get/Detail/{brandId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetBrandDetail(int brandId, int itemsPerPage, int currentPage)
        {
            //Coding session
            try
            {
                BrandGetInforViewModel brand = await _brandService.GetBrandDetail(brandId, itemsPerPage, currentPage);
                if (brand == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Can't load brand",
                    });
                }
                return Ok(new
                {
                    Data = brand,
                    success = true,
                    currentPage = currentPage,
                    itemsPerPage = itemsPerPage
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

        #region DeleteBrand
        /// <summary>
        /// DeleteBrand
        /// </summary>
        /// <returns>detele success</returns>
        /// <response code="400">if id brand doesn't exits</response>
        /// <response code="200">detele success</response>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "Id": 1
        ///
        /// </remarks>

        [ProducesResponseType(typeof(BrandViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpDelete("Detele/{brandId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            //Coding session
            try
            {
                bool result = await _brandService.DeleteBrand(brandId);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Can't load brand",
                    });

                }
                return Ok(new
                {
                    message = "Delete success",
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

        #region Get brands
        /// <summary>
        /// Get list brands and include sort, search, paging
        /// </summary>
        /// <param name="keysearch"> Search by keyword </param>
        /// <param name="keysort"> Sort by NAMEASC || NAMEDESC </param>
        /// <param name="PAGE_NUMBER"> Number page want to go </param>
        ///  <param name="PAGE_SIZE"> Amount of brand display on screen</param>
        /// <returns> Return list of brands</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keysearch": haha
        ///            "keysort": NAMEDESC
        ///            "PAGE_SIZE": 10
        ///            "PAGE_NUMBER": 2
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of brands</response>
        /// <response code="404"> Nếu list null</response>

        [HttpGet("Brands")]
        [ProducesResponseType(typeof(List<BrandViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetBrands([FromQuery] SearchingBrandViewModel? searchingViewModel)
        {
            try
            {
                int totalPage;
                int pageSize;
                int pageNumber;
                List<BrandViewModel> brandViewModel = new List<BrandViewModel>();
                Tuple<List<BrandViewModel>, int,int?,int?> result = await _brandService.GetBrandsAsync(searchingViewModel?.keySearchName,
                                                                                                       searchingViewModel?.keySortStatus,
                                                                                                       searchingViewModel?.keySortName,
                                                                                                       searchingViewModel?.keySortTTProduct,
                                                                                                       searchingViewModel?.PAGE_NUMBER,
                                                                                                       searchingViewModel?.PAGE_SIZE);

                brandViewModel = result.Item1;

                totalPage = result.Item2;
                pageNumber = (int)result.Item3!;
                pageSize = (int)result.Item4!;
                return Ok(new
                {
                    Success = true,
                    KeySearchName = searchingViewModel?.keySearchName,
                    KeySortName = searchingViewModel?.keySortName,
                    KeySortTTProduct = searchingViewModel?.keySortTTProduct,
                    KeySortStatus = searchingViewModel?.keySortStatus,
                    PageNumber = pageNumber,
                    TotalPage = totalPage,
                    Data = brandViewModel,
                    PageSize = pageSize,
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
