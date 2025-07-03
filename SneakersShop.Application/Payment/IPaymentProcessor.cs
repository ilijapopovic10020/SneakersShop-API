using System;

namespace SneakersShop.Application.Payment;

public interface IPaymentProcessor
{
    public bool ProcessPayment(string cardHolder, decimal amount, string cardNumber, string cvv, string exp); 
}
