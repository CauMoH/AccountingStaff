using DataAccess.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal sealed class DbInitializer : MigrateDatabaseToLatestVersion<AccountingStaffContext, Configuration>
    {

    }
}
