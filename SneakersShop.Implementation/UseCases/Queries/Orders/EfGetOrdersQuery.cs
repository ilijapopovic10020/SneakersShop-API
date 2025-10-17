using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Orders;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Orders;

public class EfGetOrdersQuery(SneakersShopDbContext context, IApplicationUser? user)
    : EfUseCase(context, user),
        IGetOrdersQuery
{
    public int Id => 19;

    public string Name => "Search Orders";

    public string Description => "";

    public PagedResponse<OrdersDto> Execute(BasePagedSearch search)
    {
        var query = Context
            .Orders.Where(o => o.UserId == User.Id)
            .OrderByDescending(o => o.OrderDate);

        var totalCount = query.Count();

        if (search.PerPage == null || search.PerPage < 1)
            search.PerPage = 15;

        if (search.Page == null || search.Page < 1)
            search.Page = 1;

        var skip = (search.Page.Value - 1) * search.PerPage.Value;

        var data = query
            .Skip(skip)
            .Take(search.PerPage.Value)
            .Select(o => new OrdersDto
            {
                Id = o.Id,
                TotalPrice = o.TotalPrice,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                Items = o
                    .OrderItems.Take(3)
                    .Select(oi => new OrdersItemsDto
                    {
                        Image =
                            "/images/products/"
                            + oi.ProductSize.ProductColor.ProductImages.Where(pi =>
                                    pi.Image.Path.Contains("thumb")
                                )
                                .Select(pi => Path.GetFileName(pi.Image.Path))
                                .FirstOrDefault(),
                        Price = oi.Price,
                        Brand = oi.ProductSize.ProductColor.Product.Brand.Name,
                        Name = oi.ProductSize.ProductColor.Product.Name,
                        Size = oi.ProductSize.Size.Number,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            })
            .ToList();

        return new PagedResponse<OrdersDto>
        {
            TotalCount = totalCount,
            CurrentPage = search.Page.Value,
            ItemsPerPage = search.PerPage.Value,
            Data = data,
        };
    }
}
