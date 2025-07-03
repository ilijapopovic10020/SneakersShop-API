using System;

namespace SneakersShop.Application.Emails;

public interface IEmailSender
{
    void Send(MailMessage mailMessage);
}
