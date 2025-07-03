using FluentValidation;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Implementation.Validators.Reviews;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
{ 
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Comment).NotEmpty().WithMessage("Komentar je obavezan.")
            .MinimumLength(3).WithMessage("Komentar mora imati najmanje 3 karaktera.")
            .MaximumLength(150).WithMessage("Komentar ne sme imati više od 150 karaktera.");

        RuleFor(x => x.Rating).NotEmpty().WithMessage("Ocena je obavezna.")
            .InclusiveBetween(1, 5).WithMessage("Ocena mora biti između 1 i 5.");
    }
}
