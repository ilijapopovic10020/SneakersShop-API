using System;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.Extensions;

public static class GetDeliveryEstimateExtension
{
    public static string GetDeliveryEstimate(DateTime promisedDate, DateTime? receivedDate, OrderStatus orderStatus)
    {
        if (promisedDate == null)
            return "Unknown";

        var today = DateTime.Today;


        if (orderStatus == OrderStatus.Cancelled || receivedDate != null)
        {
            return null;
        }

        var diff = (promisedDate.Date - today).Days;

        return diff switch
        {
            <= 0 => "Danas",
            1 => "Sutra",
            _ => $"Za {diff} Dana",
        };


    }
}
