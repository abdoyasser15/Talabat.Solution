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
using Talbat.Core.Specifications.Orders_Specs;

namespace Talbat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUntiOfWork _untiOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo , IUntiOfWork untiOfWork , IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _untiOfWork = untiOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            //1. Get Basket Items from Repo

            var basket = await _basketRepo.GetBasketAsync(basketId);

            //2. Get Selected Items at basket from Product Repo

            var OrderItems = new List<OrderItems>();
            if (basket?.Items?.Count > 0)
            {
                var ProductRepository =  _untiOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var Product = await ProductRepository.GetByIdAsync(item.Id); 
                    var ProductItemOrdered = new ProductItemOrdered(item.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItems(ProductItemOrdered, Product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            //3. Calculate Subtotal

            var subTotal = OrderItems.Sum(x => x.Price * x.Quantity);

            //4. Get Delivery Method from DeleveryMethod Repo

            var deliveryMehod = await _untiOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var OrderSpecifications = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await _untiOfWork.Repository<Order>().GetWithSpecAsync(OrderSpecifications);

            if(existingOrder is not null)
            {
                _untiOfWork.Repository<Order>().Delete(existingOrder);

                await _paymentService.CreaterUpdatePaymentAsync(basket.Id);
            }

            //5. Create Order 

            var Order = new Order(buyerEmail, ShippingAddress, deliveryMehod, OrderItems, subTotal,basket.PaymentIntentId);

            await _untiOfWork.Repository<Order>().AddAsync(Order);

            //6. Save To Database [TODO]

            try
            {
                var result = await _untiOfWork.CompleteAsync();
                if (result <= 0)
                    return null;
                return Order;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Save Error: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var OrderRepository = _untiOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var Orders = await OrderRepository.GetAllSpecificationsAsync(spec);

            return Orders;
        }
        public Task<Order> GetOrderByIdForuserAsync(int OrderId, string buyerEmail)
        {
            var OrderRepository = _untiOfWork.Repository<Order>();
            var spec = new OrderSpecifications(OrderId, buyerEmail);
            var order = OrderRepository.GetWithSpecAsync(spec);
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethodRepository = _untiOfWork.Repository<DeliveryMethod>();
            return DeliveryMethodRepository.GetAllAsync();
        }
    }
}
