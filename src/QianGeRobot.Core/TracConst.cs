using System;
using System.Collections.Generic;
using System.Text;

namespace QianGeRobot.Core
{
    public static class TracConst
    {
        public const string CONFIRMED_DO_NOT_MODIFY = "0-不做修改";
        public const string CONFIRMED_TO_MODIFY = "1-确认修改";
        public const string CONFIRMED_NOT_CONFIRMED = "2-未确认";

        public static string[] DevOkStatus { get; } = new string[] { "closed", "testing" };

    }
}
