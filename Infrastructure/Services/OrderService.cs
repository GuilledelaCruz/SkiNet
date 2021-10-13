using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress)
        {
            CustomerBasket basket = await _basketRepo.GetBasketAsync(basketId);

            List<OrderItem> items = new List<OrderItem>();
            foreach(BasketItem item in basket.Items)
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                ProductItemOrdered itemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                OrderItem orderItem = new OrderItem(itemOrdered, product.Price, item.Quantity);
                items.Add(orderItem);
            }

            DeliveryMethod dMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);
            decimal subtotal = items.Sum(item => item.Price * item.Quantity);
            Order order = new Order(buyerEmail, shippingAddress, dMethod, items, subtotal);

            _unitOfWork.Repository<Order>().Add(order);
            if ((await _unitOfWork.Complete()) <= 0) return null;

            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersWithItemsAndOrderingSpecification(id, buyerEmail));
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _unitOfWork.Repository<Order>().ListAsync(new OrdersWithItemsAndOrderingSpecification(buyerEmail));
        }
    }
}
