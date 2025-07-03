using FluentValidation;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(SneakersShopDbContext context)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno.")
            .Matches("^[A-Z][a-z]+$").WithMessage("Ime mora početi velikim slovom i sadržati samo slova.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Prezime je obavezno.")
            .Matches("^[A-Z][a-z]+$").WithMessage("Prezime mora početi velikim slovom i sadržati samo slova.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-adresa je obavezna.")
            .EmailAddress().WithMessage("E-adresa nije u dobrom formatu.")
            .Must((dto, email) =>
            {
                return !context.Users.Any(u => u.Email == email && u.Id != dto.Id);
            })
            .WithMessage("E-adresa je već u upotrebi.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefon je obavezan.")
            .Matches(@"^06[0-9]{7,8}$").WithMessage("Telefon mora biti u formatu npr. 0631234567 ili 06012345678.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Image)
            .Must(path => string.IsNullOrEmpty(path) || path.EndsWith(".jpg") || path.EndsWith(".png"))
            .WithMessage("Slika mora biti u formatu .jpg ili .png");
    }
}
