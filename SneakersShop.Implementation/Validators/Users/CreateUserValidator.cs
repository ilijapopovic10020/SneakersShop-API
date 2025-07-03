using FluentValidation;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;

namespace SneakersShop.Implementation.Validators.Users;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator(SneakersShopDbContext context)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno polje.")
            .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
            .WithMessage("Ime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više imena.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Prezime je obavezno polje.")
            .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
            .WithMessage("Prezime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više prezimena.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("E-adresa je obavezno polje.")
                             .EmailAddress()
                             .WithMessage("E-adresa nije u ispravnom formatu.")
                             .Must(x => !context.Users.Any(u => u.Email == x))
                             .WithMessage("E-adresa je već u upotrebi.");

        RuleFor(x => x.Username).NotEmpty().WithMessage("Korisničko ime je obavezno polje.")
                                .Matches("^(?=[a-zA-Z0-9._]{4,30}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                                .WithMessage("Korisničko ime nije u ispravnom formatu. Minimum 4, maksimum 30 karaktera, slova, brojevi i specijalni karakteri (.,_)")
                                .Must(x => !context.Users.Any(u => u.Username == x))
                                .WithMessage("Korisničko ime je već u upotrebi.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Lozinka je obavezno polje.")
                                 .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                                 .WithMessage("Lozinka nije u ispravnom formatu.");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Broj telefona je obavezno polje.")
                     .Matches(@"^06\d{7,8}$")
                     .WithMessage("Broj telefona mora početi sa 06 i imati ukupno 9 ili 10 cifara (npr. 0632389912).");
    }
}
