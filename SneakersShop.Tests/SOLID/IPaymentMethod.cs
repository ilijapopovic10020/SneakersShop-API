using System;

namespace SneakersShop.Tests.SOLID;

public interface IPaymentMethod
{
    bool Pay(decimal amount);
}

public interface ILogger
{
    void Log(string msg);
}

public class CreditCardPaymentMethod : IPaymentMethod, ILogger
{
    private string _cardNumber;
    private int _ccv;
    private string _exp; // "01/22"

    public CreditCardPaymentMethod(string cardNumber, int ccv, string exp)
    {
        _cardNumber = cardNumber;
        _ccv = ccv;
        _exp = exp;
    }

    public void Log(string msg)
    {
        throw new NotImplementedException();
    }

    public bool Pay(decimal amount)
    {
        System.Console.WriteLine("Paying with card...");
        return true;
    }
}

public class PayPalPaymentMethod : IPaymentMethod
{
    public bool Pay(decimal amount)
    {
        System.Console.WriteLine("Paying with paypal...");
        return true;
    }
}

public class BankPaymentMethod : IPaymentMethod
{
    public bool Pay(decimal amount)
    {
        System.Console.WriteLine("Paying with bank...");
        return true;
    }
}
