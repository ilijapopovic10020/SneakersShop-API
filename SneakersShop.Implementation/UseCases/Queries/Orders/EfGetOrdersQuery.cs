using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Orders;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Orders;

public class EfGetOrdersQuery(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IGetOrdersQuery
{
    public int Id => 19;

    public string Name => "Search Orders";

    public string Description => "";

    public IEnumerable<OrdersDto> Execute(PagedSearchId search)
    {
        if (User == null || User.Id != search.Id)
            throw new ForbiddenUseCaseExecutionException(User.Identity, Name);

        var orders = Context.Orders.Include(o => o.OrderItems)
                                   .ThenInclude(oi => oi.ProductSize)
                                   .ThenInclude(ps => ps.ProductColor)
                                   .ThenInclude(pc => pc.ProductImages)
                                   .ThenInclude(pi => pi.Image)
                                   .Where(o => o.UserId == search.Id)
                                   .OrderByDescending(o => o.OrderDate)
                                   .AsQueryable();


        return orders.Select(o => new OrdersDto
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            OrderDate = o.OrderDate,
            OrderStatus = o.OrderStatus,
            Items = o.OrderItems.Take(3).Select(oi => new OrdersItemsDto
            {
                Image = $"/images/products/" + Path.GetFileName(
                    oi.ProductSize.ProductColor.ProductImages
                        .Where(pi => pi.Image.Path.Contains("thumb"))
                        .Select(pi => pi.Image.Path)
                        .FirstOrDefault() ?? "")
            })
        });
    }
}
