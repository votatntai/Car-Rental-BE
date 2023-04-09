using Application.Configurations.Middleware;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
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
            return carRegistration != null ? Ok(carRegistration) : NotFound();
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
        [Authorize]
        [ProducesResponseType(typeof(CarRegistrationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarRegistrationViewModel>>
            CreateCarRegistration(ICollection<IFormFile> images, ICollection<IFormFile> licenses,[FromQuery] CarRegistrationCreateModel model)
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                var carRegistration = await _carRegistrationService.CreateCarRegistration(auth!.Id, images, licenses, model);
                return CreatedAtAction(nameof(GetCarRegistration), new { id = carRegistration.Id }, carRegistration);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CarRegistrationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarRegistrationViewModel>> UpdateCar([FromRoute] Guid id, [FromBody] CarRegistrationUpdateModel model)
        {
            try
            {
                var car = await _carRegistrationService.UpdateCar(id, model);
                return CreatedAtAction(nameof(GetCarRegistration), new { id = car.Id }, car);
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
