using System.Globalization;
using System.Text;

namespace SaysanPwa.Application.Utilities.DateAndTime;

public static class DateTimeConvertor
{
    public static string ToPersianData(this DateTime currentDate)
    {
        PersianCalendar pc = new();
        return new StringBuilder()
            .Append(pc.GetYear(currentDate).ToString())
            .Append("/")
            .Append(pc.GetMonth(currentDate).ToString("00"))
            .Append("/")
            .Append(pc.GetDayOfMonth(currentDate).ToString("00"))
            .Append(" - ")
            .Append(pc.GetHour(currentDate).ToString("00"))
            .Append(":")
            .Append(pc.GetMinute(currentDate).ToString("00"))
            .Append(":")
            .Append(pc.GetSecond(currentDate).ToString("00"))
            .ToString();
    }
}
