using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Domain.Data;
using Domain.Interfaces;

namespace Domain.Services
{
    internal sealed class DepartmentModelService: IDepartmentModelService
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentModelService(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;

            _departments.OnItemAdd += item => DepartmentsAdded?.Invoke(this, new DepartmentModelCollectionChangedEventArgs(item));
            _departments.OnItemRemove += item => DepartmentsRemoved?.Invoke(this, new DepartmentModelCollectionChangedEventArgs(item));

            Departments = new ReadOnlyCollection<DepartmentModel>(_departments);
        }

        private readonly BaseDataCollection<DepartmentModel> _departments = new BaseDataCollection<DepartmentModel>();
        public ReadOnlyCollection<DepartmentModel> Departments { get; }

        public void Load()
        {
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            var entities = _departmentsRepository.LoadDepartments();

            _departments.SetRange(entities.Select(c => new DepartmentModel(c)));

            DepartmentsLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void SaveDepartment(DepartmentEntity entity)
        {
            if (entity.Id == 0)
                _departments.Add(new DepartmentModel(entity));

            _departmentsRepository.SaveDepartment(entity);
        }

        public void DeleteDepartment(DepartmentModel department)
        {
            _departmentsRepository.DeleteDepartment(department.Id);
            _departments.Remove(department);
        }

        public event EventHandler DepartmentsLoaded;
        public event EventHandler<IDepartmentModelCollectionChangedEventArgs> DepartmentsAdded;
        public event EventHandler<IDepartmentModelCollectionChangedEventArgs> DepartmentsRemoved;
    }

    internal sealed class DepartmentModelCollectionChangedEventArgs : IDepartmentModelCollectionChangedEventArgs
    {
        public DepartmentModelCollectionChangedEventArgs(DepartmentModel department)
        {
            Department = department;
        }

        public DepartmentModel Department { get; }
    }
}
