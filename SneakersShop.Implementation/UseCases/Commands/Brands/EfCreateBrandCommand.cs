using FluentValidation;
using SneakersShop.Application.Uploads;
using SneakersShop.Application.UseCases.Commands.Brands;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Validators.Brands;

namespace SneakersShop.Implementation.UseCases.Commands.Brands;

public class EfCreateBrandCommand(SneakersShopDbContext context,
                                  IApplicationUser user,
                                  CreateBrandValidator validator,
                                  IBase64FileUploader fileUploader) : EfUseCase(context, user), ICreateBrandCommand
{
    private readonly IBase64FileUploader _fileUploader = fileUploader;
    private readonly CreateBrandValidator _validator = validator;

    public int Id => 2;

    public string Name => "Create Brand";

    public string Description => "Create brand using entity framework";

    public void Execute(CreateBrandDto request)
    {
        _validator.ValidateAndThrow(request);

        var imagePath = _fileUploader.Upload(request.Image, UploadType.BrandImage, null);

        var file = new Domain.Entities.File
        {
            Path = imagePath,
            Size = (int)new FileInfo(imagePath).Length
        };

        var brand = new Brand
        {
            Name = request.Name,
            Image = file
        };
        Context.Files.Add(file);
        Context.Brands.Add(brand);
        Context.SaveChanges();
    }
}
