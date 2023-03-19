using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<UserViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<UserViewModel>>> GetUsers([FromQuery] UserFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var user = await _userService.GetUsers(filter, pagination);
            return user != null ? Ok(user) : BadRequest();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> GetUser([FromRoute] Guid id)
        {
            var user = await _userService.GetUser(id);
            return user != null ? Ok(user) : NotFound();
        }

        [Route("manager")]
        [HttpPost]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> CreateManager([FromBody] UserCreateModel model)
        {
            try
            {
                var user = await _userService.CreateManager(model);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateModel model)
        {
            var user = await _userService.UpdateUser(id, model);
            return user != null ? Ok(user) : BadRequest();
        }

    }
}
