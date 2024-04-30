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
    public class DeliveryMethodController : ControllerBase
    {
        private readonly IDeliveryMethodService _deliveryMethodService;

        public DeliveryMethodController(IDeliveryMethodService deliveryMethodService)
        {
            _deliveryMethodService = deliveryMethodService;
        }

        #region Get delivery methods
        /// <summary>
        /// Get list delivery method and include sort, search, paging
        /// </summary>
        /// 
        /// <param name="keysearch"> Search by keyword </param>
        /// <param name="keysortname"> Sort by NAMEASC || NAMEDESC </param>
        /// <param name="keysortprice"> Sort by PRICEASC || PRICEDESC </param>
        /// <param name="keysortdescription"> Sort by DESCRIPTIONASC || DESCRIPTIONDESC </param>
        /// <param name="keysortstatus"> Sort by STATUSASC || STATUSDESC </param>
        /// <param name="PAGE_SIZE"> Number of item in one page </param>
        /// <param name="PAGE_NUMBER"> Number page want to go </param>
        /// <returns> Return list of delivery method</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "keysearch": "hoho"
        ///            "keysortname": "NAMEDESC"
        ///            "keysortprice" "PRICEDESC"
        ///            "keysortdescription": "DESCRIPTIONASC"
        ///            "keysortstatus": "STATUSASC"
        ///            "PAGE_SIZE": 5
        ///            "PAGE_NUMBER": 2
        ///     
        /// </remarks>
        /// <response code="200"> Trả về list of delivery method</response>
        /// <response code="404"> Nếu list null</response>

        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(List<DeliveryMethodViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetDeliveryMethods([FromQuery] SearchingDeliveryMethodViewModel? searchingViewModel)
        {

            try
            {
                int totalPage;
                int pageSize;
                int pageNumber;
                List<DeliveryMethodViewModel> deliveryMethodViewModels = new List<DeliveryMethodViewModel>();
                Tuple<List<DeliveryMethodViewModel>, int, int?, int?> result = await _deliveryMethodService.GetDeliveryMethodsAsync(searchingViewModel?.keysearch,
                                                                            searchingViewModel?.keysortname,
                                                                            searchingViewModel?.keysortprice,
                                                                            searchingViewModel?.keysortstatus,
                                                                            searchingViewModel?.PAGE_SIZE,
                                                                            searchingViewModel?.PAGE_NUMBER);
                deliveryMethodViewModels = result.Item1;
                if (deliveryMethodViewModels.Count == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List of delivery method is empty!"
                    });
                }

                totalPage = result.Item2;
                pageSize = (int)result.Item3!;
                pageNumber = (int)result.Item4!;

                return Ok(new
                {
                    Success = true,
                    KeySearch = searchingViewModel?.keysearch,
                    KeySortName = searchingViewModel?.keysortname,
                    KeySortPrice = searchingViewModel?.keysortprice,
                    KeySortStatus = searchingViewModel?.keysortstatus,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    TotalPage = totalPage,
                    Data = deliveryMethodViewModels,
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

        #region Get detail delivery method
        /// <summary>
        /// Get detail information of delivery
        /// </summary>
        /// 
        /// <param name="id"> id of delivery </param>
        /// <returns> Return detail information of delivery</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            GET
        ///            "id": 5
        ///     
        /// </remarks>
        /// <response code="200"> Trả về detail information of delivery</response>
        /// <response code="404"> Nếu detail information null</response>

        [HttpGet("DeliveryDetail/{id}")]
        [ProducesResponseType(typeof(DeliveryMethodViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetDeliveryDetail([FromRoute] int id)
        {
            try
            {
                DeliveryMethodViewModel deliveryMethodView = new DeliveryMethodViewModel();
                deliveryMethodView = await _deliveryMethodService.GetDeliveryMethodDetailAsync(id);

                if (deliveryMethodView == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Delivery method not found!",
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = deliveryMethodView,
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

        #region Insert delivery method
        /// <summary>
        /// Insert delivery method
        /// </summary>
        /// 
        /// <param name="deliveryMethodViewModel">Object Type: EditDeliveryMethodViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            POST
        ///            "Name": Chocolate
        ///            "Price": 250
        ///            "Description": Oh no very good
        ///            "Status": 1
        ///     
        /// </remarks>
        /// <response code="201">Insert success</response>
        /// <response code="400">Insert fail</response>

        [HttpPost("InsertDeliveryMethod")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> InsertDeliveryMethod([FromBody] EditDeliveryMethodViewModel deliveryMethodViewModel)
        {

            try
            {
                bool result = false;

                if (deliveryMethodViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                    });
                }

                #region Validation
                DeliveryMethodValidator deliveryValidator = new DeliveryMethodValidator();
                var resultValid = deliveryValidator.Validate(deliveryMethodViewModel);
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


                result = await _deliveryMethodService.InsertDeliveryMethodAsync(deliveryMethodViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string>() { "Insert failed!" }
                    });
                }

                return Ok(new
                {
                    Success = result,
                    Message = new List<string>() { "Insert successed" },
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = new List<string>() { ex.Message }
                });
            }
        }
        #endregion

        #region Update delivery method
        /// <summary>
        /// Update delivery method
        /// </summary>
        /// 
        /// <param name="id"> Id of delivery </param>
        /// <param name="deliveryMethodViewModel">Object Type: EditDeliveryMethodViewModel</param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            PUT
        ///            "id": 3
        ///            "Name": "Toan haha"
        ///            "Price": 201
        ///            "Description": "You know the game" 
        ///            "Status": 0
        ///     
        /// </remarks>
        /// <response code="200">Update success</response>
        /// <response code="400">Update fail</response>

        [HttpPut("UpdateDeliveryMethod/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateDeliveryMethod([FromRoute] int id, [FromBody] EditDeliveryMethodViewModel deliveryMethodViewModel)
        {

            try
            {
                bool result = false;

                if (deliveryMethodViewModel == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                    });
                }

                #region Validation
                DeliveryMethodValidator deliveryValidator = new DeliveryMethodValidator();
                var resultValid = deliveryValidator.Validate(deliveryMethodViewModel);
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

                result = await _deliveryMethodService.UpdateDeliveryMethodAsync(id, deliveryMethodViewModel);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        Success = result,
                        Message = new List<string>() { "Update failed!" }
                    });
                }

                return Ok(new
                {
                    Success = result,
                    Message = new List<string>() { "Update successed" },
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = new List<string>(){ex.Message}
                });
            }
        }
        #endregion

        #region Delete delivery method
        /// <summary>
        /// Delete delivery method
        /// </summary>
        /// 
        /// <param name="id"> Id of delivery method </param>
        /// <returns> Success or fail base on status code</returns>
        /// <remarks>
        ///     Sample request
        /// 
        ///            DELETE
        ///            "id": 2
        ///     
        /// </remarks>
        /// <response code="200">Delete success</response>
        /// <response code="400">Delete fail</response>

        [HttpDelete("DeleteDeliveryMethod/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteDeliveryMethod([FromRoute] int id)
        {

            try
            {
                bool result = false;
                result = await _deliveryMethodService.DeleteDeliveryMethodAsync(id);
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
