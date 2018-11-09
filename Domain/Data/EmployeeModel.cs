using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data
{
    public sealed class EmployeeModel
    {
        private readonly EmployeeEntity _entity;
        public EmployeeEntity Entity => _entity;

        public EmployeeModel(EmployeeEntity entity)
        {
            _entity = entity;
        }

        public int Id => _entity.Id;

        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; }
        }

        public string Surname
        {
            get { return _entity.Surname; }
            set { _entity.Surname = value; }
        }

        public string Patronymic
        {
            get { return _entity.Patronymic; }
            set { _entity.Patronymic = value; }
        }

        public string Protocol
        {
            get { return _entity.Protocol; }
            set { _entity.Protocol = value; }
        }

        public DateTime? СompletingСourse
        {
            get { return _entity.СompletingСourse; }
            set { _entity.СompletingСourse = value; }
        }

        public DateTime? NextCourse
        {
            get { return _entity.NextCourse; }
            set { _entity.NextCourse = value; }
        }

        public string Meta
        {
            get { return _entity.Meta; }
            set { _entity.Meta = value; }
        }

        public int DepartmentId
        {
            get { return _entity.DepartmentId; }
            set { _entity.DepartmentId = value; }
        }

        public string Office
        {
            get { return _entity.Office; }
            set { _entity.Office = value; }
        }

    }
}
