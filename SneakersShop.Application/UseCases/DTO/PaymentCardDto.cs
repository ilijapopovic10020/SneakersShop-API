using System;
using Azure.Core.Pipeline;
using Microsoft.VisualBasic;

namespace SneakersShop.Application.UseCases.DTO;

public class PaymentCardDto
{
    public string? CardHolder { get; set; }
    public string? CardNumber { get; set; }
    public string? Cvv { get; set; }
    public string? Expiration { get; set; }
}
