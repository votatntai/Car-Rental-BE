using Data.Models.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/car-owners")]
    [ApiController]
    public class CarOwnersController : ControllerBase
    {

        private readonly ICarOwnerService _carOwnerService;
        public CarOwnersController(ICarOwnerService carOwnerService)
        {
            _carOwnerService = carOwnerService;
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CarOwnerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarOwnerViewModel>> GetCarOwner([FromRoute] Guid id)
        {
            var carOwner = await _carOwnerService.GetCarOwner(id);
            return carOwner != null ? Ok(carOwner) : BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CarOwnerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarOwnerViewModel>> CreateCarOwner([FromBody] CarOwnerCreateModel model)
        {
            try
            {
                var carOwner = await _carOwnerService.CreateCarOwner(model);
                return CreatedAtAction(nameof(GetCarOwner), new { id = carOwner.Id }, carOwner);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
