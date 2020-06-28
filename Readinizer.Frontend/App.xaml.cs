using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.ViewModels;
using Readinizer.Frontend.Views;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MvvmDialogs;
using Readinizer.Backend.Business.Factory;
using Readinizer.Backend.DataAccess.Context;
using Readinizer.Backend.DataAccess.UnitOfWork;
using Unity;

namespace Readinizer.Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IApplicationViewModel, ApplicationViewModel>();
            container.RegisterType<ApplicationView>();
            container.RegisterType<IStartUpViewModel, StartUpViewModel>();
            container.RegisterType<ITreeStructureResultViewModel, TreeStructureResultViewModel>();
            container.RegisterType<ISpinnerViewModel, SpinnerViewModel>();
            container.RegisterType<IDomainResultViewModel, DomainResultViewModel>();
            container.RegisterType<IRSoPResultViewModel, RSoPResultViewModel>();
            container.RegisterType<IOUResultViewModel, OUResultViewModel>();

            container.RegisterType<IADDomainService, ADDomainService>();
            container.RegisterType<ISiteService, SiteService>();
            container.RegisterType<IOrganizationalUnitService, OrganizationalUnitService>();
            container.RegisterType<IComputerService, ComputerService>();
            container.RegisterType<IRsopService, RsopService>();
            container.RegisterType<ISysmonService, SysmonService>();
            container.RegisterType<IPingService, PingService>();
            container.RegisterType<IAnalysisService, AnalysisService>();
            container.RegisterType<IRsopPotService, RsopPotService>();
            container.RegisterType<ISysmonResultViewModel, SysmonResultViewModel>();
            container.RegisterType<IExportService, ExportService>();
            container.RegisterType<ISecuritySettingParserService, SecuritySettingParserService>();

            container.RegisterType<ITreeNodesFactory, TreeNodesFactory>();

            container.RegisterSingleton<IReadinizerDbContext, ReadinizerDbContext>();
            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IDialogService, DialogService>();

            container.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));

            var ctx = new DbContext(ConfigurationManager.ConnectionStrings["ReadinizerDbContext"].ConnectionString);
            ctx.Database.CreateIfNotExists();

            var applicationView = container.Resolve<ApplicationView>();
            applicationView.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var friendlyMsg = $"Sorry something went wrong.  The error was: [{e.Exception.Message}]";
            const string caption = "Error";
            MessageBox.Show(friendlyMsg, caption, MessageBoxButton.OK, MessageBoxImage.Error);

            // Signal that we handled things--prevents Application from exiting
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);


        }
    }
}
