using Domain.Data;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.ViewModels
{
    public class EmployeeViewModel : BindableBase 
    {
        public EmployeeModel Data { get; } = null;

        public EmployeeViewModel(EmployeeModel data)
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

        public string Surname
        {
            get { return Data.Surname; }
            set
            {
                if(Data.Surname == value)
                {
                    return;
                }
                Data.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Patronymic
        {
            get { return Data.Patronymic; }
            set
            {
                if (Data.Patronymic == value)
                {
                    return;
                }
                Data.Patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public int Id => Data.Id;

        public int DepartmentId => Data.DepartmentId;

        private DepartmentViewModel _department = null;
        public DepartmentViewModel Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }
        
        public DateTime? СompletingСourse
        {
            get { return Data.СompletingСourse; }
            set
            {
                if (Data.СompletingСourse == value)
                {
                    return;
                }
                Data.СompletingСourse = value;
                OnPropertyChanged(nameof(СompletingСourse));
            }
        }

        public DateTime? NextCourse
        {
            get { return Data.NextCourse; }
            set
            {
                if (Data.NextCourse == value)
                {
                    return;
                }
                Data.NextCourse = value;
                OnPropertyChanged(nameof(NextCourse));
                OnPropertyChanged(nameof(IsExpired));
            }
        }

        public bool IsExpired => NextCourse != null && NextCourse.Value.Date < DateTime.Now.Date;

        public string Protocol
        {
            get { return Data.Protocol; }
            set
            {
                if (Data.Protocol == value)
                {
                    return;
                }
                Data.Protocol = value;
                OnPropertyChanged(nameof(Protocol));
            }
        }

        public string Meta
        {
            get { return Data.Meta; }
            set
            {
                if (Data.Meta == value)
                {
                    return;
                }
                Data.Meta = value;
                OnPropertyChanged(nameof(Meta));
            }
        }

        public string Office
        {
            get { return Data.Office; }
            set
            {
                if (Data.Office == value)
                {
                    return;
                }
                Data.Office = value;
                OnPropertyChanged(nameof(Office));
            }
        }

        public void ForceExpiredUpdated()
        {
            OnPropertyChanged(nameof(IsExpired));
        }

    }
}
