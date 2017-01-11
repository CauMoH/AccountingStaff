using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using AccountingStaff.AppCommon;

namespace AccountingStaff.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            AssemblyVersionText.Text = AppInfo.Version.ToString();
        }

        private void AboutWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
            Close();
        }
    }
}
