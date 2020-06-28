using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class SpinnerViewModel : ViewModelBase, ISpinnerViewModel
    {
        private readonly IADDomainService adDomainService;

        private string progressText;
        public string ProgressText
        {
            get => progressText;
            set => Set(ref progressText, value);
        }

        [Obsolete("Only for design data", true)]
        public SpinnerViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public SpinnerViewModel(IADDomainService adDomainService)
        {
            this.adDomainService = adDomainService;
            Messenger.Default.Register<ChangeProgressText>(this, ChangeProgressText);
        }

        public void ChangeProgressText(ChangeProgressText changeProgressText)
        {
            ProgressText = changeProgressText.ProgressText;
            RaisePropertyChanged(nameof(ProgressText));
        }
    }
}
