using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Data;
using System.Collections.ObjectModel;
using DataAccess.Interfaces;
using Common;
using DataAccess.Entities;

namespace Domain.Services
{
    internal sealed class EmployeesModelService : IEmployeesModelService
    {
        private readonly IEmployeesRepository _employessRepository;

        public EmployeesModelService(IEmployeesRepository employeesRepository)
        {
            _employessRepository = employeesRepository;

            _employess.OnItemAdd += item => EmployeeAdded?.Invoke(this, new EmployeeModelCollectionChangedEventArgs(item));
            _employess.OnItemRemove += item => EmployeeRemoved?.Invoke(this, new EmployeeModelCollectionChangedEventArgs(item));

            Employees = new ReadOnlyCollection<EmployeeModel>(_employess);
        }

        private readonly BaseDataCollection<EmployeeModel> _employess = new BaseDataCollection<EmployeeModel>();

        public ReadOnlyCollection<EmployeeModel> Employees { get; }

        public event EventHandler EmployeesLoaded;
        public event EventHandler<IEmployeeModelCollectionChangedEventArgs> EmployeeAdded;
        public event EventHandler<IEmployeeModelCollectionChangedEventArgs> EmployeeRemoved;

        public void Load()
        {
            LoadEmployess();
        }

        private void LoadEmployess()
        {
            var entities = _employessRepository.LoadEmployees();

            _employess.SetRange(entities.Select(c => new EmployeeModel(c)));

            EmployeesLoaded?.Invoke(this, EventArgs.Empty);
        }

        void IEmployeesModelService.SaveEmployee(EmployeeEntity entity)
        {
            if (entity.Id == 0)
                _employess.Add(new EmployeeModel(entity));

            _employessRepository.SaveEmployee(entity);           
        }

        public void DeleteEmployees(IEnumerable<EmployeeModel> employees)
        {
            var empIds = new List<int>();
            foreach(var emp in employees)
            {
                empIds.Add(emp.Id);
            }
            _employessRepository.DeleteEmployees(empIds);
            foreach(var emp in employees)
            {
                _employess.Remove(emp);
            }
        }
    }

    internal sealed class EmployeeModelCollectionChangedEventArgs : IEmployeeModelCollectionChangedEventArgs
    {
        public EmployeeModelCollectionChangedEventArgs(EmployeeModel employee)
        {
            Employee = employee;
        }

        public EmployeeModel Employee { get; }
    }
}
