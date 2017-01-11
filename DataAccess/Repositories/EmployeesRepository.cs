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
        public List<EmployeeEntity> LoadEmployees()
        {
            return LoadWrapped(context => context.Employees.ToList());
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
