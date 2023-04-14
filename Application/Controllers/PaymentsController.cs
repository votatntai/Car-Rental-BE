using Application.Configurations.Middleware;
using Data.Models.Get;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Shared.ExternalServices.VnPay;
using System.Globalization;
using Utility.Constants;
using Utility.Helpers.Models;
using Utility.Settings;

namespace Application.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        private readonly AppSetting _appSettings;
        public PaymentsController(IVNPayService vnPayService, IOptions<AppSetting> appSettings)
        {
            _vnPayService = vnPayService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("request")]
        [Authorize]
        public async Task<ActionResult<string>> CreatePayRequest(VnPayInputModel input)
        {
            var auth = (AuthViewModel?)HttpContext.Items["User"];
            var now = DateTime.UtcNow.AddHours(7);
            var clientIp = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
            var requestModel = new VnPayRequestModel
            {
                TxnRef = Guid.NewGuid(),
                Command = VnPayConstant.Command,
                Locale = VnPayConstant.Locale,
                Version = VnPayConstant.Version,
                CurrencyCode = VnPayConstant.CurrencyCode,
                Amount = input.Amount,
                CreateDate = now,
                ExpireDate = now.AddMinutes(15),
                OrderInfo = $"Nạp tiền: {input.Amount} VNĐ",
                IpAddress = clientIp,
                ReturnUrl = _appSettings.ReturnUrl,
                TmnCode = _appSettings.MerchantId
            };

            var result = await _vnPayService.AddRequest(auth!.Id, requestModel);
            return result ? Ok(VnPayHelper.CreateRequestUrl(requestModel, _appSettings.VNPayUrl, _appSettings.MerchantPassword)) : BadRequest();
        }

        [HttpGet("ipn")]
        public async Task<IActionResult> VnPayIpnEntry([FromQuery] Dictionary<string, string> queryParams)
        {
            if (!VnPayHelper.ValidateSignature(_appSettings.MerchantPassword, queryParams))
                return BadRequest("Invalid Signature.");

            var model = VnPayHelper.ParseToResponseModel(queryParams);
            var result = await _vnPayService.AddResponse(model);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("result")]
        public ActionResult<PaymentViewModel> PaymentResult([FromQuery] Dictionary<string, string> queryParams)
        {
            if (!VnPayHelper.ValidateSignature(_appSettings.MerchantPassword, queryParams))
                return BadRequest("Invalid Signature.");
            var model = VnPayHelper.ParseToResponseModel(queryParams);

            DateTime? payDate = model.PayDate is null ? null : DateTime.ParseExact(model.PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            return Ok(new PaymentViewModel
            {
                TransactionStatus = model.TransactionStatus,
                Response = model.ResponseCode,
                OrderInfo = model.OrderInfo,
                BankCode = model.BankCode,
                Amount = model.Amount,
                CardType = model.CardType,
                PayDate = payDate,
                TransactionNo = model.TransactionNo
            });
        }

    }
}
