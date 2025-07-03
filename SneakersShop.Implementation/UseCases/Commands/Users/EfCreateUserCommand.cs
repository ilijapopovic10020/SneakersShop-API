using FluentValidation;
using SneakersShop.Application.Emails;
using SneakersShop.Application.Uploads;
using SneakersShop.Application.UseCases.Commands.Users;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.Validators.Users;

namespace SneakersShop.Implementation.UseCases.Commands.Users;

public class EfCreateUserCommand(SneakersShopDbContext context,
                                IApplicationUser user,
                                CreateUserValidator validator, 
                                IBase64FileUploader fileUploader,
                                IEmailSender emailSender) : EfUseCase(context, user), ICreateUserCommand
{
    private readonly CreateUserValidator _validator = validator;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IBase64FileUploader _fileUploader = fileUploader;

    public int Id => 4;

    public string Name => "Create User";

    public string Description => "";

    public void Execute(CreateUserDto request)
    {
        _validator.ValidateAndThrow(request);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        int? imageId = null;

        if(!string.IsNullOrEmpty(request.Image))
        {
            var path = _fileUploader.Upload(request.Image, UploadType.ProfileImage, $"{request.Username + Guid.NewGuid()}");

            var file = new Domain.Entities.File
            {
                Path = path,
                Size = (int) new FileInfo(path).Length,
            };

            Context.Files.Add(file);
            Context.SaveChanges();

            imageId = file.Id;
        }
        else
        {
            imageId = 1; // default image id
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Username = request.Username,
            Phone = request.Phone,
            Password = passwordHash,
            ImageId = imageId,
            RoleId = 7

        };

        Context.Users.Add(user);
        Context.SaveChanges();

        _emailSender.Send(new MailMessage
        {
            From = "noreplay@sneakersshop.com",
            To = request.Email,
            Title = "Confirm registration",
            Body = "Dear ... "
        });
    }
}
