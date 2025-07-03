using System;
using SneakersShop.Application.Emails;

namespace SneakersShop.Implementation.Emails;

public class FakeEmailSender : IEmailSender
{
    public void Send(MailMessage mailMessage)
    {
        System.Console.WriteLine("Sending email to: " + mailMessage.To);
    }
}
