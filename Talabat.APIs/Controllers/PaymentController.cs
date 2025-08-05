using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talbat.Core.Entities;
using Talbat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]// Post: api/payment/basketId
        public async Task<ActionResult<CustomerBasket>> CreateOrUdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreaterUpdatePaymentAsync(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400,"An Error With Your Basket"));
            return Ok(basket);
        }
    }
}
