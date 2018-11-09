using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Data;
using Prism.Mvvm;

namespace AccountingStaff.ViewModels
{
    public class DepartmentViewModel:BindableBase
    {
        public DepartmentModel Data { get; } = null;

        public DepartmentViewModel(DepartmentModel data)
        {
            Data = data;
        }

        public string Name
        {
            get { return Data.Name; }
            set
            {
                if (Data.Name == value)
                {
                    return;
                }
                Data.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Id => Data.Id;
    }
}
