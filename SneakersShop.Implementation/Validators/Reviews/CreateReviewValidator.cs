using System;
using FluentValidation;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;

namespace SneakersShop.Implementation.Validators.Reviews;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.")
            .Must(x => x > 0).WithMessage("Product ID must be a positive integer.");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.")
            .Must(x => x > 0).WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.")
            .MinimumLength(3).WithMessage("Comment must be at least 3 characters long.")
            .MaximumLength(150).WithMessage("Comment must be at most 150 characters long.");

        RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating is required.")
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
    }
}
