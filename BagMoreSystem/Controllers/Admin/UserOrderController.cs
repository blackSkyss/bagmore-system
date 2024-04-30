using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Validator;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using BAL.Authorization;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserOrderController : ControllerBase
    {
        IOrderService _orderService;
        public UserOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region GetUser'sOrderDetail
        /// <summary>
        /// GetUser'sOrderDetail
        /// </summary>
        /// <returns>user's order detail</returns>
        /// <response code="400">user id does not exsits</response>
        /// <response code="200">user's order detail</response>

        [ProducesResponseType(typeof(OrderUserDetailViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("Get/{orderId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetUserOrderDetail(int orderId)
        {
            //Coding session
            try
            {
                OrderUserDetailViewModel orderUserDetailViewModel = await _orderService.getOrderUserDetailAsync(orderId); 
                if (orderUserDetailViewModel == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Can't load brand",
                    });
                }
                return Ok(new
                {
                    Data = orderUserDetailViewModel,
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

        [HttpGet("UserOrders")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetUserOrders([FromQuery] SearchingUserOrderViewModel? searchingUserOrder)
        {
            try
            {
                List<UserOrderViewModel> userOrders = await this._orderService.GetUserOrders(searchingUserOrder);
                return Ok(new
                {
                    Success = true,
                    Data = userOrders,
                    Parameter = searchingUserOrder
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        [HttpPut("Change-UserOrderStatus/{orderId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int orderId, [FromBody] int deliveryStatus)
        {
            try
            {
                await this._orderService.UpdateOrderStatus(orderId, deliveryStatus);
                return Ok(new
                {
                    Success = true,
                    Data = "Update order delivery status successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }
    }
}
