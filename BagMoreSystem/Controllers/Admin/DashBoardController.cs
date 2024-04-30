using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {

        IDashBoardService _dashBoardService;

        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        #region GetTotalDashBoard
        /// <summary>
        /// Get all Dash board
        /// </summary>
        /// <returns>information dash board</returns>
        /// <response code="400">if information no exist</response>
        /// <response code="200">information dashboard</response>

        [ProducesResponseType(typeof(DashBoardInformationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("ViewTotal")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetTotalDashBoard([FromQuery] int? orderSoldYear, [FromQuery] int? profitYear)
        {
            //Coding session
            try
            {
                DashBoardInformationViewModel result = await _dashBoardService.GetDashBoardInformation();
                List<OrderSoldViewModel> orderSolds = await _dashBoardService.GetNumberOfOrdersSoldAsync(orderSoldYear);
                List<NumberOfProfitViewModel> numberOfProfits = await _dashBoardService.GetNumberOfProfitAsync(profitYear);
                result.NumberOfProfits = numberOfProfits;
                result.OrderSolds = orderSolds;
                if (result == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Get fail",
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

        #region GetNumberOfOrdersSold
        /// <summary>
        /// Get number of orders sold with month, year
        /// </summary>
        /// <returns>List Order Sold</returns>
        /// <response code="400">if information no exist</response>
        /// <response code="200">List Order Sold</response>

        [ProducesResponseType(typeof(OrderSoldViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("ViewNumberOrderSold/{year}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetNumberOfOrdersSoldAsync([FromRoute] int year)
        {
            //Coding session
            try
            {
                List<OrderSoldViewModel> result = await _dashBoardService.GetNumberOfOrdersSoldAsync(year);
                if (result.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Get fail",
                    });

                }
                return Ok(new
                {
                    result,
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

        #region GetNumberOfProfitAsync
        /// <summary>
        /// Get number of profit with month, year
        /// </summary>
        /// <returns>List number of profit</returns>
        /// <response code="400">if information no exist</response>
        /// <response code="200">List number of profit</response>

        [ProducesResponseType(typeof(NumberOfProfitViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("ViewNumberOfProfit/{year}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetNumberOfProfitAsync([FromRoute] int year)
        {
            //Coding session
            try
            {
                List<NumberOfProfitViewModel> result = await _dashBoardService.GetNumberOfProfitAsync(year);
                if (result.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Get fail",
                    });

                }
                return Ok(new
                {
                    result,
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
    }
}
