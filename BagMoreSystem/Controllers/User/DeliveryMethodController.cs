using BAL.Authorization;
using BAL.Models;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagMoreSystem.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly IDeliveryMethodService _deliveryMethodService;

        public DeliveryMethodController(IDeliveryMethodService deliveryMethodService)
        {
            _deliveryMethodService = deliveryMethodService;
        }

        #region get delivery method for user
        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(List<DeliveryMethodUserViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> GetDeliveryMethods()
        {

            try
            {
                List<DeliveryMethodUserViewModel> deliveryMethodViewModels = new List<DeliveryMethodUserViewModel>();
                deliveryMethodViewModels = await _deliveryMethodService.GetDeliveryUser();
                if (deliveryMethodViewModels.Count == 0)
                {
                    return NotFound(new
                    {
                        Success = false,
                        ErrorMessage = "List of delivery method is empty!"
                    });
                }
                return Ok(new
                {
                    Success = true,
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
    }
}
