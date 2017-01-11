using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Migrations
{
    internal sealed class ContextFactory : IDbContextFactory<AccountingStaffContext>
    {
        public AccountingStaffContext Create()
        {
            return new AccountingStaffContext(DbRepository.ConnectionString);
        }
    }
}
