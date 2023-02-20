using Data.Models.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
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

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> GetUser([FromRoute] Guid id)
        {
            var user = await _userService.GetUser(id);
            return user != null ? Ok(user) : BadRequest();
        }

        [Route("register/manager")]
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

    }
}
