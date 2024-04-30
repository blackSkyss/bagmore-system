using BAL.Models;
using DAL.Infrastructure;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using BAL.Services.Interfaces;
using BAL.Services.Implements;
using BAL.Authorization;

namespace BagMoreSystem.Controllers.User
{

    [Route("api/user/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region GetProfileUser
        /// <summary>
        /// View profile of user
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>User profile</returns>
        /// <response code="400">If email User doesn't exist</response>
        /// <response code="200">View User profile</response>
        /// 
        [HttpGet("Get/{email}")]
        [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        /*[PermissionAuthorize("User")]*/
        public async Task<IActionResult> GetProfileUser([FromRoute] string email)
        {

            UserProfileViewModel result = null;
            try
            {
                result = await _userService.GetUserProfileAsync(email);
                if (result == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "user id doesn't exist"
                    });
                }
                return Ok(new
                {
                    data = result,
                    success = true,
                });
            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message
                });
            }
        }
        #endregion

        #region UpdateProfileUser
        /// <summary>
        /// update profile of user
        /// </summary>
        /// <param name="userViewModel">userViewModel</param>
        /// <param name="userEmail">userId</param>
        /// <returns>Update Profile of user</returns>
        /// <response code="400">If id User doesn't exist</response>
        /// <response code="200">Update User profile</response>

        [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPut("Update/{userEmail}")]
        /*[PermissionAuthorize("User")]*/
        public async Task<IActionResult> UpdateUSerProfile([FromRoute]string userEmail, [FromBody] UserProfileViewModel userViewModel)
        {

            //Coding session
            try
            {
               await _userService.UpdateUserProfileAsync(userEmail, userViewModel);
                return Ok(new
                {
                    success = true,
                    message = "Update Success"
                });
               
            }
            catch (Exception Ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = Ex.Message,
                }) ;
            }
        }
        #endregion
    }
}
