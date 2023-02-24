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

        [Route("drivers")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthViewModel>> AuthenticatedDriver([FromBody][Required] AuthRequestModel model)
        {
            var driver = await _authService.AuthenticatedDriver(model);
            if (driver is null)
            {
                return NotFound();
            }
            return Ok(driver);
        }

        [Route("car-owners")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthViewModel>> AuthenticatedCarOwner([FromBody][Required] AuthRequestModel model)
        {
            var carOwner = await _authService.AuthenticatedCarOwner(model);
            if (carOwner is null)
            {
                return NotFound();
            }
            return Ok(carOwner);
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserViewModel>> GetUser()
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                if (auth != null)
                {
                    var user = await _authService.GetUserById(auth.Id);
                    return user != null ? Ok(user) : NotFound();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("customers")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer()
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                if(auth != null)
                {
                    var customer = await _authService.GetCustomerById(auth.Id);
                    return customer != null ? Ok(customer) : NotFound();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("drivers")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverViewModel>> GetDriver()
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                if (auth != null)
                {
                    var driver = await _authService.GetDriverById(auth.Id);
                    return driver != null ? Ok(driver) : NotFound();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("car-owners")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarOwnerViewModel>> GetCarOwner()
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                if (auth != null)
                {
                    var carOwner = await _authService.GetCarOwnerById(auth.Id);
                    return carOwner != null ? Ok(carOwner) : NotFound();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
