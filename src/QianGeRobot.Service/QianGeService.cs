using QianGeRobot.Core;
using QianGeRobot.Core.Trac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QianGeRobot.Services
{
    public class QianGeService
    {
        private readonly QianGeConfigs _configs;
        private readonly TracService _tracService;
        private readonly DingTalkService _dingTalkService;

        public QianGeService(
            QianGeConfigs configs,
            TracService tracService,
            DingTalkService dingTalkService
            )
        {
            _configs = configs;
            _tracService = tracService;
            _dingTalkService = dingTalkService;
        }

        public void SendNotCompletedInDevDeadlineTicketsMessage()
        {
            var currentDate = DateTime.Now.Date;

            var peoples = _configs.Peoples.ToDictionary(p => p.Username);

            var unfinishedDevTickets = GetNotCompletedInDevDeadlineTickets(currentDate);

            var atList = new List<string>();

            var message = new StringBuilder();
            message.AppendLine("# 超时未完成 Ticket 通知");

            foreach (var unfinishedDevTicketsGroup in unfinishedDevTickets.GroupBy(t => t.Owner))
            {
                var tracUsername = unfinishedDevTicketsGroup.Key;
                var atPeople = tracUsername;

                if (peoples.ContainsKey(tracUsername))
                {
                    var people = peoples[tracUsername];

                    atList.Add(people.Mobile);

                    atPeople = $"@{people.Mobile}";
                }

                message.AppendLine($"## {atPeople} ");

                foreach (var unfinishedDevTicket in unfinishedDevTicketsGroup)
                {
                    var ticketLink = $"[#{unfinishedDevTicket.TicketId} {unfinishedDevTicket.Summary}]({_configs.Trac.Url}/ticket/{unfinishedDevTicket.TicketId})";
                    var dateContent = $"原定于 {unfinishedDevTicket.Deadline4Dev.Value.ToLongDateString()} 完成，现已超时 {(currentDate - unfinishedDevTicket.Deadline4Dev.Value).Days} 天";

                    message.AppendLine($"- {ticketLink} {dateContent}");
                }

                message.AppendLine();
            }

            message.AppendLine("请及时更新 Ticket 状态或说明原因，谢谢");

            var messageString = message.ToString();

            _dingTalkService.SendMarkdown("超时未完成 Ticket 通知", messageString, atList);
        }

        public void SendTodayShouldCompletedTicketsMessage()
        {
            var currentDate = DateTime.Now.Date;

            var peoples = _configs.Peoples.ToDictionary(p => p.Username);

            var todayShouldCompletedTickets = GetTodayShouldCompletedTickets(currentDate);

            var atList = new List<string>();

            var message = new StringBuilder();
            message.AppendLine("# 今天需要完成的 Ticket 通知");

            foreach (var todayShouldCompletedTicketsGroup in todayShouldCompletedTickets.GroupBy(t => t.Owner))
            {
                var tracUsername = todayShouldCompletedTicketsGroup.Key;
                var atPeople = tracUsername;

                if (peoples.ContainsKey(tracUsername))
                {
                    var people = peoples[tracUsername];

                    atList.Add(people.Mobile);

                    atPeople = $"@{people.Mobile}";
                }

                message.AppendLine($"## {atPeople} ");

                foreach (var unfinishedDevTicket in todayShouldCompletedTicketsGroup)
                {
                    var ticketLink = $"[#{unfinishedDevTicket.TicketId} {unfinishedDevTicket.Summary}]({_configs.Trac.Url}/ticket/{unfinishedDevTicket.TicketId})";

                    message.AppendLine($"- {ticketLink}  ");
                }

                message.AppendLine();
            }

            message.AppendLine("以上 Ticket 计划于今日完成，但截至目前还未修改为测试状态。请及时更新 Ticket 状态或调整排期，谢谢");

            var messageString = message.ToString();

            _dingTalkService.SendMarkdown("今天需要完成的 Ticket 通知", messageString, atList);
        }

        public void SendCompletionOfMonthMessage(DateTime? date = null)
        {
            var queryDate = DateTime.Now.Date;
            if (date.HasValue)
            {
                queryDate = date.Value.Date;
            }

            var tickets = _tracService.GetMonthlyTickets(queryDate.Year, queryDate.Month);

            float ticketCount = tickets.Count;
            var ticketGroupByType = tickets.GroupBy(t => t.Type);
            var ticketGroupByOwner = tickets.GroupBy(t => t.Owner);
            var devOkTickets = tickets.Where(t => TracConst.DevOkStatus.Contains(t.Status));
            var mostType = ticketGroupByType.FirstOrDefault(g => g.Count() == ticketGroupByType.Max(gm => gm.Count()));
            var mostDoingTickets = ticketGroupByOwner.FirstOrDefault(g => g.Count() == ticketGroupByOwner.Max(tm => tm.Count()));
            
            var message = new StringBuilder();
            message.AppendLine($"截至{queryDate.ToLongDateString()}，{queryDate.Month}月共有创建 Ticket {ticketCount} 个");
            message.Append($"{mostType.Key} 类型的最多");
            message.AppendLine($" （{String.Join("、", ticketGroupByType.Select(g => $"{g.Key} {g.Count()} 个"))}）");
            message.AppendLine("------------");
            message.AppendLine($"目前已经提交测试或关闭 {devOkTickets.Count()} 个，占 {(devOkTickets.Count() / ticketCount).ToString("P")}");
            message.AppendLine($"棒棒哒，我们都是这条街最棒的程序员");
            message.AppendLine("------------");
            message.AppendLine($"光 {mostDoingTickets.Key} 一人就完成了 {mostDoingTickets.Count()} 个 Ticket，真是厉害");
            message.AppendLine($"（Ticket 完成数量 TOP 3：{String.Join("、", ticketGroupByOwner.OrderByDescending(g => g.Count()).Take(3).Select(g => $"{g.Key} {g.Count()} 个"))}）");

            _dingTalkService.SendText(message.ToString());
        }


        private List<TracQueryResultItem> GetNotCompletedInDevDeadlineTickets(DateTime currentDate)
        {
            var tickets = _tracService.GetMonthlyTickets(currentDate.Year, currentDate.Month);

            var unfinishedDevTickets = tickets.Where(t => t.Confirmed == TracConst.CONFIRMED_TO_MODIFY)
                        .Where(t => TracConst.DevOkStatus.Contains(t.Status) == false)
                        .Where(t => t.Deadline4Dev < currentDate)
                        .ToList();

            return unfinishedDevTickets;
        }

        private List<TracQueryResultItem> GetTodayShouldCompletedTickets(DateTime currentDate)
        {
            var tickets = _tracService.GetMonthlyTickets(currentDate.Year, currentDate.Month);

            var todayShouldCompletedTickets = tickets.Where(t => t.Confirmed == TracConst.CONFIRMED_TO_MODIFY)
                        .Where(t => TracConst.DevOkStatus.Contains(t.Status) == false)
                        .Where(t => t.Deadline4Dev.HasValue)
                        .Where(t => (t.Deadline4Dev.Value - currentDate).Days == 0)
                        .ToList();

            return todayShouldCompletedTickets;
        }

    }
}
