using MD.PersianDateTime.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaysanPwa.Application.Utilities.DateAndTime
{
    public static class DateTimeExtensions
    {
        public static string GetPersianDay(string enDay)
        {
            string day = "";
            switch (enDay)
            {
                case "Saturday":
                    day = "شنبه";
                    break;
                case "Sunday":
                    day = "یک شنبه";
                    break;
                case "Monday":
                    day = "دو شنبه";
                    break;
                case "Tuesday":
                    day = "سه شنبه";
                    break;
                case "Wednesday":
                    day = "چهار شنبه";
                    break;
                case "Thursday":
                    day = "پنج شنبه";
                    break;
                case "Friday":
                    day = "جمعه";
                    break;
            }

            return day;
        }

        public static DateTime ConvertShamsiToMiladi(this string date)
        {
            PersianDateTime persianDateTime = PersianDateTime.Parse(date);
            return persianDateTime.ToDateTime();
        }

        public static string ConvertMiladiToShamsi(this DateTime? date, string format)
        {
            PersianDateTime persianDateTime = new PersianDateTime(date);
            return persianDateTime.ToString(format);
        }

        public static DateTimeResult CheckShamsiDateTime(this string date)
        {
            try
            {
                DateTime miladiDate = PersianDateTime.Parse(date).ToDateTime();
                return new DateTimeResult { MiladiDate = miladiDate, IsShamsi = true };
            }

            catch
            {
                return new DateTimeResult { IsShamsi = false };
            }
        }

        public static DateTime DateTimeWithOutMilliSecends(DateTime dateTime) => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));

        public static List<DateTime?> GetDateTimeForSearch(this string searchText)
        {
            DateTime? startDateTime = Convert.ToDateTime("01/01/01");
            DateTime? endDateTime = Convert.ToDateTime("01/01/01");
            var dateTimeResult = searchText.CheckShamsiDateTime();

            if (dateTimeResult.IsShamsi)
            {
                startDateTime = dateTimeResult.MiladiDate;
                if (searchText.Contains(":"))
                    endDateTime = startDateTime;
                else
                    endDateTime = startDateTime.Value.Date + new TimeSpan(23, 59, 59);
            }

            return new List<DateTime?>() { startDateTime, endDateTime };
        }

        public static string FirstCharToUpper(this string input) =>
       input switch
       {
            null => throw new ArgumentNullException(nameof(input)),
           "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
           _ => input.First().ToString().ToUpper() + input.Substring(1)
           
       };
    }

    public class DateTimeResult
    {
        public bool IsShamsi { get; set; }
        public DateTime? MiladiDate { get; set; }
    }

}
