using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using SneakersShop.Application.Payment;

namespace SneakersShop.Implementation.Paymenet;

public partial class MockPaymentProcessor : IPaymentProcessor
{
    public bool ProcessPayment(string cardHolder, decimal amount, string cardNumber, string cvv, string expiration)
    {
        if (!CardNumberRegex().IsMatch(cardNumber))
            throw new ValidationException("Neispravan format broja kartice. Ispravan je: #### #### #### ####.");

        if (!cardNumber.StartsWith("4") ||  !cardNumber.StartsWith("5") || !cardNumber.StartsWith("2"))
            throw new ValidationException("Samo Visa i MasterCard su podržane.");

        if (!CvvRegex().IsMatch(cvv))
            throw new ValidationException("CVV mora imati tačno 3 cifre.");

        if (!DateTime.TryParseExact("01/" + expiration, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expDate))
            throw new ValidationException("Neispravan format datuma isteka. Ispravan je: MM/yy.");

        if (expDate < DateTime.Now)
            throw new ValidationException("Kartica je istekla.");

        if (!CardHolderRegex().IsMatch(cardHolder))
            throw new ValidationException("Ime vlasnika kartice može sadržati samo slova i razmake.");

        return true;
    }

    [GeneratedRegex(@"^\d{4} \d{4} \d{4} \d{4}$")]
    private static partial Regex CardNumberRegex();
    
    [GeneratedRegex(@"^\d{3}$")]
    private static partial Regex CvvRegex();
    
    [GeneratedRegex(@"^[A-ZČĆŽŠĐa-zčćžšđ]\s[A-ZČĆŽŠĐa-zčćžšđ]+$")]
    private static partial Regex CardHolderRegex();
}
