using BAL.Authorization;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        #region Get suppliers
        /// <summary>
        /// Get list supplier and include sort, search, paging
        /// </summary>
        /// 
        /// <param name="keysearch"> Search by keyword </param>
        /// <param name="keysortname"> Sort by NAMEASC || NAMEDESC </param>
        /// <param name="keysortemail"> Sort by EMAILASC || EMAILDESC </param>
        /// <param name="keysortphone"> Sort by PHONEASC || PHONEDESC </param>
        /// <param name="keysortstatus"> Sort by STATUSASC || STATUSDESC </param>
        /// <param name="PAGE_SIZE"> Number of item in one page </param>
        /// <param name="PAGE_NUMBER"> Number page want to go </param>
        /// <returns> Return list of supplier</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keysearch": "la"
        ///            "keysortname": "NAMEDESC"
        ///            "keysortemail": "EMAILASC"
        ///            "keysortphone": "PHONEASC"
        ///            "keysortstatus": "STATUSDESC"
        ///            "PAGE_SIZE": 5
        ///            "PAGE_NUMBER": 2
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of supplier</response>
        /// <response code="404"> Nếu list null</response>

        [HttpGet("Suppliers")]
        [ProducesResponseType(typeof(List<SupplierViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetSuppliers([FromQuery] SearchingSupplierViewModel? searchingViewModel)
        {

            try
            {
                int totalPage;
                int pageSize;
                int pageNumber;
                List<SupplierViewModel> supplierViewModels = new List<SupplierViewModel>();
                Tuple<List<SupplierViewModel>, int, int?, int?> result = await _supplierService.GetSuppliersAsync(searchingViewModel?.keysearch,
                                                                             searchingViewModel?.keysortname,
                                                                             searchingViewModel?.keysortemail,
                                                                             searchingViewModel?.keysortphone,
                                                                             searchingViewModel?.keySortTTProduct,
                                                                             searchingViewModel?.keysortstatus,
                                                                             searchingViewModel?.PAGE_SIZE,
                                                                             searchingViewModel?.PAGE_NUMBER);
                supplierViewModels = result.Item1;

                totalPage = result.Item2;
                pageSize = (int)result.Item3!;
                pageNumber = (int)result.Item4!;

                return Ok(new
                {
                    Success = true,
                    KeySearch = searchingViewModel?.keysearch,
                    KeySortName = searchingViewModel?.keysortname,
                    KeySortEmail = searchingViewModel?.keysortemail,
                    KeySortPhone = searchingViewModel?.keysortphone,
                    KeySortStatus = searchingViewModel?.keysortstatus,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    TotalPage = totalPage,
                    Data = supplierViewModels,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region Get detail supplier
        /// <summary>
        /// Get detail information of supplier
        /// </summary>
        /// 
        /// <param name="id"> id of supplier </param>
        /// <returns> Return detail information of supplier</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "id": 5
        ///     
        /// </remarks>
        /// <response code="200"> Trả về detail information of supplier</response>
        /// <response code="404"> Nếu detail information null</response>

        [HttpGet("SupplierDetail/{id}")]
        [ProducesResponseType(typeof(SupplierViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetSupplierDetail([FromRoute] int id, [FromQuery] int currentPage, [FromQuery] int itemPerPage)
        {
            try
            {
                SupplierViewModel supplierViewModel = new SupplierViewModel();
                supplierViewModel = await _supplierService.GetSupplierDetailAsync(id);
                supplierViewModel.Products = supplierViewModel.Products.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToList();

                if (supplierViewModel == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Supplier not found!",
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = supplierViewModel,
                    CurrentPage = currentPage, 
                    IteamPerpage = itemPerPage
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region Insert supplier
        /// <summary>
        /// Insert supplier
        /// </summary>
        /// 
        /// <param name="editSupplierViewModel">Object Type: EditSupplierViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            POST
        ///            "Name": BachHaiPhong
        ///            "Email": bachvippro@gmail.com
        ///            "Phone": 0928346573
        ///            "Status": 1
        ///     
        /// </remarks>
        /// <response code="201">Insert success</response>
        /// <response code="400">Insert fail</response>

        [HttpPost("InsertSupplier")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> InsertSupplier([FromBody] EditSupplierViewModel editSupplierViewModel)
        {

            try
            {
                bool result = false;

                if (editSupplierViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                    });
                }

                #region Validation
                SupplierValidator supplierValidator = new SupplierValidator();
                var resultValid = supplierValidator.Validate(editSupplierViewModel);
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


                result = await _supplierService.InsertSupplierAsync(editSupplierViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string> { "Insert failed!" }
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
                    Message = new List<string> { ex.Message }
                });
            }
        }
        #endregion

        #region Update supplier
        /// <summary>
        /// Update supplier
        /// </summary>
        /// 
        /// <param name="id"> Id of supplier </param>
        /// <param name="supplierViewModel">Object Type: EditSupplierViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            PUT
        ///            "id": 15
        ///            "Name": "Nexonn"
        ///            "Email": "thanhvippro910jqk@gmail.com"
        ///            "Phone": "0912873642" 
        ///            "Status": 0
        ///     
        /// </remarks>
        /// <response code="200">Update success</response>
        /// <response code="400">Update fail</response>

        [HttpPut("UpdateSupplier/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateSupplier([FromRoute] int id, [FromBody] EditSupplierViewModel supplierViewModel)
        {

            try
            {
                bool result = false;

                if (supplierViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                    });
                }

                #region Validation
                SupplierValidator supplierValidator = new SupplierValidator();
                var resultValid = supplierValidator.Validate(supplierViewModel);
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

                result = await _supplierService.UpdateSupplierAsync(id, supplierViewModel);
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

        #region Delete supplier
        /// <summary>
        /// Delete supplier
        /// </summary>
        /// 
        /// <param name="id"> Id of supplier </param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            DELETE
        ///            "id": 13
        ///     
        /// </remarks>
        /// <response code="200">Delete success</response>
        /// <response code="400">Delete fail</response>

        [HttpDelete("DeleteSupplier/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteSupplier([FromRoute] int id)
        {

            try
            {
                bool result = false;
                result = await _supplierService.DeleteSupplierAsync(id);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Delete failed!"
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
                    Message = ex.Message
                });
            }
        }
        #endregion

    }
}
