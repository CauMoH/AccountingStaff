using AccountingStaff.AppStartup;
using Common;
using Common.Logging;
using Domain.Data;
using Domain.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AccountingStaff.Views;
using DataAccess.Entities;
using UiCommon;

namespace AccountingStaff.ViewModels
{
    public class MainViewModel: BindableBase
    {
        private readonly IEmployeesModelService _emplModelService;
        private readonly IDepartmentModelService _depModelService;

        private readonly Timer _timer = new Timer();
        private const int IntervalUpdated = 60000;

        private TempEmployee _tempEmployee = new TempEmployee();
        public TempEmployee TempEmployee
        {
            get { return _tempEmployee; }
            set { SetProperty(ref _tempEmployee, value); }
        }

        private TempDepartment _tempDepartment = new TempDepartment();
        public TempDepartment TempDepartment
        {
            get { return _tempDepartment;}
            set { SetProperty(ref _tempDepartment, value); }
        }

        private int _overdueCourses = 0;
        public int OverdueCourses
        {
            get { return _overdueCourses; }
            set { SetProperty(ref _overdueCourses, value); }
        }

        public BaseDataCollection<EmployeeViewModel> Employees { get; } = new BaseDataCollection<EmployeeViewModel>();
        public BaseDataCollection<DepartmentViewModel> Departments { get; } = new BaseDataCollection<DepartmentViewModel>();

        private DepartmentViewModel _openedDepartment = null;
        public DepartmentViewModel OpenedDepartment
        {
            get { return _openedDepartment;}
            set { SetProperty(ref _openedDepartment, value); }
        }

        private DateTime _startingDate = DateTime.Now;

        public MainViewModel(IEmployeesModelService employeesModelService, IDepartmentModelService departmentModelService)
        {
            _emplModelService = employeesModelService;
            Employees.CollectionChanged += _employees_CollectionChanged;
            _emplModelService.EmployeesLoaded += _emplModelService_EmployeesLoaded;
            _emplModelService.EmployeeAdded += _emplModelService_EmployeeAdded;
            _emplModelService.EmployeeRemoved += _emplModelService_EmployeeRemoved;

            _depModelService = departmentModelService;
            Departments.CollectionChanged += DepartmentsOnCollectionChanged;
            _depModelService.DepartmentsLoaded += _depModelService_DepartmentsLoaded;
            _depModelService.DepartmentsAdded += _depModelService_DepartmentsAdded;
            _depModelService.DepartmentsRemoved += _depModelService_DepartmentsRemoved;
       
            InitCommands();
        }

        #region DepartmentsService Handlers

        private void _depModelService_DepartmentsRemoved(object sender, IDepartmentModelCollectionChangedEventArgs e)
        {
            var findDepartment = Departments.FirstOrDefault(em => em.Id == e.Department.Id);
            if (findDepartment != null)
            {
                Departments.Remove(findDepartment);
            }
        }

        private void _depModelService_DepartmentsAdded(object sender, IDepartmentModelCollectionChangedEventArgs e)
        {
            Departments.Add(new DepartmentViewModel(e.Department));
        }

        private void _depModelService_DepartmentsLoaded(object sender, EventArgs e)
        {
            
        }

