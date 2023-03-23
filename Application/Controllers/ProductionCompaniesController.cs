using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/production-companies")]
    [ApiController]
    public class ProductionCompaniesController : ControllerBase
    {
        private readonly IProductionCompanyService _productionCompanyService;
        public ProductionCompaniesController(IProductionCompanyService productionCompanyService)
        {
            _productionCompanyService = productionCompanyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<ProductionCompanyViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<ProductionCompanyViewModel>>> GetProductionCompanies([FromQuery] ProductionCompanyFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var productionCompany = await _productionCompanyService.GetProductionCompanies(filter, pagination);
            return productionCompany != null ? Ok(productionCompany) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductionCompanyViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductionCompanyViewModel>> GetProductionCompany([FromRoute] Guid id)
        {
            var productionCompany = await _productionCompanyService.GetProductionCompany(id);
            return productionCompany != null ? Ok(productionCompany) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductionCompanyViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductionCompanyViewModel>> CreateProductionCompany([FromBody] ProductionCompanyCreateModel model)
        {
            try
            {
                var productionCompany = await _productionCompanyService.CreateProductionCompany(model);
                return CreatedAtAction(nameof(GetProductionCompany), new { id = productionCompany.Id }, productionCompany);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductionCompanyViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductionCompanyViewModel>> UpdateProductionCompany([FromRoute] Guid id, [FromBody] ProductionCompanyUpdateModel model)
        {
            try
            {
                var productionCompany = await _productionCompanyService.UpdateProductionCompany(id, model);
                return CreatedAtAction(nameof(GetProductionCompany), new { id = productionCompany.Id }, productionCompany);
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
        public async Task<ActionResult<ProductionCompanyViewModel>> DeleteProductionCompany([FromRoute] Guid id)
        {
            try
            {
                var productionCompany= await _productionCompanyService.DeleteProductionCompany(id);
                return productionCompany? NoContent() : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

    }
}
