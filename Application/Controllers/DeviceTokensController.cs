using Application.Configurations.Middleware;
using Data.Models.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/device-tokens")]
    [ApiController]
    public class DeviceTokensController : ControllerBase
    {
        private readonly IDeviceTokenService _deviceTokenService;
        public DeviceTokensController(IDeviceTokenService deviceTokenService)
        {
            _deviceTokenService = deviceTokenService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CreateDeviceToken([FromBody] DeviceTokenCreateModel model)
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                var carModel = await _deviceTokenService.CreateDeviceToken(auth!.Id, model);
                return Ok(carModel );
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