        private void DepartmentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DepartmentViewModel dep in e.NewItems)
                {
                    dep.PropertyChanged += Dep_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (DepartmentViewModel dep in e.OldItems)
                {
                    dep.PropertyChanged -= Dep_PropertyChanged;
                }
            }
        }

        private void Dep_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var department = (DepartmentViewModel)sender;
            _depModelService.SaveDepartment(department.Data.Entity);
        }

        #endregion
    
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
            if (OpenedDepartment!= null && e.Employee.DepartmentId != OpenedDepartment.Id)
                return;

            Employees.Add(new EmployeeViewModel(e.Employee) {Department = Departments.FirstOrDefault(dep => dep.Id == e.Employee.DepartmentId)});
        }

        private void _emplModelService_EmployeesLoaded(object sender, EventArgs e)
        {
            OverdueCourses = Employees.Count(c => c.IsExpired);

            if (_timer.Enabled)
            {               
                return;
            }

            _timer.Interval = IntervalUpdated;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _employees_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (EmployeeViewModel emp in e.NewItems)
                {
                    emp.PropertyChanged += Emp_PropertyChanged;
                }
            }
            if (e.OldItems != null)
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

        #endregion

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OverdueCourses = Employees.Count(c => c.IsExpired);

            if (_startingDate.Date != DateTime.Now)
            {
                foreach (var employee in Employees)
                {
                    employee.ForceExpiredUpdated();
                }
                _startingDate = DateTime.Now;
            }
        }

        public void OnLoad()
        {
            var sw = new Stopwatch();
            sw.Start();

            _depModelService.Load();
            _emplModelService.Load();

            sw.Stop();
        }

        public void OnUnload()
        {
            //TODO
        }

        private void InitCommands()
        {
            CreateNewEmployeeCommand = new RelayCommand(CreateNewEmployeeExecute);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployeeExecute);
            OpenAboutWindowCommand = new RelayCommand(OpenAboutWindowExecute);
            CreateNewDepartmentCommand = new RelayCommand(CreateNewDepartmentExecute);
            ChangeDepartmentCommand = new RelayCommand(ChangeDepartmentExecute);
            RemoveDepartmentCommand = new RelayCommand(RemoveDepartmentExecute);
            ExportToExcelCommand = new RelayCommand(ExportToExcelExecute, CanExecuteExportToExcel);
        }      

        #region Commands

        public ICommand CreateNewEmployeeCommand { get; private set; }
        public ICommand RemoveEmployeeCommand { get; private set; }
        public ICommand OpenAboutWindowCommand { get; private set; }
        public ICommand CreateNewDepartmentCommand { get; private set; }
        public ICommand ChangeDepartmentCommand { get; private set; }
        public ICommand RemoveDepartmentCommand { get; private set; }
        public ICommand ExportToExcelCommand { get; private set; }

        private void CreateNewEmployeeExecute(object obj)
        {
            Logging.LoggerFacade.WriteInformation("Added new employee");
            _emplModelService.SaveEmployee(new EmployeeEntity
            {
                Meta = _tempEmployee.Meta,
                Name = _tempEmployee.Name,
                NextCourse = _tempEmployee.NextCourse,
                Patronymic = _tempEmployee.Patronymic,
                Protocol = _tempEmployee.Protocol,
                Surname = _tempEmployee.Surname,
                СompletingСourse = _tempEmployee.СompletingСourse,
                Office = _tempEmployee.Office,
                DepartmentId = _tempEmployee.DepartmentId
            });
            TempEmployee = new TempEmployee();
            EmployeeCreated?.Invoke(this, EventArgs.Empty);
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

        private void CreateNewDepartmentExecute(object obj)
        {
            Logging.LoggerFacade.WriteInformation("Added new department");
            _depModelService.SaveDepartment(new DepartmentEntity()
            {
                Name = _tempDepartment.Name
            });
            TempDepartment = new TempDepartment();
        }

        private void ChangeDepartmentExecute(object obj)
        {
            if (obj is DepartmentViewModel)
            {
                var department = (DepartmentViewModel)obj;
                OpenedDepartment = department;

                _emplModelService.LoadEmployees(OpenedDepartment.Id);
            }
            else
            {
                OpenedDepartment = null;
                _emplModelService.Load();
            }
        }

        private void RemoveDepartmentExecute(object obj)
        {              
            var result = MessageBox.Show(Localization.strings.RemoveEmployees, Localization.strings.Warning,
                MessageBoxButton.YesNoCancel);

            var department = (DepartmentViewModel)obj;

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Logging.LoggerFacade.WriteInformation("Removed department");
                    _depModelService.DeleteDepartment(department.Data);
                    _emplModelService.DeleteEmployees(department.Id);

                    if (OpenedDepartment == null ||
                        OpenedDepartment.Id == department.Id)
                        ChangeDepartmentCommand.Execute(null);

                    break;

                case MessageBoxResult.No:
                    Logging.LoggerFacade.WriteInformation("Removed department");
                    _depModelService.DeleteDepartment(department.Data);

                    if (OpenedDepartment == null ||
                        OpenedDepartment.Id == department.Id)
                        ChangeDepartmentCommand.Execute(null);
                    break;

                case MessageBoxResult.Cancel:
                    break;

                default:
                    break;
            }

        }

        private void ExportToExcelExecute(object obj)
        {
            ExportToExcel();
        }

        private bool CanExecuteExportToExcel(object o)
        {
            return _exportIsCompleted;
        }
        #endregion

        #region Events

        public event EventHandler EmployeeCreated;

        #endregion

        #region Export to Excel

        private bool _exportIsCompleted = true;
        private void ExportToExcel()
        {
            Task.Run(() =>
            {
                _exportIsCompleted = false;
                try
                {
                    //Create table
                    Microsoft.Office.Interop.Excel.Application xlexcel;
                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;
                    xlexcel = new Microsoft.Office.Interop.Excel.Application {Visible = true};
                    xlWorkBook = xlexcel.Workbooks.Add(misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet) xlWorkBook.Worksheets.Item[1];
                    xlWorkSheet.Columns.AutoFit();

                    //Create columns
                    xlWorkSheet.Cells[1, 1] = Localization.strings.Surname;
                    xlWorkSheet.Cells[1, 2] = Localization.strings.Name;
                    xlWorkSheet.Cells[1, 3] = Localization.strings.Patronymic;
                    xlWorkSheet.Cells[1, 4] = Localization.strings.Protocol;
                    xlWorkSheet.Cells[1, 5] = Localization.strings.Office;
                    xlWorkSheet.Cells[1, 6] = Localization.strings.DepartmentName;
                    xlWorkSheet.Cells[1, 7] = Localization.strings.СompletingСourse;
                    xlWorkSheet.Cells[1, 8] = Localization.strings.NextCourse;
                    xlWorkSheet.Cells[1, 9] = Localization.strings.Meta;

                    //writing data
                    for (int index = 0; index < Employees.Count; index++)
                    {
                        var employee = Employees[index];
                        var row = index + 2;

                        xlWorkSheet.Cells[row, 1] = employee.Surname;
                        xlWorkSheet.Cells[row, 2] = employee.Name;
                        xlWorkSheet.Cells[row, 3] = employee.Patronymic;
                        xlWorkSheet.Cells[row, 4] = employee.Protocol;
                        xlWorkSheet.Cells[row, 5] = employee.Office;
                        xlWorkSheet.Cells[row, 6] = employee.Department.Name;
                        xlWorkSheet.Cells[row, 7] = employee.СompletingСourse.ToString();
                        xlWorkSheet.Cells[row, 8] = employee.NextCourse.ToString();
                        xlWorkSheet.Cells[row, 9] = employee.Meta;
                    }
                }
                catch (Exception ex)
                {
                    Logging.LoggerFacade.WriteError("Error export to excel: " + ex);
                }
                finally
                {
                    _exportIsCompleted = true;
                }
            });
        }
       

        #endregion
    }
}
