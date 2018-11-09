using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class DepartmentEntity : EntityBase
    {
        public string Name { get; set; }

        #region Configuration

        public sealed class Configuration : EntityTypeConfiguration<DepartmentEntity>
        {
            public Configuration()
            {
                ToTable("Department");
            }
        }


        #endregion
    }
}
