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
    public class DomainResultViewModel : ViewModelBase, IDomainResultViewModel
    {
        private ADDomain Domain { get; set; }

        private List<string> goodList { get; set; }
        public List<string> GoodList => goodList;

        private List<string> badList { get; set; }
        public List<string> BadList => badList;

        private readonly IUnitOfWork unitOfWork;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(Back));

        private string potName;
        public string PotName
        {
            get => potName;
            set
            {
                potName = value;
                var rsopPotID = RsopPots.Find(x => x.Name.Equals(potName)).RsopPotId;
                ShowPotView(rsopPotID);
                potName = null;
            }
        }

        public int RefId { get; set; }

        public string DomainName => Domain.Name;

        public List<KeyValuePair<string, int>> PieChartData => LoadPieChartData();

        public List<RsopPot> RsopPots { get; set; }


        [Obsolete("Only for design data", true)]
        public DomainResultViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public DomainResultViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork; 
        }

        public void loadRsopPots()
        {
            Domain = unitOfWork.ADDomainRepository.GetByID(RefId);
            RsopPots = Domain.RsopPots;
            fillLists();
        }

        private void fillLists()
        {
            var bad = new List<string>();
            var good = new List<string>();
            foreach (var pot in RsopPots)
            {
                if (pot.Rsops.FirstOrDefault().RsopPercentage > 99)
                {
                    good.Add(pot.Name);
                    
                }
                else
                {
                    bad.Add(pot.Name);
                }
            }

            goodList = good;
            badList = bad;
        }

        private List<KeyValuePair<string, int>> LoadPieChartData()
        {
            var goodPots = GoodList.Count;
            var badPots = BadList.Count;

            var valueList = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Correct", goodPots),
                new KeyValuePair<string, int>("Not Correct", badPots)
            };

            return valueList;
        }

        private void ShowPotView(int potRefId)
        {
           Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potRefId));
        }

        private void ShowTreeStructure()
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel)));
        }

        private void Back()
        {
            ShowTreeStructure();
        }
    }
}

