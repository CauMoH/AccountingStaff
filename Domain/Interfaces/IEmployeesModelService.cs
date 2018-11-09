using DataAccess.Entities;
using Domain.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmployeesModelService
    {
        ReadOnlyCollection<EmployeeModel> Employees { get; }
        void Load();
        void LoadEmployees(int departmentId);
        void SaveEmployee(EmployeeEntity entity);
        void DeleteEmployees(IEnumerable<EmployeeModel> employees);
        void DeleteEmployees(int departmentId);

        event EventHandler EmployeesLoaded;
        event EventHandler<IEmployeeModelCollectionChangedEventArgs> EmployeeAdded;
        event EventHandler<IEmployeeModelCollectionChangedEventArgs> EmployeeRemoved;
    }

    public interface IEmployeeModelCollectionChangedEventArgs
    {
        EmployeeModel Employee { get; }
    }
}
