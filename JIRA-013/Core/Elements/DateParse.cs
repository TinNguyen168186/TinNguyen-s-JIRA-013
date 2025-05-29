using System;
using System.Globalization;

namespace Core.Elements
{
    public class DateParse
    {
        public static DateTime ParseDate(string dateInput)
        {
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy" , "dd-MMM-yyyy","dd MMMM,yyyy"};

            try
            {
                return DateTime.ParseExact(dateInput, formats, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Invalid date format: {dateInput}");
            }
        }
        public static (int StartYear, int EndYear) ParseYearRange(string range)
        {
            var parts = range.Split('-');
            int start = int.Parse(parts[0].Trim());
            int end = int.Parse(parts[1].Trim());
            return (start, end);
        }
    }
}