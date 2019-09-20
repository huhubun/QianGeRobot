using System;

namespace QianGeRobot.Core.TuiCalendar
{
    public class Schedule
    {
        public string Id { get; set; }

        public string CalendarId { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsReadOnly { get; set; }

        public string Body { get; set; }

        public bool IsAllDay { get; set; }

        public string Location { get; set; }
    }
}
