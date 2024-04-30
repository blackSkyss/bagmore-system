using BagMoreSystem.Helpers;
using BAL.Authorization;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BagMoreSystem.Controllers.User
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartProductService _cartProductService;
        public CartController(ICartProductService cartProductService)
        {
            _cartProductService = cartProductService;
        }
        [HttpPost("add-product")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> AddToCart([FromBody] CartProductViewModel cartProduct)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                await _cartProductService.AddToCart(cartProduct, usename);
                return Ok(new
                {
                    Success = true,
                    Message = "Add to cart successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [HttpDelete("remove-product")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] CartProductViewModel cartProduct)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                await _cartProductService.RemoveFromCart(cartProduct, usename);
                return Ok(new
                {
                    Success = true,
                    Message = "Remove from cart successsfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [HttpGet("products")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetCartProducts()
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                List<CartProductDetailsViewModel> cartProductDetails = await this._cartProductService.GetCartProducts(usename);
                return Ok(new
                {
                    Success = true,
                    Data = cartProductDetails
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [HttpPost("checkout")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutViewModel checkout)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                await this._cartProductService.Checkout(checkout, usename);
                return Ok(new
                {
                    Success = true,
                    Message = "Checkout successfully"
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
    }
}
