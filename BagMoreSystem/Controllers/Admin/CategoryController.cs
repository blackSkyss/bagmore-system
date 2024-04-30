using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #region Get categories
        /// <summary>
        /// Get list categories and include sort, search, paging
        /// </summary>
        /// 
        /// <param name="keysearch"> Search by keyword </param>
        /// <param name="keysortname"> Sort by NAMEASC || NAMEDESC </param>
        /// <param name="keysortstatus"> Sort by STATUSASC || STATUSDESC </param>
        /// <param name="PAGE_SIZE"> Number of item in one page </param>
        /// <param name="PAGE_NUMBER"> Number page want to go </param>
        /// <returns> Return list of categories</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keysearch": "haha"
        ///            "keysortname": "NAMEDESC"
        ///            "keysortstatus": "STATUSDES"
        ///            "PAGE_SIZE": 5
        ///            "PAGE_NUMBER": 2
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of categories Model</response>
        /// <response code="404"> Nếu list null</response>

        [HttpGet("Categories")]
        [ProducesResponseType(typeof(List<CategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetCategories([FromQuery] SearchingCategoryViewModel? searchingViewModel)
        {

            try
            {
                int totalPage;
                int pageSize;
                int pageNumber;
                List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();
                Tuple<List<CategoryViewModel>, int, int?, int?> result = await _categoryService.GetCategoriesAsync(searchingViewModel?.keysearch,
                                                                             searchingViewModel?.keysortname,
                                                                             searchingViewModel?.keysortstatus,
                                                                             searchingViewModel?.keySortTTProduct,
                                                                             searchingViewModel?.PAGE_SIZE,
                                                                             searchingViewModel?.PAGE_NUMBER);

                categoryViewModel = result.Item1;


                totalPage = result.Item2;
                pageSize = (int)result.Item3!;
                pageNumber = (int)result.Item4!;

                return Ok(new
                {
                    Success = true,
                    KeySearch = searchingViewModel?.keysearch,
                    KeySortName = searchingViewModel?.keysortname,
                    KeySortStatus = searchingViewModel?.keysortstatus,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    TotalPage = totalPage,
                    Data = categoryViewModel,
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

        #region Get category detail
        /// <summary>
        /// Get detail information of category
        /// </summary>
        /// 
        /// <param name="id"> id of category </param>
        /// <returns> Return detail information of category</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "id": 3
        ///     
        /// </remarks>
        /// <response code="200">Trả về detail information of category</response>
        /// <response code="404">nếu list null</response>

        [HttpGet("CategoryDetail/{id}")]
        [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetCategoryDetail([FromRoute] int id, [FromQuery]int? currentPage, [FromQuery]int? itemPerPage)
        {
            try
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel();
                categoryViewModel = await _categoryService.GetCategoryDetailAsync(id, currentPage, itemPerPage);

                if (categoryViewModel == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "Category not found!",
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = categoryViewModel,
                    CurrentPage = currentPage,
                    ItemPerPage = itemPerPage
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

        #region Insert category
        /// <summary>
        /// Insert category
        /// </summary>
        /// 
        /// <param name="categoryViewModel">Object Type: EditCategoryViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            POST
        ///            "Name": Stream
        ///            "Status": 0
        ///     
        /// </remarks>
        /// <response code="201">Insert success</response>
        /// <response code="400">Insert fail</response>

        [HttpPost("InsertCategory")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> InsertCategory([FromBody] EditCategoryViewModel categoryViewModel)
        {
            try
            {
                bool result = false;

                if (categoryViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string> { "Invalid category format." }
                    });
                }

                #region Validation
                CategoryValidator categoryValidator = new CategoryValidator();
                var resultValid = categoryValidator.Validate(categoryViewModel);
                if (!resultValid.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var error in resultValid.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    return BadRequest(new
                    {
                        Success = result,
                        Message = errors
                    });
                }
                #endregion

                result = await _categoryService.InsertCategoryAsync(categoryViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        ErrorMessage = new List<string> { "Insert failed!" }
                    });
                }

                return Ok(new
                {
                    Success = result,
                    Message = new List<string> { "Insert successed" },
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = new List<string> { ex.Message }
                });
            }
        }
        #endregion

        #region Update category
        /// <summary>
        /// Update category
        /// </summary>
        /// 
        /// <param name="id"> Id of category </param>
        /// <param name="categoryViewModel">Object Type: EditCategoryViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            PUT
        ///            "id": 4
        ///            "Name": Thanh haha
        ///            "Status": 0
        ///     
        /// </remarks>
        /// <response code="200">Update success</response>
        /// <response code="400">Update fail</response>

        [HttpPut("UpdateCategory/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] EditCategoryViewModel categoryViewModel)
        {

            try
            {
                bool result = false;

                if (categoryViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string> { "Invalid category format." }
                    });
                }

                #region Validation
                CategoryValidator categoryValidator = new CategoryValidator();
                var resultValid = categoryValidator.Validate(categoryViewModel);
                if (!resultValid.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var error in resultValid.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    return BadRequest(new
                    {
                        Success = result,
                        Message = errors
                    });
                }
                #endregion

                result = await _categoryService.UpdateCategoryAsync(id, categoryViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string> { "Update failed!" }
                    });
                }

                return Ok(new
                {
                    Success = result,
                    Message = new List<string> { "Update successed" },
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = new List<string> { ex.Message }
                });
            }
        }
        #endregion

        #region Delete category
        /// <summary>
        /// Delete category
        /// </summary>
        /// 
        /// <param name="id"> Id of category </param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            DELETE
        ///            "id": 3
        ///     
        /// </remarks>
        /// <response code="200">Delete success</response>
        /// <response code="400">Delete fail</response>

        [HttpDelete("DeleteCategory/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {

            try
            {
                bool result = false;
                result = await _categoryService.DeleteCategoryAsync(id);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        ErrorMessage = "Delete failed!"
                    });
                }

                return Ok(new
                {
                    Success = result,
                    Message = "Delete successed",
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
