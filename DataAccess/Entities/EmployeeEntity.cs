using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class EmployeeEntity : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? СompletingСourse { get; set; }
        public DateTime? NextCourse { get; set; }
        public string Protocol { get; set; }
        public string Meta { get; set; }
        public string Office { get; set; }

        public int DepartmentId { get; set; }

        #region Configuration

        public sealed class Configuration : EntityTypeConfiguration<EmployeeEntity>
        {
            public Configuration()
            {
                ToTable("Employee");
            }
        }


        #endregion

    }
}
