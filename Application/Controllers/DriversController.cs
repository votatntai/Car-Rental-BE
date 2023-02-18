using Data.Models.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {

        private readonly IDriverService _driverService;
        public DriversController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(DriverViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> GetDriver([FromRoute] Guid id)
        {
            var driver = await _driverService.GetDriver(id);
            return driver != null ? Ok(driver) : BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(typeof(DriverViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> CreateDriver([FromBody] DriverCreateModel model)
        {
            var driver = await _driverService.CreateDriver(model);
            return driver != null ? Ok(driver) : BadRequest();
        }
    }
}
