using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IDepartmentsRepository
    {
        void SaveDepartments(IEnumerable<DepartmentEntity> departments);
        void SaveDepartment(DepartmentEntity department);
        List<DepartmentEntity> LoadDepartments();
        void DeleteDepartment(int departmentId);
    }
}
