using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    internal sealed class EmployeesRepository : DbRepository, IEmployeesRepository
    {      
        public List<EmployeeEntity> LoadEmployees(int departmentId)
        {
            return LoadWrapped(context =>
            {
                return context.Employees
                     .Where(m => m.DepartmentId == departmentId)
                     .OrderBy(m => m.NextCourse)
                     .ToList();
            });
        }

        public List<EmployeeEntity> LoadEmployees()
        {
            return LoadWrapped(context =>
            {
                return context.Employees
                     .OrderBy(m => m.NextCourse)
                     .ToList();
            });
        }

        public void DeleteEmployees(IEnumerable<int> employessIds)
        {
            SaveWrapped(context =>
            {
                foreach (var id in employessIds)
                {
                    if (id == 0)
                        continue;
                    context.DeleteEntity(new EmployeeEntity { Id = id });
                }
            });
        }

        public void DeleteEmployees(int departmentId)
        {
            SaveWrapped(context =>
            {
                var employees = context.Employees.Where(entry => entry.DepartmentId == departmentId);
                context.Employees.RemoveRange(employees);
            });
        }

        public void SaveEmployee(EmployeeEntity employee)
        {
            SaveEntity(employee);
        }

        public void SaveEmployees(IEnumerable<EmployeeEntity> employees)
        {
            SaveEntities(employees);
        }

    }
}
