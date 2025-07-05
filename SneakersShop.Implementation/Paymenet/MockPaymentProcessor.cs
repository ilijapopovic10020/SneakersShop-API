using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using SneakersShop.Application.Payment;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Implementation.Validators.Payments;

namespace SneakersShop.Implementation.Paymenet;

public partial class MockPaymentProcessor(MockPaymentValidator validator) : IPaymentProcessor
{
    private readonly MockPaymentValidator _validator = validator;

    public bool ProcessPayment(PaymentCardDto payment)
    {
        var result = _validator.Validate(payment);

        return result.IsValid;
    }
}
