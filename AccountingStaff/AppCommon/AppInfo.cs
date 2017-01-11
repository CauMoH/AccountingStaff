using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.AppCommon
{
    internal static class AppInfo
    {
        public const string AppName = "AccountingStaff";
        public const string AppMutexName = "AccountingStaff{109492CB-4930-451F-B83A-D1E2AE65030A}";

        public static Version Version => Assembly.GetEntryAssembly().GetName().Version;
    }
}
