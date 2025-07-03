using System;
using FluentValidation;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;

namespace SneakersShop.Implementation.Validators.Brands;

public class CreateBrandValidator : BaseValidator<CreateBrandDto>
{
    public CreateBrandValidator(SneakersShopDbContext context) : base(context)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                            .Must(x => !Context.Brands.Any(b => b.Name == x && b.IsActive))
                            .WithMessage("Name is already in use.");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required.")
                             .Must(x => !Context.Files.Any(f => f.Path == x)).WithMessage("Image path already in use.");
    }
}
