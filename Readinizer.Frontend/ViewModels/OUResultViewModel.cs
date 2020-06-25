using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class OUResultViewModel : ViewModelBase, IOUResultViewModel
    {
        private readonly IUnitOfWork unityOfWork;
        private readonly ISecuritySettingParserService securitySettingParserService;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(Back));

        public int RefId{ get; set; }

        public Rsop rsop { get; set; }

        public string Ou => rsop.OrganizationalUnit.Name;

        private List<SecuritySettingsParsed> securitySettings;
        public List<SecuritySettingsParsed> SecuritySettings
        {
            get => securitySettings;
            set => Set(ref securitySettings, value);
        }

        [Obsolete("Only for design data", true)]
        public OUResultViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public OUResultViewModel(IUnitOfWork unityOfWork, ISecuritySettingParserService securitySettingParserService)
        {
            this.securitySettingParserService = securitySettingParserService;
            this.unityOfWork = unityOfWork;
        }

        public void Load() => LoadSettings();

        private async void LoadSettings()
        {
            SecuritySettings = await securitySettingParserService.ParseSecuritySettings(RefId, "OU");
            RaisePropertyChanged(nameof(SecuritySettings));
        }

        private static void ShowPotView(int potRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potRefId));

        }

        private void Back()
        {
            ShowPotView(rsop.RsopPotRefId.GetValueOrDefault());
        }
    }
}
 