using System;
using System.Reflection;
using FluentValidation;
using SneakersShop.Application.Uploads;
using SneakersShop.Application.UseCases.Commands.Products;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Validators.Products;

namespace SneakersShop.Implementation.UseCases.Commands.Products;

public class EfCreateProductCommand(SneakersShopDbContext context,
                                    IApplicationUser user,
                                    CreateProductValidator validator,
                                    IBase64FileUploader fileUploader) : EfUseCase(context, user), ICreateProductCommand
{
    private readonly CreateProductValidator _validator = validator;
    private readonly IBase64FileUploader _fileUploader = fileUploader;

    public int Id => 7;

    public string Name => "Create Product";

    public string Description => "";

    public void Execute(CreateProductDto request)
    {
        _validator.ValidateAndThrow(request);

        var product = new Product
        {
            Name = request.Name,
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            Price = request.Price
        };

        Context.Products.Add(product);
        Context.SaveChanges();

        foreach (var color in request.ProductVariants)
        {
            var productColor = new ProductColor
            {
                ProductId = product.Id,
                ColorId = color.ColorId,
                Code = color.Code
            };

            Context.ProductColors.Add(productColor);
            Context.SaveChanges();

            foreach (var imagePath in color.VariantImages)
            {
                var savedPath = _fileUploader.Upload(imagePath.ImagePath, UploadType.ProductImage, Path.GetFileNameWithoutExtension(imagePath.ImageName));
                var image = new Domain.Entities.File
                {
                    Path = savedPath,
                    Size = savedPath.Length,
                };

                Context.Files.Add(image);
                Context.SaveChanges();

                var productImage = new ProductImage
                {
                    ProductColorId = productColor.Id,
                    ImageId = image.Id
                };

                Context.ProductImages.Add(productImage);
            }

            foreach (var sizeId in color.SizeIds)
            {
                var productSize = new ProductSize
                {
                    ProductColorId = productColor.Id,
                    SizeId = sizeId,
                    Quantity = color.Quantity
                };

                Context.ProductSizes.Add(productSize);
            }
        }

        Context.SaveChanges();
    }
}
