using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using Readinizer.Backend.Domain.Exceptions;
using AD = System.DirectoryServices.ActiveDirectory;

namespace Readinizer.Backend.Business.Services
{
    public class ADDomainService : IADDomainService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork unitOfWork;

        public ADDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task SearchDomains(string domainName, bool subdomainsChecked)
        {
            var domains = new List<AD.Domain>();
            var treeDomainsWithChildren = new List<AD.Domain>();
            var unavailableDomains = new List<string>();

            try
            {
                AD.Domain startDomain;
                if (string.IsNullOrEmpty(domainName))
                {
                    startDomain = Forest.GetCurrentForest().RootDomain;
                }
                else
                {
                    startDomain = AD.Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainName));
                }

                if (subdomainsChecked)
                {
                    GetTreeDomains(startDomain, treeDomainsWithChildren, unavailableDomains);
                    AddAllChildDomains(startDomain, domains, unavailableDomains);
                }
                else
                {
                    domains.Add(startDomain);
                }
            }
            catch (UnauthorizedAccessException accessException)
            {
                var message = "Invalid access rights for domain call";
                logger.Error(accessException, message);
                throw new InvalidAuthenticationException(message);
            }
            catch (ActiveDirectoryServerDownException severDownException)
            {
                var message = $"Server {severDownException.Name} is down";
                logger.Error(severDownException, message);
                throw new InvalidAuthenticationException(message);
            }
            catch (ActiveDirectoryObjectNotFoundException adObjectNotFoundException)
            {
                var message = $"The domain {adObjectNotFoundException.Name} could not be contacted";
                logger.Error(adObjectNotFoundException, message);
                throw new InvalidAuthenticationException(message);
            }
            catch (Exception e)
            {
                var message = e.Message;
                logger.Error(e, message);
                throw new InvalidAuthenticationException(message);
            }

            var models = MapToDomainModel(domains, treeDomainsWithChildren);
            unitOfWork.ADDomainRepository.AddRange(models);

            var modelsUnavailable = unavailableDomains.Select(x => new ADDomain { Name = x, IsAvailable = false }).ToList();
            unitOfWork.ADDomainRepository.AddRange(modelsUnavailable);

            await unitOfWork.SaveChangesAsync();
        }

        private static void GetTreeDomains(AD.Domain startDomain, List<AD.Domain> treeDomainsWithChildren, List<string> unavailableDomains)
        {
            var domainTrusts = startDomain.GetAllTrustRelationships();
            List<AD.Domain> treeDomains = new List<AD.Domain>();

            foreach (TrustRelationshipInformation domainTrust in domainTrusts)
            {
                if (domainTrust.TrustType.Equals(TrustType.TreeRoot))
                {
                    try
                    {
                        var treeDomain =
                            AD.Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainTrust.TargetName));
                        treeDomains.Add(treeDomain);
                    }
                    catch
                    {
                        unavailableDomains.Add(domainTrust.TargetName);
                    }
                }
            }

            foreach (var treeDomain in treeDomains)
            {
                AddAllChildDomains(treeDomain, treeDomainsWithChildren, unavailableDomains);
            }
        }

        private static void AddAllChildDomains(AD.Domain root, List<AD.Domain> domains, List<string> unavailableDomains)
        {
            domains.Add(root);

            for (var i = 0; i < root.Children.Count; ++i)
            {
                try
                {
                    var subDomain = AD.Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, root.Children[i].Name));
                    AddAllChildDomains(subDomain, domains, unavailableDomains);
                }
                catch
                {
                    unavailableDomains.Add(root.Children[i].Name);
                }
            }
        }

        private static List<ADDomain> MapToDomainModel(List<AD.Domain> domains, List<AD.Domain> treeDomains)
        {
            var models = domains.Select(x => new ADDomain { Name = x.Name, IsAvailable = true, SubADDomains = new List<ADDomain>() }).ToList();
            var treeModels = treeDomains.Select(x => new ADDomain {Name = x.Name, IsAvailable = true, IsTreeRoot = true, SubADDomains = new List<ADDomain>() }).ToList();

            AddSubDomains(domains, models);
            AddSubDomains(treeDomains, treeModels);

            var allModels = models.Union(treeModels).ToList();

            var root = new ADDomain();
            if (allModels.Exists(x => IsForestRoot(x.Name)))
            {
                root = allModels.FirstOrDefault(m => IsForestRoot(m.Name));
                root.IsForestRoot = true;
            }
            else
            {
                root = allModels.FirstOrDefault();
            }

            root?.SubADDomains.AddRange(treeModels);

            return allModels;
        }

        private static void AddSubDomains(List<AD.Domain> domains, List<ADDomain> models)
        {
            foreach (var adDomain in models)
            {
                var children = domains.ToArray().Where(d => d.Parent?.Name == adDomain.Name).Select(x => x.Name);
                adDomain.SubADDomains.AddRange(models.Where(m => children.Contains(m.Name)));
            }
        }

        private static bool IsForestRoot(string domainName)
        {
            return Forest.GetCurrentForest().RootDomain.Name.Equals(domainName);
        }

        public bool IsDomainInForest(string fullyQualifiedDomainName)
        {
            var isInForest = false;

            foreach (AD.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    isInForest = true;
                }
            }

            return isInForest;
        }
    }
}
