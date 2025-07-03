using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Microsoft.Identity.Client.Extensibility;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;

namespace SneakersShop.Implementation.Validators.Products;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    private readonly SneakersShopDbContext _context;
    public CreateProductValidator(SneakersShopDbContext context) => _context = context;
    public CreateProductValidator() 
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
                            .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
                            .Must(x => !_context.Products.Any(p => p.Name == x)).WithMessage("Name is already in use.");

        RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand is required.")
                               .GreaterThan(0).WithMessage("Brand Id must be greater then 0.")
                               .Must(x => !_context.Brands.Any(b => b.Id == x)).WithMessage($"Selected brand does not exist.");

        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.")
                                  .GreaterThan(0).WithMessage("Category Id must be greater then 0.")
                                  .Must(x => !_context.Categories.Any(b => b.Id == x)).WithMessage($"Selected category does not exist.");

        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required.")
                             .GreaterThan(0).WithMessage("Price must be greater then 0.");

        RuleFor(x => x.ProductVariants).NotEmpty().WithMessage("At least one color must be provided.");

        RuleForEach(x => x.ProductVariants).SetValidator(new CreateProductVariantValidator());
    }
}

public class CreateProductVariantValidator : AbstractValidator<CreateProductVariantDto>
{
    private readonly SneakersShopDbContext _context;
    public CreateProductVariantValidator(SneakersShopDbContext context) => _context = context;
    public CreateProductVariantValidator()
    {
        RuleFor(x => x.ColorId).NotEmpty().WithMessage("Color is required.")
                               .GreaterThan(0).WithMessage("Color Id must be greater then 0.")
                               .Must(x => !_context.Colors.Any(b => b.Id == x)).WithMessage($"Selected color does not exist.");

        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.")
                            .Must(x => !_context.ProductColors.Any(pc => pc.Code == x)).WithMessage("Code is already in use.");

        RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required.")
                                .GreaterThan(0).WithMessage("Quantity must be greater then 0.");

        RuleForEach(x => x.VariantImages).NotEmpty().WithMessage("At least one image must be provided.")
                                  .SetValidator(new CreateProductVariantImagesValidator());

        RuleForEach(x => x.SizeIds).NotEmpty().WithMessage("At least one size must be provided")
                                   .GreaterThan(0).WithMessage("Size Id must be greater then 0.")
                                   .Must(x => !_context.Sizes.Any(b => b.Id == x)).WithMessage($"Selected size does not exist.");
    }
}

public class CreateProductVariantImagesValidator : AbstractValidator<CreateProductVaraintImageDto>
{
    private readonly SneakersShopDbContext _context;
    public CreateProductVariantImagesValidator(SneakersShopDbContext context) => _context = context;
    public CreateProductVariantImagesValidator()
    {
        RuleFor(x => x.ImagePath)
            .NotEmpty().WithMessage("Image path is required.")
            .Must(path => !_context.Files.Any(f => f.Path == path))
            .WithMessage("Image path is already in use.");
    }
}
