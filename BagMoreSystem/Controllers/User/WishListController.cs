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
    public class WishListController : ControllerBase
    {
        IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            this._wishListService = wishListService;
        }

        #region GetWishList
        /// <summary>
        /// Get wish list
        /// </summary>
        /// 
        /// <returns>View wish list</returns>
        /// <response code="400">If wish list empty</response>
        /// <response code="200">View wish list</response>

        [ProducesResponseType(typeof(WishListViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("Get")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetWishList()
        {
            //Coding session
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(HttpContext);
                string usename = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                List<WishListViewModel> wishListViewModels = await _wishListService.GetWishListByUserIdAsync(usename);
                return Ok(new
                {
                    success = true,
                    wishListViewModels,
                    
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

        #region CreateWishList
        /// <summary>
        /// Add wish list
        /// </summary>
        /// 
        /// <returns>Add success</returns>
        /// <response code="400">If user id or product id does not exists</response>
        /// <response code="200">Add successfully</response>

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost("Create")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> AddWishList([FromBody] WishListCreateNewViewModel wishListCreateNew)
        {
            //Coding session
            try
            {
                bool result = await _wishListService.AddWishListAsync(wishListCreateNew);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Can't add wish list"
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Add success"
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

        #region DeleteWishList
        /// <summary>
        /// Delete Wishlist
        /// </summary>
        /// 
        /// <returns>Delete Successfully</returns>
        /// <response code="400">Id does not exist</response>
        /// <response code="200">Delete successfully</response>

        [ProducesResponseType(typeof(WishListViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpDelete("Delete/{wishListId}")]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> DeleteWishList([FromRoute] int wishListId)
        {
            //Coding session
            try
            {
                bool result = await _wishListService.DeleteWishListAsync(wishListId);
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Delete fail"
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Delete successfully"
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
