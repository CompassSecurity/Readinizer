using System;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;
using SnackbarMessage = Readinizer.Frontend.Messages.SnackbarMessage;

namespace Readinizer.Frontend.ViewModels
{
    public class ApplicationViewModel : ViewModelBase, IApplicationViewModel
    {
        private readonly StartUpViewModel startUpViewModel;
        private readonly TreeStructureResultViewModel treeStructureResultViewModel;
        private readonly SpinnerViewModel spinnerViewModel;
        private readonly DomainResultViewModel domainResultViewModel;
        private readonly RSoPResultViewModel rsopResultViewModel;
        private readonly OUResultViewModel ouResultViewModel;
        private readonly SysmonResultViewModel sysmonResultViewModel;
        private readonly IDialogService dialogService;
        private readonly IExportService exportService;

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(OnClose));

        private ICommand userManualCommand;
        public ICommand UserManualCommand => userManualCommand ?? (userManualCommand = new RelayCommand(OnUserManual));

        private ICommand centralLoggingCommand;
        public ICommand CentralLoggingCommand => centralLoggingCommand ?? (centralLoggingCommand = new RelayCommand(OnCentralLogging));

        private ICommand sysmonCommand;
        public ICommand SysmonCommand => sysmonCommand ?? (sysmonCommand = new RelayCommand(OnSysmon));

        private ICommand optimizedGPOCommand;
        public ICommand OptimizedGPOCommand => optimizedGPOCommand ?? (optimizedGPOCommand = new RelayCommand(OnOptimizedGPO));

        private ICommand exportRSoPPotsCommand;
        public ICommand ExportRSoPPotsCommand => exportRSoPPotsCommand ?? (exportRSoPPotsCommand = new RelayCommand(() => Export(typeof(RsopPot))));

        private ICommand exportRSoPsCommand;
        public ICommand ExportRSoPsCommand => exportRSoPsCommand ?? (exportRSoPsCommand = new RelayCommand(() => Export(typeof(Rsop))));

        private ICommand newAnalysisCommand;
        public ICommand NewAnalysisCommand => newAnalysisCommand ?? (newAnalysisCommand = new RelayCommand(OnNewAnalysis));

