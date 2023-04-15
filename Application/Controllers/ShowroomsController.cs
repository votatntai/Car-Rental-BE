using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/showrooms")]
    [ApiController]
    public class ShowroomsController : ControllerBase
    {
        private readonly IShowroomService _showroomService;
        public ShowroomsController(IShowroomService showroomService)
        {
            _showroomService = showroomService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<ShowroomViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<ShowroomViewModel>>> GetShowrooms([FromQuery] ShowroomFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var showrooms = await _showroomService.GetShowrooms(filter, pagination);
            return showrooms != null ? Ok(showrooms) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ShowroomViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShowroomViewModel>> GetShowroom([FromRoute] Guid id)
        {
            var showroom = await _showroomService.GetShowroom(id);
            return showroom != null ? Ok(showroom) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShowroomViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShowroomViewModel>> CreateShowroom([FromBody] ShowroomCreateModel model)
        {
            try
            {
                var showroom = await _showroomService.CreateShowroom(model);
                return CreatedAtAction(nameof(GetShowroom), new { id = showroom.Id }, showroom);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ShowroomViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShowroomViewModel>> UpdateShowroom([FromRoute] Guid id, [FromBody] ShowroomUpdateModel model)
        {
            try
            {
                var showroom = await _showroomService.UpdateShowroom(id, model);
                return CreatedAtAction(nameof(GetShowroom), new { id = showroom.Id }, showroom);
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
        public async Task<ActionResult<ShowroomViewModel>> DeleteShowroom([FromRoute] Guid id)
        {
            try
            {
                var showroom = await _showroomService.DeleteShowroom(id);
                return showroom ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
