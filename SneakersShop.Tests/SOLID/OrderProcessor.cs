namespace SneakersShop.Tests.SOLID;

public class OrderProcessor
{
    private IPaymentMethod _paymentMethod;

    public bool emailSent = false;

    public OrderProcessor(IPaymentMethod paymentMethod)
    {
        _paymentMethod = paymentMethod;
    }

    public void ProcessOrder(Order order)
    {
        emailSent = false;
        // _paymentMethod = new CreditCardPaymentMethod("8829-0488-0401-1111", 444, "11/25");
        //Ispisati Stavke
        foreach (var ol in order.Lines)
        {
            Console.WriteLine(ol.Name + ": " + ol.Price);
        }

        Console.WriteLine("Total: " + order.Lines.Sum(x => x.Price));

        //Izvrsiti Placanje
        var result = _paymentMethod.Pay(order.Lines.Sum(x => x.Price * x.Quantity));

        //Poslati email ili baciti izuzetak
        if (!result)
        {
            throw new Exception("Placanje neuspesno");
        }

        SendEmail();
    }

    public void ProcessOrder(IEnumerable<OrderLine> lines)
    {
        emailSent = false;
        // _paymentMethod = new CreditCardPaymentMethod("8829-0488-0401-1111", 444, "11/25");
        //Ispisati Stavke
        foreach (var ol in lines)
        {
            Console.WriteLine(ol.Name + ": " + ol.Price);
        }

        Console.WriteLine("Total: " + lines.Sum(x => x.Price));

        //Izvrsiti Placanje
        var result = _paymentMethod.Pay(lines.Sum(x => x.Price * x.Quantity));

        //Poslati email ili baciti izuzetak
        if (!result)
        {
            throw new Exception("Placanje neuspesno");
        }

        SendEmail();
    }

    private void SendEmail()
    {
        emailSent = true;
    }
}

public class Order
{
    public IEnumerable<OrderLine> Lines { get; set; }
}

public class OrderLine
{
    public decimal Price { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
}