        private readonly IUnitOfWork unitOfWork;

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set => Set(ref currentViewModel, value);
        }

        private bool canExport;
        public bool CanExport
        {
            get => canExport;
            set => Set(ref canExport, value);
        }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        public readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight * 0.8;

        [Obsolete("Only for design data", true)]
        public ApplicationViewModel() : this(new StartUpViewModel(), null, null, null, null, null, null, null, null, null, null)
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design data");
            }
        }

        public ApplicationViewModel(StartUpViewModel startUpViewModel, TreeStructureResultViewModel treeStructureResultViewModel, 
                                    SpinnerViewModel spinnerViewModel, DomainResultViewModel domainResultViewModel, 
                                    RSoPResultViewModel rsopResultViewModel, OUResultViewModel ouResultViewModel,
                                    SysmonResultViewModel sysmonResultViewModel, ISnackbarMessageQueue snackbarMessageQueue,
                                    IDialogService dialogService, IExportService exportService, IUnitOfWork unitOfWork)
        {
            this.startUpViewModel = startUpViewModel;
            this.treeStructureResultViewModel = treeStructureResultViewModel;
            this.spinnerViewModel = spinnerViewModel;
            this.domainResultViewModel = domainResultViewModel;
            this.rsopResultViewModel = rsopResultViewModel;
            this.ouResultViewModel = ouResultViewModel;
            this.sysmonResultViewModel = sysmonResultViewModel;
            this.dialogService = dialogService;
            this.exportService = exportService;
            this.unitOfWork = unitOfWork;

            SnackbarMessageQueue = snackbarMessageQueue;

            var computers = unitOfWork.ComputerRepository.GetAllEntities().Result;
            var i = computers.Find(x => x.isSysmonRunning.HasValue) != null;
            if (computers.Count != 0)
            {
                if (computers.Find(x => x.isSysmonRunning.HasValue) != null)
                {
                    ShowTreeStructureResultView("Visible");
                }
                else
                {
                    ShowTreeStructureResultView("Hidden");
                }
            }
            else
            {
                ShowStartUpView();
            }

            Messenger.Default.Register<ChangeView>(this, ChangeView);
            Messenger.Default.Register<SnackbarMessage>(this, OnShowMessage);
            Messenger.Default.Register<EnableExport>(this, EnableExport);
        }

        private void ShowStartUpView()
        {
            CurrentViewModel = startUpViewModel;
        }

        private void ShowTreeStructureResultView(string visibility)
        {
            CurrentViewModel = treeStructureResultViewModel;
            treeStructureResultViewModel.WithSysmon = visibility;
            treeStructureResultViewModel.BuildTree();
        }

        private void ShowSpinnerView()
        {
            CurrentViewModel = spinnerViewModel;
        }

        private void ShowDomainResultView(int refId)
        {
            CurrentViewModel = domainResultViewModel;
            domainResultViewModel.RefId = refId;
            domainResultViewModel.loadRsopPots();
        }

        private void ShowRsopResultView(int refId)
        {
            CurrentViewModel = rsopResultViewModel;
            rsopResultViewModel.RefId = refId;
            rsopResultViewModel.rsopPot = unitOfWork.RsopPotRepository.GetByID(refId);
            rsopResultViewModel.Load();
        }

        private void ShowOuResultView(int refId)
        {
            CurrentViewModel = ouResultViewModel;
            ouResultViewModel.RefId = refId;
            ouResultViewModel.rsop = unitOfWork.RsopRepository.GetByID(refId);
            ouResultViewModel.Load();
        }

        private void ShowSysmonResultView()
        {
            CurrentViewModel = sysmonResultViewModel;
            sysmonResultViewModel.loadComputers();

        }

        private void ChangeView(ChangeView message)
        {
            if (message.ViewModelType == typeof(StartUpViewModel))
            {
                ShowStartUpView();
            }
            else if (message.ViewModelType == typeof(TreeStructureResultViewModel))
            {
                ShowTreeStructureResultView(message.Visibility);
            }
            else if (message.ViewModelType == typeof(SpinnerViewModel))
            {
                ShowSpinnerView();
            }
            else if(message.ViewModelType == typeof(DomainResultViewModel))
            {
                ShowDomainResultView(message.RefId);
               
            }
            else if(message.ViewModelType == typeof(RSoPResultViewModel))
            {
                ShowRsopResultView(message.RefId);
            }
            else if (message.ViewModelType == typeof(OUResultViewModel))
            {
                ShowOuResultView(message.RefId);
            }
            else if (message.ViewModelType == typeof(SysmonResultViewModel))
            {
                ShowSysmonResultView();
            }
        }

        private void EnableExport(EnableExport message)
        {
            CanExport = message.ExportEnabled;
            RaisePropertyChanged(nameof(CanExport));
        }

        private void OnShowMessage(SnackbarMessage message)
        {
            SnackbarMessageQueue.Enqueue(message.Message);
        }

        private void OnNewAnalysis()
        {
            ClearDb();
            ShowStartUpView();
        }

        private async void Export(Type type)
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "Save all identical audit settings",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "JSON-file (*.json)|*.json|All Files (*.*)|*.*",
                CreatePrompt = false,
                CheckFileExists = false
            };

            bool? success = dialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                var exportPath = settings.FileName;
                if (settings.CheckPathExists)
                {
                    var successfullyExported = await exportService.Export(type, exportPath);
                    if (!successfullyExported)
                    {
                        Messenger.Default.Send(new SnackbarMessage("Something went wrong during saving the file"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new SnackbarMessage($"The specified path '{exportPath}' does not exist"));
                }
            }
        }

        private static void OnClose()
        {
            Application.Current.Shutdown();
        }

        private static void OnUserManual()
        {
            Process.Start("https://github.com/clma91/Readinizer/wiki/User-Manual");
        }

        private static void OnCentralLogging()
        {
            Process.Start("https://github.com/clma91/Readinizer/wiki/Windows-Event-Forwarding-deploying-fleet-wide");
        }

        private static void OnSysmon()
        {
            Process.Start("https://github.com/clma91/Readinizer/wiki/Install-Sysmon-through-GPO");
        }

        private static void OnOptimizedGPO()
        {
            Process.Start("https://github.com/clma91/Readinizer/wiki/Recommended-Group-Policy");
        }
  
        private static void ClearDb()
        {
            var dbContext = new DbContext(ConfigurationManager.ConnectionStrings["ReadinizerDbContext"].ConnectionString);

            dbContext.Database.Connection.Close();

            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.OrganizationalUnitComputer");
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.SiteADDomain");

            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.AuditSetting DBCC CHECKIDENT('READINIZER.dbo.AuditSetting', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.RegistrySetting DBCC CHECKIDENT('READINIZER.dbo.RegistrySetting', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Policy DBCC CHECKIDENT('READINIZER.dbo.Policy', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.SecurityOption DBCC CHECKIDENT('READINIZER.dbo.SecurityOption', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Gpo DBCC CHECKIDENT('READINIZER.dbo.Gpo', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Rsop DBCC CHECKIDENT('READINIZER.dbo.Rsop', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.RsopPot DBCC CHECKIDENT('READINIZER.dbo.RsopPot', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Computer DBCC CHECKIDENT('READINIZER.dbo.Computer', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.OrganizationalUnit DBCC CHECKIDENT('READINIZER.dbo.OrganizationalUnit', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Site DBCC CHECKIDENT('READINIZER.dbo.Site', NORESEED)");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.ADDomain DBCC CHECKIDENT('READINIZER.dbo.ADDomain', NORESEED)");
        }
    }
}
