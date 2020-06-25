using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Frontend.Interfaces;

namespace Readinizer.Frontend.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IBaseViewModel
    {

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
