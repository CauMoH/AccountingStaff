using AccountingStaff.AppCommon;
using AccountingStaff.Logging;
using AccountingStaff.ViewModels;
using Common.Logging;
using DataAccess;
using Domain;
using Domain.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using UiCommon;

namespace AccountingStaff.AppStartup
{
    internal sealed class Bootstrapper : UnityBootstrapper
    {
        private readonly IList<string> _cmdArgs;
        private readonly App _application;

        public Bootstrapper(IList<string> cmdArgs, App application)
        {
            _cmdArgs = cmdArgs;
            _application = application;
        }

        public void ProcessCommandLineArgs(IList<string> args)
        {
            LoggerFacade.WriteInformation(string.Format("Cmd args: {0}", string.Join(" ", args)));

            if (args.Count == 0)
            {
                _mainwindow.Open();
                return;
            }

            var arg = args[0];
            var lowerCaseArg = arg.ToLowerInvariant();

            if (lowerCaseArg == CommandLineArgs.Exit)
            {
                _mainwindow.ExitFromApp();
            }
        }

        #region Overrides

        public override void Run(bool runWithDefaultConfiguration)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            InitializeCurrentDirectory();

            base.Run(runWithDefaultConfiguration);
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            InitializeMainWindow();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());

            RepositoryInitializer.Initialize(Container,
                () => PathHelper.AppDataFolderPath);

            DomainServicesInitializer.Initialize(Container);
        }

        #endregion

        #region Initialize

        private void InitializeCurrentDirectory()
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(directory);
        }

        private readonly AutoResetEvent _initializedEvent = new AutoResetEvent(false);

        private MainViewModel _mainVm;
        private Views.MainWindow _mainwindow;

        private void InitializeMainWindow()
        {
            _mainVm = new MainViewModel(ServiceLocator.Current.GetInstance<IEmployeesModelService>(),
                                        ServiceLocator.Current.GetInstance<IDepartmentModelService>());
            _mainwindow = (Views.MainWindow)Shell;
            _mainwindow.DataContext = _mainVm;

            Application.Current.MainWindow = _mainwindow;
            Application.Current.MainWindow.Show();
            _mainwindow.Open();

            Task.Run(() =>
            {
                _initializedEvent.WaitOne();
                UiInvoker.Invoke(() =>
                {
                    ProcessCommandLineArgs(_cmdArgs);
                });
            });
        }

        #endregion

        public void OnExit()
        {
            _mainVm.OnUnload();
            Task.Run(() =>
            {
                UiInvoker.Invoke(() => _application.Shutdown());
            });
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {

        }
    }
}
