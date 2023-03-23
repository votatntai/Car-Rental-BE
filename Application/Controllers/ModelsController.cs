using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _modelService;
        public ModelsController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<CarModelViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarModelViewModel>>> GetModels([FromQuery] ModelFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var model = await _modelService.GetModels(filter, pagination);
            return model != null ? Ok(model) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CarModelViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarModelViewModel>> GetModel([FromRoute] Guid id)
        {
            var model = await _modelService.GetModel(id);
            return model != null ? Ok(model) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CarModelViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CarModelViewModel>> CreateModel([FromBody] ModelCreateModel model)
        {
            try
            {
                var carModel = await _modelService.CreateModel(model);
                return CreatedAtAction(nameof(GetModel), new { id = carModel.Id }, carModel);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CarModelViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CarModelViewModel>> UpdateModel([FromRoute] Guid id, [FromBody] ModelUpdateModel model)
        {
            try
            {
                var carModel = await _modelService.UpdateModel(id, model);
                return CreatedAtAction(nameof(GetModel), new { id = carModel.Id }, carModel);
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
        public async Task<ActionResult<CarModelViewModel>> DeleteModel([FromRoute] Guid id)
        {
            try
            {
                var model = await _modelService.DeleteModel(id);
                return model ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
