﻿using Data.Models.Create;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;

namespace Application.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer([FromRoute] Guid id)
        {
            var customer = await _customerService.GetCustomer(id);
            return customer != null ? Ok(customer) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewModel>> CreateCustomer([FromBody] CustomerCreateModel model)
        {
            try
            {
                var customer = await _customerService.CreateCustomer(model);
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewModel>> UpdateCustomer([FromRoute] Guid id, [FromBody] CustomerUpdateModel model)
        {
            try
            {
                var customer = await _customerService.UpdateCustomer(id, model);
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
