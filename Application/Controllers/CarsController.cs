﻿using Application.Configurations.Middleware;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Utility.Enums;

namespace Application.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        private readonly ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<CarViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarViewModel>>> GetCars([FromQuery] CarFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var car = await _carService.GetCars(filter, pagination);
            return car != null ? Ok(car) : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("for-car-owners")]
        [ProducesResponseType(typeof(ListViewModel<CarViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarViewModel>>> GetCarsForCarOwner([FromQuery] CarStatus status, [FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var car = await _carService.GetCarsByCarOwnerId(auth!.Id, status, pagination);
            return car != null ? Ok(car) : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("is-not-tracking")]
        [ProducesResponseType(typeof(ListViewModel<CarViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<CarViewModel>>> GetCarsIsNotTracking([FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var car = await _carService.GetCarsIsNotTracking(auth!.Id, pagination);
            return car != null ? Ok(car) : BadRequest();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarViewModel>> GetCar([FromRoute] Guid id)
        {
            var car = await _carService.GetCar(id);
            return car != null ? Ok(car) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarViewModel>> CreateCar([FromBody] CarCreateModel model)
        {
            try
            {
                var car = await _carService.CreateCar(model);
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarViewModel>> UpdateCar([FromRoute] Guid id, [FromBody] CarUpdateModel model)
        {
            try
            {
                var car = await _carService.UpdateCar(id, model);
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("tracking/{id}")]
        [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarViewModel>> TrackingACar([FromRoute] Guid id)
        {
            try
            {
                var car = await _carService.TrackingACar(id);
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("cancel-tracking/{id}")]
        [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarViewModel>> CancelTrackingACar([FromRoute] Guid id)
        {
            try
            {
                var car = await _carService.CancelTrackingACar(id);
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
