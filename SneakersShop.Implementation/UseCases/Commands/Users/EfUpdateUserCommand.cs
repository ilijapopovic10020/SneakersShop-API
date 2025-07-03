using FluentValidation;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.Uploads;
using SneakersShop.Application.UseCases.Commands.Users;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Implementation.Validators.Users;

namespace SneakersShop.Implementation.UseCases.Commands.Users;

public class EfUpdateUserCommand(SneakersShopDbContext context,
                                 IApplicationUser? user,
                                 UpdateUserValidator validator,
                                 IBase64FileUploader fileUploader) : EfUseCase(context, user), IUpdateUserCommand
{
    private readonly UpdateUserValidator _validator = validator;
    private readonly IBase64FileUploader _fileUploader = fileUploader;
    public int Id => 24;

    public string Name => "Update user";

    public string Description => "";

    public void Execute(UpdateUserDto request)
    {
        _validator.ValidateAndThrow(request);
        var currentUser = Context.Users.FirstOrDefault(x => x.Id == request.Id) ?? throw new EntityNotFoundException(User.Id, nameof(Domain.Entities.User));


        if (!string.IsNullOrWhiteSpace(request.FirstName))
            if (currentUser.FirstName != request.FirstName)
                currentUser.FirstName = request.FirstName;

        if (!string.IsNullOrWhiteSpace(request.LastName))
            if (currentUser.LastName != request.LastName)
                currentUser.LastName = request.LastName;

        if (!string.IsNullOrWhiteSpace(request.Email))
            if (currentUser.Email != request.Email)
                currentUser.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            if (currentUser.Phone != request.Phone)
                currentUser.Phone = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.Image))   
        {
            int? imageId = null;

            if (!string.IsNullOrEmpty(request.Image))
            {
                var path = _fileUploader.Upload(request.Image, UploadType.ProfileImage, $"{User.Identity + Guid.NewGuid()}");

                var file = new Domain.Entities.File
                {
                    Path = path,
                    Size = (int)new FileInfo(path).Length,
                };

                Context.Files.Add(file);
                Context.SaveChanges();

                imageId = file.Id;
            }
            else
            {
                imageId = 1;
            }
            currentUser.ImageId = imageId;
        }

        Context.SaveChanges();
    }
}

