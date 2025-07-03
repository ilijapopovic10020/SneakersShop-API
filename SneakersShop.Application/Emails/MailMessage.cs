using System;

namespace SneakersShop.Application.Emails;

public class MailMessage
{
    public string To { get; set; }
    public string From { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
