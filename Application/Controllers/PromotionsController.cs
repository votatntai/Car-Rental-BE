using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<PromotionViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<PromotionViewModel>>> GetPromotions([FromQuery] PromotionFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var promotions = await _promotionService.GetPromotions(filter, pagination);
            return promotions != null ? Ok(promotions) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PromotionViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PromotionViewModel>> GetPromotion([FromRoute] Guid id)
        {
            var promotion = await _promotionService.GetPromotion(id);
            return promotion != null ? Ok(promotion) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(PromotionViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PromotionViewModel>> CreatePromotion([FromBody] PromotionCreateModel model)
        {
            try
            {
                var promotion = await _promotionService.CreatePromotion(model);
                return CreatedAtAction(nameof(GetPromotion), new { id = promotion.Id }, promotion);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(PromotionViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PromotionViewModel>> UpdatePromotion([FromRoute] Guid id, [FromBody] PromotionUpdateModel model)
        {
            try
            {
                var promotion = await _promotionService.UpdatePromotion(id, model);
                return CreatedAtAction(nameof(GetPromotion), new { id = promotion.Id }, promotion);
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
        public async Task<ActionResult<PromotionViewModel>> DeletePromotion([FromRoute] Guid id)
        {
            try
            {
                var promotion = await _promotionService.DeletePromotion(id);
                return promotion ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
