using System;

namespace SneakersShop.Implementation.Extensions;

public static class DateTimeExtensions
{
    public static DateTime AddWorkingDays(this DateTime date, int workingDays)
    {
        var result = date;
        while (workingDays > 0)
        {
            result = result.AddDays(1);
            if (result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
            {
                workingDays--;
            }
        }

        return result;
    }
}
