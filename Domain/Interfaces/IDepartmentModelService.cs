using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Domain.Data;

namespace Domain.Interfaces
{
    public interface IDepartmentModelService
    {
        ReadOnlyCollection<DepartmentModel> Departments { get; }
        void Load();
        void SaveDepartment(DepartmentEntity entity);
        void DeleteDepartment(DepartmentModel department);

        event EventHandler DepartmentsLoaded;
        event EventHandler<IDepartmentModelCollectionChangedEventArgs> DepartmentsAdded;
        event EventHandler<IDepartmentModelCollectionChangedEventArgs> DepartmentsRemoved;
    }

    public interface IDepartmentModelCollectionChangedEventArgs
    {
        DepartmentModel Department { get; }
    }
}
