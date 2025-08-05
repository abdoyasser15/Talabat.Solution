using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Order_Aggregate;
using Talbat.Core.Repositories.Contract;
using Talbat.Core.Services.Contract;
using Product = Talbat.Core.Entities.Product;

namespace Talbat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configration;
        private readonly IBasketRepository basketRepository;
        private readonly IUntiOfWork unitOfWork;

        public PaymentService(IConfiguration configration , IBasketRepository basketRepository , IUntiOfWork unitOfWork)
        {
            this.configration = configration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreaterUpdatePaymentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configration["StripeSettings:Secretkey"];

            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket == null) return null;

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod  = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }

            if (basket?.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price!= product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create New Payment Intent
            {
                var Createoption = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum( Item=> Item.Price * 100 * Item.Quantity ) + (long)shippingPrice*100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" },
                };
                paymentIntent = await paymentIntentService.CreateAsync(Createoption); // Integrate with Stripe API to create a new payment intent

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update Existing Payment Intent
            {
                var UpdateOption = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(Item => Item.Price * 100 * Item.Quantity) + (long)shippingPrice * 100,
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, UpdateOption); // Integrate with Stripe API to update the existing payment intent
            }
            await basketRepository.UpdateBasketAsync(basket); // Update the basket in the repository
            return basket; // Return the updated basket
        }
    }
}
