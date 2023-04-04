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
        [ProducesResponseType(typeof(ListViewModel<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<OrderViewModel>>> GetOrders([FromQuery] OrderFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var order = await _orderService.GetOrders(auth != null ? auth.Id : null!, filter, pagination);
            return order != null ? Ok(order) : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("for-car-owners")]
        [ProducesResponseType(typeof(ListViewModel<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<OrderViewModel>>> GetOrdersForCarOwner([FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var order = await _orderService.GetOrdersForCarOwner(auth!.Id, pagination);
            return order != null ? Ok(order) : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("for-drivers")]
        [ProducesResponseType(typeof(ListViewModel<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<OrderViewModel>>> GetOrdersForDriver([FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var order = await _orderService.GetOrdersForDriver(auth!.Id, pagination);
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
        [Route("status/{id}")]
        [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderViewModel>> UpdateOrderStatus([FromRoute] Guid id, string? description, OrderStatus status)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatus(id, description, status);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
