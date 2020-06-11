using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.AppHelper
{


    internal static class TaskManger
    {
        public static string Running_Process = "";

        internal static class TaskManagerStatus
        {
            public const string Running = "Running";
            public const string Ready = "Ready";
            public const string Completed = "Completed";
            public const string Paused = "Paused";
            public const string Failed = "Failed";
        }


        internal static class TaskMangerProcess
        {
            public const string ScrapBoard = "Scrap Board";
            public const string ScrapSearch = "Scrap Search";
            public const string Posting = "Post Pins";

        }

    }
}
