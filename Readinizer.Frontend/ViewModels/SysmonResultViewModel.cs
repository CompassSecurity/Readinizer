using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class SysmonResultViewModel : ViewModelBase, ISysmonResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(Back));

        private List<string> sysmonActiveList { get; set; }
        public List<string> SysmonActiveList => sysmonActiveList;

        private List<string> sysmonNotActiveList { get; set; }
        public List<string> SysmonNotActiveList => sysmonNotActiveList;

        public List<Computer> Computers { get; set; }

        public void loadComputers()
        {
            Computers = unitOfWork.ComputerRepository.GetAllEntities().Result;
            fillLists();
        }

        public List<KeyValuePair<string, int>> PieChartData => LoadPieChartData();

        [Obsolete("Only for design data", true)]
        public SysmonResultViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public SysmonResultViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork; 
        }

        private void fillLists()
        {
            var bad = new List<string>();
            var good = new List<string>();
            foreach (var computer in Computers)
            {
                if (computer.isSysmonRunning.Equals(true))
                {
                    good.Add(computer.ComputerName + "." + computer.OrganizationalUnits.FirstOrDefault().ADDomain.Name);
                    
                }
                else if (computer.isSysmonRunning.Equals(false))
                {
                    bad.Add(computer.ComputerName + "." + computer.OrganizationalUnits.FirstOrDefault().ADDomain.Name);
                }
            }

            sysmonActiveList = good;
            sysmonNotActiveList = bad;
        }

        private List<KeyValuePair<string, int>> LoadPieChartData()
        {
            var runningCounter = SysmonActiveList.Count;
            var notRunningCounter = sysmonNotActiveList.Count;

            var valueList = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Sysmon is running", runningCounter),
                new KeyValuePair<string, int>("Sysmon is not running", notRunningCounter)
            };

            return valueList;
        }

        private static void ShowTreeStructure()
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel)));
        }

        private static void Back()
        {
            ShowTreeStructure();
        }
    }
}

