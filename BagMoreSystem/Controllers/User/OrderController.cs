using BagMoreSystem.Helpers;
using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BagMoreSystem.Controllers.User
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        [HttpGet("Orders")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                List<OrderViewModel> orderViewModels = await this._orderService.GetOrders(usename);
                return Ok(new
                {
                    Success = true,
                    Data = orderViewModels
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
