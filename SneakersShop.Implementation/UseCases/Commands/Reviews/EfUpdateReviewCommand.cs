using FluentValidation;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Reviews;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Validators.Reviews;

namespace SneakersShop.Implementation.UseCases.Commands.Reviews;

public class EfUpdateReviewCommand(SneakersShopDbContext context, IApplicationUser? user, UpdateReviewValidator validator) : EfUseCase(context, user), IUpdateReviewCommand
{
    private readonly UpdateReviewValidator _validator = validator;
    public int Id => 28;

    public string Name => "Update review";

    public string Description => "";

    public void Execute(UpdateReviewDto request)
    {
        _validator.ValidateAndThrow(request);

        var currentReview = Context.Reviews.FirstOrDefault(x => x.Id == request.Id) ?? throw new EntityNotFoundException(request.Id, nameof(Review));
        if (currentReview.UserId != User?.Id)
        {
            throw new UnauthorizedAccessException("Niste ovlašćeni da menjate ovu recenziju.");
        }

        if (!string.IsNullOrWhiteSpace(request.Comment))
            if (currentReview.Comment != request.Comment)
                currentReview.Comment = request.Comment;

        if (currentReview.Rating != request.Rating)
            currentReview.Rating = request.Rating;

    }
}
