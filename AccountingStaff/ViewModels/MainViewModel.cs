using AccountingStaff.AppStartup;
using Common;
using Common.Logging;
using Domain.Data;
using Domain.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Input;
using AccountingStaff.Views;
using UiCommon;

namespace AccountingStaff.ViewModels
{
    public class MainViewModel: BindableBase
    {
        private readonly IEmployeesModelService _emplModelService;

        private readonly Timer _timer = new Timer();
        private const int IntervalUpdated = 60000;

        private TempEmployee _tempEmployee = new TempEmployee();
        public TempEmployee TempEmployee
        {
            get { return _tempEmployee; }
            set { SetProperty(ref _tempEmployee, value); }
        }

        private int _overdueCourses = 0;
        public int OverdueCourses
        {
            get { return _overdueCourses; }
            set { SetProperty(ref _overdueCourses, value); }
        }

        public BaseDataCollection<EmployeeViewModel> Employees { get; } = new BaseDataCollection<EmployeeViewModel>();

        public MainViewModel(IEmployeesModelService employeesModelService)
        {
            _emplModelService = employeesModelService;
            Employees.CollectionChanged += _employees_CollectionChanged;
            _emplModelService.EmployeesLoaded += _emplModelService_EmployeesLoaded;
            _emplModelService.EmployeeAdded += _emplModelService_EmployeeAdded;
            _emplModelService.EmployeeRemoved += _emplModelService_EmployeeRemoved;
       
            InitCommands();
        }

        private void _employees_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(EmployeeViewModel emp in e.NewItems)
                {
                    emp.PropertyChanged += Emp_PropertyChanged;                   
                }
            }
            if(e.OldItems != null)
            {
                foreach (EmployeeViewModel emp in e.OldItems)
                {
                    emp.PropertyChanged -= Emp_PropertyChanged;                   
                }
            }
        }

        private void Emp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var employee = (EmployeeViewModel)sender;
            _emplModelService.SaveEmployee(employee.Data.Entity);
        }

        #region EmployeesService Handlers

        private void _emplModelService_EmployeeRemoved(object sender, IEmployeeModelCollectionChangedEventArgs e)
        {
            var findEmployee = Employees.FirstOrDefault(em => em.Id == e.Employee.Id);
            if(findEmployee != null)
            {
                Employees.Remove(findEmployee);
            }
        }

        private void _emplModelService_EmployeeAdded(object sender, IEmployeeModelCollectionChangedEventArgs e)
        {
            Employees.Add(new EmployeeViewModel(e.Employee));
        }

        private void _emplModelService_EmployeesLoaded(object sender, EventArgs e)
        {
            OverdueCourses = Employees.Count(c => c.IsExpired);

            _timer.Interval = IntervalUpdated;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        #endregion

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OverdueCourses = Employees.Count(c => c.IsExpired);
        }

        public void OnLoad()
        {
            var sw = new Stopwatch();
            sw.Start();
            _emplModelService.Load();

            sw.Stop();
        }

        public void OnUnload()
        {
            //ToDO
        }

        private void InitCommands()
        {
            CreateNewEmployeeCommand = new RelayCommand(CreateNewEmployeeExecute);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployeeExecute);
            OpenAboutWindowCommand = new RelayCommand(OpenAboutWindowExecute);
        }

        #region Commands

        public ICommand CreateNewEmployeeCommand { get; private set; }
        public ICommand RemoveEmployeeCommand { get; private set; }
        public ICommand OpenAboutWindowCommand { get; private set; }

        private void CreateNewEmployeeExecute(object obj)
        {
            Logging.LoggerFacade.WriteInformation("Added new employee");
            _emplModelService.SaveEmployee(new DataAccess.Entities.EmployeeEntity
            {
                Meta = _tempEmployee.Meta,
                Name = _tempEmployee.Name,
                NextCourse = _tempEmployee.NextCourse,
                Patronymic = _tempEmployee.Patronymic,
                Protocol = _tempEmployee.Protocol,
                Surname = _tempEmployee.Surname,
                СompletingСourse = _tempEmployee.СompletingСourse
            });
            TempEmployee = new TempEmployee();
        }

        private void RemoveEmployeeExecute(object obj)
        {
            Logging.LoggerFacade.WriteInformation("Removed employees");

            var list = obj as IList;
            if (list != null)
            {
                var selectedItemsList = list.Cast<EmployeeViewModel>();
                var models = new List<EmployeeModel>();
                foreach(var emp in selectedItemsList)
                {
                    models.Add(emp.Data);
                }
                _emplModelService.DeleteEmployees(models);
            }
        }

        private AboutWindow _aboutWindow;
        private void OpenAboutWindowExecute(object obj)
        {
            if (_aboutWindow == null)
            {
                _aboutWindow = new AboutWindow { Owner = System.Windows.Application.Current.MainWindow };
            }
            _aboutWindow.Show();
        }
        #endregion
    }
}
