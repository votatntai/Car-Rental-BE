using Application.Configurations.Middleware;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(WalletViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WalletViewModel>> GetWallet()
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var wallet = await _walletService.GetWallet(auth!.Id);
            return wallet != null ? Ok(wallet) : NotFound();
        }
    }
}
