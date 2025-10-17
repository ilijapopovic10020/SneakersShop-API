using System;
using Azure.Core.Pipeline;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Reviews;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Reviews;

public class EfGetReviewsQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetReviewsQuery
{
    public int Id => 15;

    public string Name => "Search Reviews";

    public string Description => "";

    public PagedResponse<ReviewsDto> Execute(PagedSearchId search)
    {
        var product = Context.ProductColors.Where(pc => pc.Id == search.Id)
                                           .Select(pc => pc.ProductId)
                                           .FirstOrDefault();
        
        var reviews = Context.Reviews.Include(r => r.User)
                                     .ThenInclude(u => u.Image)
                                     .Where(r => r.ProductId == product)
                                     .OrderByDescending(x => x.Id)
                                     .AsQueryable();

        if (search.PerPage == null || search.PerPage < 1)
        {
            search.PerPage = 15;
        }

        if (search.Page == null || search.Page < 1)
        {
            search.Page = 1;
        }

        var skip = (search.Page.Value - 1) * search.PerPage.Value;

        var response = new PagedResponse<ReviewsDto>();
        response.TotalCount = reviews.Count();
        response.Data = reviews.Skip(skip).Take(search.PerPage.Value).Select(x => new ReviewsDto
        {
            Id = x.Id,
            UserId = x.UserId,
            ProductId = x.ProductId,
            Username = x.User.Username,
            UserImage = x.User.Image != null ? $"/images/profile/{Path.GetFileName(x.User.Image.Path)}" : null,
            Comment = x.Comment,
            Rating = x.Rating,
            CreatedAt = x.CreatedAt.ToString("dd. MMMM yyyy.", new System.Globalization.CultureInfo("rs-Latn-RS")),
        });
        response.CurrentPage = search.Page.Value;
        response.ItemsPerPage = search.PerPage.Value;

        return response;

    }
}
