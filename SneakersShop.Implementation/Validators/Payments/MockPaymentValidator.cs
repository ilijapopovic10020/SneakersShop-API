using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Implementation.Validators.Payments;

public class MockPaymentValidator : AbstractValidator<PaymentCardDto>
{
    public MockPaymentValidator()
    {
        RuleFor(x => x.CardNumber)
                    .NotNull().WithMessage("Broj kartice je obavezno polje.")
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                        .WithMessage("Broj kartice je obavezno polje.")
                    .Must(x => Regex.IsMatch(x ?? "", @"^\d{4} \d{4} \d{4} \d{4}$"))
                        .WithMessage("Broj kartice mora biti u formatu: xxxx xxxx xxxx xxxx.")
                    .Must(x => x != null && (x.StartsWith("4") || x.StartsWith("5") || x.StartsWith("2")))
                        .WithMessage("Podržane su samo Visa i MasterCard kartice.");

        RuleFor(x => x.CardHolder)
            .NotNull().WithMessage("Vlasnik kartice je obavezno polje.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Vlasnik kartice je obavezno polje.")
            .Must(x => Regex.IsMatch(x ?? "", @"^[A-ZČĆŽŠĐa-zčćžšđ]+\s[A-ZČĆŽŠĐa-zčćžšđ]+$"))
                .WithMessage("Vlasnik kartice nije u ispravnom formatu.");

        RuleFor(x => x.Cvv)
            .NotNull().WithMessage("Cvv je obavezno polje.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Cvv je obavezno polje.")
            .Must(x => Regex.IsMatch(x ?? "", @"^\d{3}$"))
                .WithMessage("Cvv nije u ispravnom formatu.");

        RuleFor(x => x.Expiration)
            .NotNull().WithMessage("Datum isteka kartice je obavezno polje.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Datum isteka kartice je obavezno polje.")
            .Must(x =>
            {
                if (x == null || string.IsNullOrWhiteSpace(x))
                    return false;

                var valid = DateTime.TryParseExact(
                    "01/" + x,
                    "dd/MM/yy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var expDate);

                return valid && expDate > DateTime.Now;
            })
                .WithMessage("Neispravan datum ili je kartica istekla. Ispravan format: mm/gg.");
    }
}
