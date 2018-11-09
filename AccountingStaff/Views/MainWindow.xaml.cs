using System;
using System.Windows;
using System.Windows.Controls;
using AccountingStaff.ViewModels;

namespace AccountingStaff.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewmodel = (MainViewModel) e.NewValue;
            viewmodel.EmployeeCreated += Viewmodel_EmployeeCreated;
        }

        private void Viewmodel_EmployeeCreated(object sender, System.EventArgs e)
        {
            DepartmentsComboBox.SelectedIndex = -1;
        }

        public MainViewModel ViewModel => (MainViewModel)DataContext;
        
        public void Open()
        {
            Activate();
        }

        /// <summary>
        /// Выход из приложения 
        /// </summary>
        public void ExitFromApp()
        {
            App.Exit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoad();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ExitFromApp();
        }

        private void OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void TempEmployeeDepartment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DepartmentViewModel department = null;
            foreach (var addedItem in e.AddedItems)
            {
                department = (DepartmentViewModel) addedItem;
                break;
            }
            if (department != null)
            {
                ViewModel.TempEmployee.DepartmentId = department.Id;
            }
        }
    }
}
