using Application.Configurations.Middleware;
using Data.Models.Get;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("users")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthViewModel>> AuthenticatedUser([FromBody][Required] AuthRequestModel model)
        {
            var user = await _authService.AuthenticatedUser(model);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("customers")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthViewModel>> AuthenticatedCustomer([FromBody][Required] AuthRequestModel model)
        {
            var customer = await _authService.AuthenticatedCustomer(model);
            if (customer is null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> GetUser()
        {
            var user = (AuthViewModel?)HttpContext.Items["User"];
            return user != null ? Ok(await _authService.GetUserById(user.Id)) : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("customers")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer()
        {
            var customer = (AuthViewModel?)HttpContext.Items["User"];
            return customer != null ? Ok(await _authService.GetCustomerById(customer.Id)) : BadRequest();
        }
    }
}
