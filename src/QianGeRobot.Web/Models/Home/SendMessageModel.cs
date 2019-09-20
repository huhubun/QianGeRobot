using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QianGeRobot.Web.Models.Home
{
    public class SendMessageModel
    {
        public string Content { get; set; }

        public bool AtAll { get; set; }

        public string Type { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public IEnumerable<SelectListItem> YearSelectList
        {
            get
            {
                for (var i = 0; i <= 50; i++)
                {
                    var value = (2019 + i).ToString();
                    yield return new SelectListItem(value, value);
                }
            }
        }

        public IEnumerable<SelectListItem> MonthSelectList
        {
            get
            {
                for (var i = 1; i <= 12; i++)
                {
                    var value = i.ToString();
                    yield return new SelectListItem(value, value);
                }
            }
        }
    }
}
