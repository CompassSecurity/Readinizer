using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class RSoPResultViewModel : ViewModelBase, IRSoPResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISecuritySettingParserService securitySettingParserService;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(Back));

        private List<SecuritySettingsParsed> securitySettings;
        public List<SecuritySettingsParsed> SecuritySettings
        {
            get => securitySettings;
            set => Set(ref securitySettings, value);
        }

        private string rsop;
        public string Rsop
        {
            get => rsop;
            set
            {
                rsop = value;
                var rsopList = rsopPot.Rsops.ToList();
                var rsopID = rsopList.Find(x => x.OrganizationalUnit.Name.Equals(rsop)).RsopId;
                ShowOUView(rsopID);
                rsop = null;
            }
        }

        public RsopPot rsopPot { get; set; }

        public string GISS => rsopPot.Name;

        public int RefId { get; set; }

        public List<string> OUsInGISS => loadOUs();

        public void Load() => LoadSettings();


        [Obsolete("Only for design data", true)]
        public RSoPResultViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public RSoPResultViewModel(IUnitOfWork unitOfWork, ISecuritySettingParserService securitySettingParserService)
        {
            this.unitOfWork = unitOfWork;
            this.securitySettingParserService = securitySettingParserService;
        }

        private async void LoadSettings()
        {
            SecuritySettings = await securitySettingParserService.ParseSecuritySettings(RefId, "RSoPPot");
            RaisePropertyChanged(nameof(SecuritySettings));
        }

        private List<string> loadOUs()
        {
            var rsops = rsopPot.Rsops;
            return rsops.Select(x => x.OrganizationalUnit.Name).ToList();
        }

        private async Task<List<OrganizationalUnit>> GetOusAsync()
        {
            var ous = await unitOfWork.OrganizationalUnitRepository.GetAllEntities();
            return ous;
        }

        private static void ShowOUView(int rsopRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(OUResultViewModel), rsopRefId));
        }

        private static void ShowDomainView(int domainRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(DomainResultViewModel), domainRefId));
        }

        private void Back()
        {
            ShowDomainView(rsopPot.Domain.ADDomainId);
        }
    }
}
 