using Application.Configurations.Middleware;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ListViewModel<NotificationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<NotificationViewModel>>> GetNotifications([FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var notification = await _notificationService.GetNotifications(auth!.Id, pagination);
            return notification != null ? Ok(notification) : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(NotificationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NotificationViewModel>> GetNotification([FromRoute] Guid id)
        {
            var notification = await _notificationService.GetNotification(id);
            return notification != null ? Ok(notification) : NotFound();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(NotificationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NotificationViewModel>> UpdateNotification([FromRoute] Guid id, [FromBody] NotificationUpdateModel model)
        {
            try
            {
                var notification = await _notificationService.UpdateNotification(id, model);
                return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
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
        public async Task<ActionResult<NotificationViewModel>> DeleteNotification([FromRoute] Guid id)
        {
            try
            {
                var notification = await _notificationService.DeleteNotification(id);
                return notification ? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
