using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.Payment;

public interface IPaymentProcessor
{
    public bool ProcessPayment(PaymentCardDto payment); 
}
