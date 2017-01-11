using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.ViewModels
{
    public class TempEmployee : BindableBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _surname = string.Empty;
        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        private string _patronymic = string.Empty;
        public string Patronymic
        {
            get { return _patronymic; }
            set { SetProperty(ref _patronymic, value); }
        }

        private DateTime? _completingСourse = DateTime.Now;
        public DateTime? СompletingСourse
        {
            get { return _completingСourse; }
            set { SetProperty(ref _completingСourse, value); }
        }

        private DateTime? _nextCourse = DateTime.Now;
        public DateTime? NextCourse
        {
            get { return _nextCourse; }
            set { SetProperty(ref _nextCourse, value); }
        }

        private string _protocol = string.Empty;
        public string Protocol
        {
            get { return _protocol; }
            set { SetProperty(ref _protocol, value); }
        }

        private string _meta = string.Empty;
        public string Meta
        {
            get { return _meta; }
            set { SetProperty(ref _meta, value); }
        }
    }
}
