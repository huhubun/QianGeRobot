using CsvHelper.Configuration.Attributes;
using System;

namespace QianGeRobot.Core.Trac
{
    public class TracQueryResultItem
    {
        /// <summary>
        /// Ticket 号
        /// </summary>
        [Name("id")]
        public int TicketId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Name("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 开发完成时间
        /// </summary>
        [Name("deadline4dev")]
        public DateTime? Deadline4Dev { get; set; }

        /// <summary>
        /// 归属人
        /// </summary>
        [Name("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Ticket 类型
        /// </summary>
        [Name("type")]
        public string Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Name("status")]
        public string Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Name("time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 确认修改
        /// </summary>
        [Name("confirmed")]
        public string Confirmed { get; set; }

        /// <summary>
        /// 整体完成时间
        /// </summary>
        [Name("deadline")]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// 酒店
        /// </summary>
        [Name("hotel")]
        public string Hotel { get; set; }

        /// <summary>
        /// 提出人
        /// </summary>
        [Name("reporter")]
        public string Reporter { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Name("startdate")]
        public DateTime? Startdate { get; set; }

    }
}
