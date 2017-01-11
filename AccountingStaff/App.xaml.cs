using AccountingStaff.AppCommon;
using AccountingStaff.AppStartup;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AccountingStaff
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (!SingleInstance<App>.InitializeAsFirstInstance(AppInfo.AppMutexName))
            {
                return;
            }

            try
            {
                if (args.Any(a =>
                    a.Trim().ToLower() == CommandLineArgs.Exit ||
                    a.Trim().ToLower() == CommandLineArgs.SignOut))
                {
                    // Если приложение не запущено и вызывается с флагом завершения, то и не запускаем его.
                    return;
                }

                var application = new App();

                application.InitializeComponent();
                application.Run();
            }
            finally
            {
                SingleInstance<App>.Cleanup();
            }
        }

        private Bootstrapper _bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var splashScreen = new SplashScreen("/Images/Splash.png");
            splashScreen.Show(true);

            _bootstrapper = new Bootstrapper(e.Args, this);
            _bootstrapper.Run();
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (args == null || args.Count == 0)
            {
                return true;
            }

            _bootstrapper.ProcessCommandLineArgs(args.Skip(1).ToArray());

            return true;
        }

        #region Exiting

        public static bool IsExiting { get; private set; }

        public new static void Exit()
        {
            if (IsExiting)
            {
                return;
            }

            IsExiting = true;

            ((App)Current)._bootstrapper.OnExit();
        }

        #endregion
    }

}
