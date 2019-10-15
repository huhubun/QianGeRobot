using Microsoft.AspNetCore.Mvc;
using QianGeRobot.Core;
using QianGeRobot.Core.TuiCalendar;
using QianGeRobot.Services;
using QianGeRobot.Web.Models;
using QianGeRobot.Web.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QianGeRobot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly QianGeConfigs _qianGeConfigs;
        private readonly DingTalkService _dingTalkService;
        private readonly QianGeService _qianGeService;
        private readonly TracService _tracService;

        public HomeController(QianGeConfigs qianGeConfigs, DingTalkService dingTalkService, QianGeService qianGeService, TracService tracService)
        {
            _qianGeConfigs = qianGeConfigs;
            _dingTalkService = dingTalkService;
            _qianGeService = qianGeService;
            _tracService = tracService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMessage()
        {
            var model = new SendMessageModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult SendMessage(SendMessageModel model)
        {
            switch (model.Type)
            {
                case "SendText":
                    _dingTalkService.SendText(model.Content, atAll: model.AtAll);
                    break;

                case "SendNotCompletedInDevDeadlineTicketMessage":
                    _qianGeService.SendNotCompletedInDevDeadlineTicketsMessage();
                    break;

                case "SendTodayShouldCompletedTicketsMessage":
                    _qianGeService.SendTodayShouldCompletedTicketsMessage();
                    break;

                case "SendTodayShouldStartTicketsMessage":
                    _qianGeService.SendTodayShouldStartTicketsMessage();
                    break;

                case "SendCompletionOfMonthMessage":
                    _qianGeService.SendCompletionOfMonthMessage();
                    break;

                case "SendCompletionOfMonthMessageByMonth":
                    var year = model.Year;
                    var month = model.Month;
                    var date = new DateTime(year, month, 1);
                    var nextMonth = date.AddMonths(1);
                    date = nextMonth.AddDays(-1);

                    _qianGeService.SendCompletionOfMonthMessage(date);
                    break;

                default:
                    throw new ArgumentException("Type 值错误");
            }

            return RedirectToAction(nameof(SendMessage));
        }

        public IActionResult GetSchedules()
        {
            var currentDate = DateTime.Now;

            var tickets = _tracService.GetMonthlyTickets(currentDate.Year, currentDate.Month);
            var peopleDic = GetConfiguredPeoples().ToDictionary(p => p.Username);

            var schedules = tickets.Select(t => new Schedule
            {
                Id = t.TicketId.ToString(),
                CalendarId = peopleDic.ContainsKey(t.Owner) ? peopleDic[t.Owner].Id.ToString() : "99999",
                Title = $"#{t.TicketId} {t.Status} {t.Hotel} {t.Summary}",
                Category = "time",
                Start = t.Startdate ?? t.CreatedTime.Date,
                End = t.Deadline4Dev.Value,
                IsReadOnly = true,
                IsAllDay = true,
                Location = t.Hotel
            });

            return Json(schedules);
        }

        public IActionResult GetCalendars()
        {
            var peoples = GetConfiguredPeoples();

            var calendars = new List<Calendar>();

            foreach (var people in peoples)
            {
                var calendarColor = ColorConst.GetCalendarColor(people.Id);

                calendars.Add(new Calendar
                {
                    Id = people.Id.ToString(),
                    Name = $"{people.Name} ({people.Username})",
                    Color = calendarColor.Color,
                    BgColor = calendarColor.BgColor,
                    BorderColor = calendarColor.BorderColor
                });
            }

            return Json(calendars);
        }

        public IActionResult GetPeoples()
        {
            return Json(GetConfiguredPeoples());
        }

        private List<PeopleConfig> GetConfiguredPeoples()
        {
            return _qianGeConfigs.Peoples.Select((p, index) => new PeopleConfig
            {
                Id = index,
                Username = p.Username,
                Name = p.Name
            }).ToList();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
