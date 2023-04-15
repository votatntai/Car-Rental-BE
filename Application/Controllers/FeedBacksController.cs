using Application.Configurations.Middleware;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/feed-backs")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        public FeedBacksController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<FeedBackViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<FeedBackViewModel>>> GetFeedBacks([FromQuery] FeedBackFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var feedBacks = await _feedBackService.GetFeedBacks(filter, pagination);
            return feedBacks != null ? Ok(feedBacks) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(FeedBackViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FeedBackViewModel>> GetFeedBack([FromRoute] Guid id)
        {
            var feedBack = await _feedBackService.GetFeedBack(id);
            return feedBack != null ? Ok(feedBack) : NotFound();
        }

        [HttpGet]
        [Route("drivers/{id}")]
        [ProducesResponseType(typeof(ListViewModel<FeedBackViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<FeedBackViewModel>>> GetFeedBacksForDriver([FromRoute] Guid id, [FromQuery] PaginationRequestModel pagination)
        {
            var feedBacks = await _feedBackService.GetFeedBacksForDriver(id, pagination);
            return feedBacks != null ? Ok(feedBacks) : NotFound();
        }

        [HttpGet]
        [Route("cars/{id}")]
        [ProducesResponseType(typeof(ListViewModel<FeedBackViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<FeedBackViewModel>>> GetFeedBacksForCar([FromRoute] Guid id, [FromQuery] PaginationRequestModel pagination)
        {
            var feedBacks = await _feedBackService.GetFeedBacksForCar(id, pagination);
            return feedBacks != null ? Ok(feedBacks) : NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("car")]
        [ProducesResponseType(typeof(FeedBackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedBackViewModel>> CreateFeedBackForCar([FromBody] FeedBackCreateModel model)
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                var feedBack = await _feedBackService.CreateFeedBackForCar(auth!.Id, model);
                return CreatedAtAction(nameof(GetFeedBack), new { id = feedBack.Id }, feedBack);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("driver")]
        [ProducesResponseType(typeof(FeedBackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedBackViewModel>> CreateFeedBackForDriver([FromBody] FeedBackCreateModel model)
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                var feedBack = await _feedBackService.CreateFeedBackForDriver(auth!.Id, model);
                return CreatedAtAction(nameof(GetFeedBack), new { id = feedBack.Id }, feedBack);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(FeedBackViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedBackViewModel>> UpdateFeedBack([FromRoute] Guid id, [FromBody] FeedBackUpdateModel model)
        {
            try
            {
                var feedBack = await _feedBackService.UpdateFeedBack(id, model);
                return CreatedAtAction(nameof(GetFeedBack), new { id = feedBack.Id }, feedBack);
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
        public async Task<ActionResult<FeedBackViewModel>> DeleteFeedBack([FromRoute] Guid id)
        {
            try
            {
                var feedBack = await _feedBackService.DeleteFeedBack(id);
                return feedBack ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
