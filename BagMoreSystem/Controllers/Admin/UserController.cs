using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("Users")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetUsers([FromQuery]SearchingUserViewModel? searchingUserViewModel)
        {
            try
            {
                List<UserViewModel> users = await this._userService.GetUsers(searchingUserViewModel);
                return Ok(new
                {
                    Success = true,
                    Data = users
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpDelete("Ban/{userId}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> BanUser([FromRoute]int userId)
        {
            try
            {
                await this._userService.BanUser(userId);
                return Ok(new
                {
                    Successful = true,
                    Message = "Ban user successfully"
                });
            } catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }
    }
}
