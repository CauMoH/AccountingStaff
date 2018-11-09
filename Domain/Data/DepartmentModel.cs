using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Domain.Data
{
    public sealed class DepartmentModel
    {
        private readonly DepartmentEntity _entity;
        public DepartmentEntity Entity => _entity;

        public DepartmentModel(DepartmentEntity entity)
        {
            _entity = entity;
        }

        public int Id => _entity.Id;

        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; }
        }
    }
}
