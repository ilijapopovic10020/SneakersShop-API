using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Orders;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Orders;

public class EfUpdateOrderCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IUpdateOrderCommand
{
    public int Id => 30;

    public string Name => "Update order";

    public string Description => "";

    public void Execute(UpdateOrderDto request)
    {
        var currentOrder = Context.Orders.Include(x => x.OrderItems)
                                         .Include(x => x.User)
                                            .ThenInclude(x => x.Addresses)
                                         .FirstOrDefault(x => x.Id == request.Id)
                                            ?? throw new EntityNotFoundException(request.Id, nameof(Order));

        if (currentOrder.UserId != User?.Id)
        {
            throw new ForbiddenUseCaseExecutionException(nameof(Name), User?.Identity);
        }

        if (currentOrder.OrderStatus != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Narudžbina može da se izmeni samo dok je u statusu obrade.");
        }

        var userAddresses = Context.Addresses
                            .Where(a => a.UserId == currentOrder.UserId)
                            .ToList();

        if (request.AddressId != 0 && userAddresses.Any(a => a.Id == request.AddressId))
        {
            currentOrder.AddressId = request.AddressId;
        }

        Context.OrderItems.RemoveRange(currentOrder.OrderItems);

        decimal newTotal = 0;
        var newItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            var productSize = Context.ProductSizes
                            .Include(ps => ps.ProductColor)
                                .ThenInclude(pc => pc.Product)
                            .FirstOrDefault(ps => ps.ProductColorId == item.ProductColorId && ps.SizeId == item.SizeId)
                                ?? throw new EntityNotFoundException(0, nameof(ProductSize));

            newItems.Add(new OrderItem
            {
                OrderId = currentOrder.Id,
                ProductSizeId = productSize.Id,
                Quantity = item.Quantity,
                Price = productSize.ProductColor.Product.Price
            });

            newTotal += productSize.ProductColor.Product.Price * item.Quantity;
        }

        currentOrder.TotalPrice = newTotal;
        Context.OrderItems.AddRange(newItems);

        Context.SaveChanges();
    }
}
