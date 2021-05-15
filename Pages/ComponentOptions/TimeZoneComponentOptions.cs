using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeSite.Pages.ComponentOptions
{
    public class TimeZoneComponentOptions
    {
        public DateTime DateTimeUtc { get; set; }
        public bool UseSmallClass { get; set; }
        public TimeZoneComponentOptions(string dateTimeStringUtc, bool useSmallClass = true)
        {
            DateTime.TryParse(dateTimeStringUtc, out DateTime parsedDateTime);
            DateTimeUtc = parsedDateTime;
            UseSmallClass = useSmallClass;
        }

        public TimeZoneComponentOptions(DateTime dateTimeUtc, bool useSmallClass = true)
        {
            DateTimeUtc = dateTimeUtc;
            UseSmallClass = useSmallClass;
        }
    }
}
