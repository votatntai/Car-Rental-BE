﻿using Application.Configurations.Middleware;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ListViewModel<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<OrderViewModel>>> GetOrders([FromQuery] OrderFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var order = await _orderService.GetOrders(auth!.Id, filter, pagination);
            return order != null ? Ok(order) : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderViewModel>> GetOrder([FromRoute] Guid id)
        {
            var order = await _orderService.GetOrder(id);
            return order != null ? Ok(order) : NotFound();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderViewModel>> CreateOrder([FromBody] OrderCreateModel model)
        {
            try
            {
                var auth = (AuthViewModel?)HttpContext.Items["User"];
                var order = await _orderService.CreateOrder(auth!.Id, model);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderViewModel>> UpdateOrder([FromRoute] Guid id, [FromBody] OrderUpdateModel model)
        {
            try
            {
                var order = await _orderService.UpdateOrder(id, model);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}