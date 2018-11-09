using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AccountingStaff.ViewModels
{
    public class TempDepartment:BindableBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name;}
            set { SetProperty(ref _name, value); }
        }
        
    }
}
