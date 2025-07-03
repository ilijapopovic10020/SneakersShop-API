using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Reviews;

public interface IGetReviewsQuery : IQuery<PagedSearchId, PagedResponse<ReviewsDto>>
{

}
