using BagMoreSystem.Helpers;
using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BagMoreSystem.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        IShippingAddressService _shippingAddressService;
        public ShippingAddressController(IShippingAddressService shippingAddressService)
        {
            _shippingAddressService = shippingAddressService;
        }

        #region GetShippingAddress
        /// <summary>
        /// User view shipping address 
        /// </summary>
        /// <param name="idUser">idUser</param>
        /// <returns>Get Shipping address of user</returns>
        /// <response code="400">If Id User doesn't exist</response>
        /// <response code="200">View shipping adddress of user </response>
        
        [HttpGet("Get")]
        [ProducesResponseType(typeof(ShippingAddressViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetShippingAddresses()
        {
            //Coding session

            List<ShippingAddressViewModel> result = null;
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                result = await _shippingAddressService.GetShippingAddressAsync(usename);
                if(result == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "No have shipping address"
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

        #region AddShippingAddress
        /// <summary>
        /// Add new shipping address
        /// </summary>
        /// <param name="userViewModel">Id USer </param>
        /// <returns>Add shipping address of user</returns>
        /// <response code="400">If Id User doesn't exist</response>
        /// <response code="200">add shipping address success</response>
        
        [ProducesResponseType(typeof(ShippingAddressViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost("Create")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> AddShippingAddress([FromBody] ShippingAddressViewModel shippingAddressViewModel)
        {
            //Coding session
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                await _shippingAddressService.AddShippingAddressesAsync(usename, shippingAddressViewModel);
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

    }
}
