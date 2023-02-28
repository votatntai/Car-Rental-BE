using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
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

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<CarOwnerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarOwnerViewModel>>> GetCarOwners([FromQuery] CarOwnerFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var carOwner = await _carOwnerService.GetCarOwners(filter, pagination);
            return carOwner != null ? Ok(carOwner) : BadRequest();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CarOwnerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarOwnerViewModel>> GetCarOwner([FromRoute] Guid id)
        {
            var carOwner = await _carOwnerService.GetCarOwner(id);
            return carOwner != null ? Ok(carOwner) : NotFound();
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

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CarOwnerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarOwnerViewModel>> UpdateCarOwner([FromRoute] Guid id, [FromBody] CarOwnerUpdateModel model)
        {
            try
            {
                var carOwner = await _carOwnerService.UpdateCarOwner(id, model);
                return CreatedAtAction(nameof(GetCarOwner), new { id = carOwner.Id }, carOwner);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
