using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Factory
{
    public class TreeNodesFactory : ITreeNodesFactory
    {
        private readonly IUnitOfWork unitOfWork;

        public TreeNodesFactory(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ObservableCollection<TreeNode>> CreateTree()
        {
            var domains = await unitOfWork.ADDomainRepository.GetAllEntities();
            var tree = new ObservableCollection<TreeNode>();
            var root = new TreeNode();

            var rootDomain = domains.FirstOrDefault();
            var rsopPots = GetRsopPotsOfDomain(rootDomain);
            if (rootDomain != null)
            {
                rootDomain.DomainPercentage = rsopPots.Min(x => x.Rsops.Min(y => y.RsopPercentage));
                unitOfWork.ADDomainRepository.Update(rootDomain);

                root = NewDomainNode(rootDomain);
                foreach (var rsopPot in rsopPots)
                {
                    var rsopPotOfDomain = NewRsopPotNode(rsopPot);
                    root.ChildNodes.Add(rsopPotOfDomain);
                }

                BuildTree(root, rootDomain.SubADDomains);
            }

            await unitOfWork.SaveChangesAsync();
            tree.Add(root);
            return tree;
        }

        private void BuildTree(TreeNode root, List<ADDomain> domains)
        {
            if (domains != null)
            {
                foreach (var domain in domains)
                {
                    if (domain.IsAvailable)
                    {
                        var rsopPots = GetRsopPotsOfDomain(domain);
                        domain.DomainPercentage = rsopPots.Min(x => x.Rsops.Min(y => y.RsopPercentage));
                        unitOfWork.ADDomainRepository.Update(domain);

                        var child = NewDomainNode(domain);
                        foreach (var rsopPot in rsopPots)
                        {
                            var rsopPotOfDomain = NewRsopPotNode(rsopPot);
                            child.ChildNodes.Add(rsopPotOfDomain);
                        }

                        root.ChildNodes.Add(child);
                        BuildTree(child, domain.SubADDomains);
                    }
                }
            }
        }

        private List<RsopPot> GetRsopPotsOfDomain(ADDomain domain)
        {
            var rsopsOfDomain = domain.Rsops;
            var rsopPots = new HashSet<RsopPot>();

            if (rsopsOfDomain != null)
            {
                foreach (var rsop in rsopsOfDomain)
                {
                    rsopPots.Add(unitOfWork.RsopPotRepository.GetByID(rsop.RsopPotRefId));
                }
                rsopPots.Remove(null);
            }

            return rsopPots.ToList();
        }

        private static TreeNode NewDomainNode(ADDomain domain)
        {
            return new TreeNode
            {
                Description = "Domain: ",
                IsRSoP = false,
                TypeRefIdDictionary = new Dictionary<string, int> { { "Domain", domain.ADDomainId } },
                Identifier = domain.Name,
                AnalysisPercentage = domain.DomainPercentage ?? 0.0
            };
        }

        private static TreeNode NewRsopPotNode(RsopPot rsopPot)
        {
            return new TreeNode
            {
                Description = rsopPot.Name,
                IsRSoP = true,
                TypeRefIdDictionary = new Dictionary<string, int> { { "RSoPPot", rsopPot.RsopPotId } },
                Identifier = rsopPot.DateTime,
                AnalysisPercentage = rsopPot.Rsops.First().RsopPercentage,
                OrganizationalUnits = rsopPot.Rsops.Select(rsop => rsop.OrganizationalUnit).ToList(),
                Rsop = rsopPot.Rsops.FirstOrDefault()
            };
        }
    }
}
