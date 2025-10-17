using System;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Payment;
using SneakersShop.Application.UseCases.Commands.Orders;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Extensions;

namespace SneakersShop.Implementation.UseCases.Commands.Orders;

public class EfCreateOrderCommand(
    SneakersShopDbContext context,
    IApplicationUser? user,
    IPaymentProcessor paymentProcessor
) : EfUseCase(context, user), ICreateOrderCommand
{
    private IPaymentProcessor _paymentProcessor = paymentProcessor;
    public int Id => 21;

    public string Name => "Create Order";

    public string Description => "";

    public void Execute(CreateOrderDto request)
    {
        if (request.Items == null || !request.Items.Any())
            throw new Exception("Order must contain at least one item.");

        decimal total = 0;

        var productSizeMap = new List<(ProductSize Size, CreateOrderItemDto Dto, decimal Price)>();

        foreach (var item in request.Items)
        {
            var productSize = Context
                .ProductSizes.Include(ps => ps.ProductColor)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefault(ps =>
                    ps.ProductColorId == item.ProductColorId && ps.SizeId == item.SizeId
                );

            if (productSize == null)
                throw new Exception(
                    $"ProductSize for ProductColorId {item.ProductColorId} and SizeId {item.SizeId} not found."
                );

            var basePrice = productSize.ProductColor.Product.Price;

            var activeDiscount = productSize
                .ProductColor.ProductDiscounts.Where(pd => pd.Discount.IsActive)
                .Select(pd => pd.Discount)
                .FirstOrDefault();

            decimal finalPrice = basePrice;

            if (activeDiscount != null)
            {
                finalPrice = Math.Round(basePrice * (1 - activeDiscount.Percentage / 100), 2);
            }

            total += finalPrice * item.Quantity;
            productSizeMap.Add((productSize, item, finalPrice));
        }

        var payment = new PaymentCardDto
        {
            CardHolder = request.CardHolder!,
            CardNumber = request.CardNumber!,
            Cvv = request.Cvv!,
            Expiration = request.Expiration!,
        };

        if (request.PaymentType == PaymentType.Card)
        {
            var success = _paymentProcessor.ProcessPayment(payment);

            if (!success)
                throw new Exception("Plaćanje karticom nije uspelo.");
        }

        var now = DateTime.UtcNow;

        var order = new Order
        {
            UserId = User.Id,
            TotalPrice = total,
            PaymentType = request.PaymentType,
            OrderDate = now,
            PromisedDate = now.AddWorkingDays(3),
            OrderStatus = OrderStatus.Pending,
            ReceivedDate = null,
            AddressId = request.AddressId,
            Notes = request.Notes,
        };

        Context.Orders.Add(order);
        Context.SaveChanges();

        var orderItems = productSizeMap
            .Select(x => new OrderItem
            {
                OrderId = order.Id,
                ProductSizeId = x.Size.Id,
                Quantity = x.Dto.Quantity,
                Price = x.Price,
            })
            .ToList();

        Context.OrderItems.AddRange(orderItems);
        Context.SaveChanges();
    }
}
