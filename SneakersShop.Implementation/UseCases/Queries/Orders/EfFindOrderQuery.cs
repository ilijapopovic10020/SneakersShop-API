using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Orders;
using SneakersShop.Implementation.Extensions;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Orders;

public class EfFindOrderQuery(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IFindOrderQuery
{
    public int Id => 20;

    public string Name => "Find Order";

    public string Description => "";

    public OrderDto Execute(int search)
    {
        var order = Context.Orders
        .Where(o => o.UserId == User.Id && o.Id == search)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            OrderDate = o.OrderDate,
            PromisedDate = o.PromisedDate,
            ReceivedDate = o.ReceivedDate,
            OrderStatus = o.OrderStatus,
            EstimatedArrival = GetDeliveryEstimateExtension.GetDeliveryEstimate(o.PromisedDate, o.ReceivedDate, o.OrderStatus),
            Street = o.Address.Street,
            City = o.Address.City.Name,
            ZipCode = o.Address.City.ZipCode,
            Items = o.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductColorId = oi.ProductSize.ProductColorId,
                Image = "/images/products/" +
                        oi.ProductSize.ProductColor.ProductImages
                            .Where(pi => pi.Image.Path.Contains("thumb"))
                            .Select(pi => Path.GetFileName(pi.Image.Path))
                            .FirstOrDefault(),
                SizeNumber = oi.ProductSize.Size.Number,
                Name = oi.ProductSize.ProductColor.Product.Name,
                Brand = oi.ProductSize.ProductColor.Product.Brand.Name,
                Category = oi.ProductSize.ProductColor.Product.Category.Name,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        })
        .FirstOrDefault();

        return order;
    }

}
