using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/car-registrations")]
    [ApiController]
    public class CarRegistrationsController : ControllerBase
    {

        private readonly ICarRegistrationService _carRegistrationService;
        public CarRegistrationsController(ICarRegistrationService carRegistrationService)
        {
            _carRegistrationService = carRegistrationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<CarRegistrationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarRegistrationViewModel>>> GetCarRegistrations([FromQuery] CarRegistrationFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var carRegistration = await _carRegistrationService.GetCarRegistrations(filter, pagination);
            return carRegistration != null ? Ok(carRegistration) : BadRequest();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CarRegistrationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarRegistrationViewModel>> GetCarRegistration([FromRoute] Guid id)
        {
            var carRegistration = await _carRegistrationService.GetCarRegistration(id);
            return carRegistration != null ? Ok(carRegistration) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CarRegistrationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarRegistrationViewModel>> CreateCarRegistration([FromBody] CarRegistrationCreateModel model)
        {
            try
            {
                var carRegistration = await _carRegistrationService.CreateCarRegistration(model);
                return CreatedAtAction(nameof(GetCarRegistration), new { id = carRegistration.Id }, carRegistration);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarRegistrationViewModel>> DeleteCarRegistration([FromRoute] Guid id)
        {
            try
            {
                var carRegistration = await _carRegistrationService.DeleteCarRegistration(id);
                return carRegistration ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
