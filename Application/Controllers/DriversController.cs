using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
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

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<DriverViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<DriverViewModel>>> GetDrivers([FromQuery] DriverFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var driver = await _driverService.GetDrivers(filter, pagination);
            return driver != null ? Ok(driver) : BadRequest();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(DriverViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> GetDriver([FromRoute] Guid id)
        {
            var driver = await _driverService.GetDriver(id);
            return driver != null ? Ok(driver) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(DriverViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> CreateDriver([FromBody] DriverCreateModel model)
        {
            try
            {
                var driver = await _driverService.CreateDriver(model);
                return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(DriverViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> UpdateDriver([FromRoute] Guid id, [FromBody] DriverUpdateModel model)
        {
            try
            {
                var driver = await _driverService.UpdateDriver(id, model);
                return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
