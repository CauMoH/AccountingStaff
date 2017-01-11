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
    }
}
