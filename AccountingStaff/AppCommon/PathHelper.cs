using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.AppCommon
{
    internal static class PathHelper
    {
        public static string AppDataFolderPath
        {
            get
            {
                var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(rootFolder, AppInfo.AppName);
            }
        }
    }
}
