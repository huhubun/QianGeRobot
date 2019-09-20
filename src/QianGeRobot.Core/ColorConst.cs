using QianGeRobot.Core.TuiCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QianGeRobot.Core
{
    public static class ColorConst
    {
        public static Calendar GetCalendarColor(int index)
        {
            return CalendarColors.ToList()[index];
        }

        private static IEnumerable<Calendar> CalendarColors
        {
            get
            {
                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#81C896",
                    BorderColor = "#81C896"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#1C448E",
                    BorderColor = "#1C448E"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#667782",
                    BorderColor = "#667782"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#575B37",
                    BorderColor = "#575B37"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#FF4040",
                    BorderColor = "#FF4040"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#9D9D9D",
                    BorderColor = "#9D9D9D"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#00A9FF",
                    BorderColor = "#00A9FF"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#03BD9E",
                    BorderColor = "#03BD9E"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#FF5583",
                    BorderColor = "#FF5583"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#FFBB3B",
                    BorderColor = "#FFBB3B"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#C57B57",
                    BorderColor = "#C57B57"
                };

                yield return new Calendar
                {
                    Color = "#FFF",
                    BgColor = "#F1AB86",
                    BorderColor = "#F1AB86"
                };

            }
        }
    }
}
