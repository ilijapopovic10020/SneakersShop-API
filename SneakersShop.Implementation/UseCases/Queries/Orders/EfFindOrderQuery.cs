using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Orders;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Extensions;

namespace SneakersShop.Implementation.UseCases.Queries.Orders;

public class EfFindOrderQuery(SneakersShopDbContext context, IApplicationUser? user)
    : EfUseCase(context, user),
        IFindOrderQuery
{
    public int Id => 20;

    public string Name => "Find Order";

    public string Description => "";

    public OrderDto Execute(int search)
    {
        var order =
            Context
                .Orders.Include(o => o.Address)
                .ThenInclude(a => a.City)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductSize)
                .ThenInclude(ps => ps.ProductColor)
                .ThenInclude(pc => pc.ProductImages)
                .ThenInclude(pi => pi.Image)
                .FirstOrDefault(o => o.Id == search)
            ?? throw new EntityNotFoundException(search, nameof(Order));

        if (order.UserId != User.Id)
            throw new UnauthorizedAccessException();

        return new OrderDto
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            OrderDate = order.OrderDate,
            PromisedDate = order.PromisedDate,
            ReceivedDate = order.ReceivedDate,
            OrderStatus = order.OrderStatus,
            EstimatedArrival = GetDeliveryEstimateExtension.GetDeliveryEstimate(
                order.PromisedDate,
                order.ReceivedDate,
                order.OrderStatus
            ),
            Street = order.Address.Street,
            City = order.Address.City.Name,
            ZipCode = order.Address.City.ZipCode,
            Items = order
                .OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductColorId = oi.ProductSize?.ProductColorId ?? 0,
                    Image =
                        "/images/products/"
                        + (
                            oi.ProductSize?.ProductColor?.ProductImages?.Where(pi =>
                                    pi.Image != null && pi.Image.Path.Contains("thumb")
                                )
                                .Select(pi => Path.GetFileName(pi.Image.Path))
                                .FirstOrDefault() ?? "default.png"
                        ),
                })
                .ToList(),
        };
    }
}
