using Data.Models.Get;
using Data.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<TransactionViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewModel<TransactionViewModel>>> GetTransactions([FromQuery] TransactionFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            var transaction = await _transactionService.GetTransactions(filter, pagination);
            return transaction != null ? Ok(transaction) : BadRequest();
        }
    }
}
