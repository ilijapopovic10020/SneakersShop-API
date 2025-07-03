using FluentValidation;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Reviews;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Validators.Reviews;

namespace SneakersShop.Implementation.UseCases.Commands.Reviews;

public class EfCreateReviewCommand(SneakersShopDbContext context, IApplicationUser user, CreateReviewValidator validator) : EfUseCase(context, user), ICreateReviewCommand
{
    private readonly CreateReviewValidator _validator = validator;
    public int Id => 16;

    public string Name => "Create Review";

    public string Description => "";

    public void Execute(CreateReviewDto request)
    {
        if (!Context.Products.Any(x => x.Id == request.ProductId))
        {
            throw new EntityNotFoundException(request.ProductId, nameof(Product));
        }

        if (!Context.Users.Any(x => x.Id == request.UserId))
        {
            throw new EntityNotFoundException(request.UserId, nameof(User));
        }

        _validator.ValidateAndThrow(request);

        var review = new Review
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            Comment = request.Comment,
            Rating = request.Rating
        };

        Context.Reviews.Add(review);
        Context.SaveChanges();
    }
}
