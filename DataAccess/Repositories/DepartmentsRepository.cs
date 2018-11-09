using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    internal sealed class DepartmentsRepository:DbRepository, IDepartmentsRepository
    {
        public void SaveDepartments(IEnumerable<DepartmentEntity> departments)
        {
            SaveEntities(departments);
        }

        public void SaveDepartment(DepartmentEntity department)
        {
            SaveEntity(department);
        }

        public List<DepartmentEntity> LoadDepartments()
        {
            return LoadWrapped(context => context.Departments.ToList());
        }

        public void DeleteDepartment(int departmentId)
        {
            SaveWrapped(context =>
            {
                context.DeleteEntity(new DepartmentEntity() { Id = departmentId });
            });
        }
    }
}
