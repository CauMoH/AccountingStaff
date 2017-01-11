using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IEmployeesRepository
    {
        void SaveEmployees(IEnumerable<EmployeeEntity> employees);
        void SaveEmployee(EmployeeEntity employee);
        void DeleteEmployees(IEnumerable<int> employessIds);
        List<EmployeeEntity> LoadEmployees();
    }
}
