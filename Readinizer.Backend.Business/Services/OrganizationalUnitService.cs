using Readinizer.Backend.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.DirectoryServices;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.Business.Services
{
    public class OrganizationalUnitService : IOrganizationalUnitService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrganizationalUnitService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task GetAllOrganizationalUnits()
        {
            var allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            
            foreach (var domain in allDomains)
            {
                if (domain.IsAvailable)
                {
                    var entry = new DirectoryEntry("LDAP://" + domain.Name);
                    var searcher = new DirectorySearcher(entry)
                    {
                        Filter = ("(objectCategory=organizationalUnit)"), SearchScope = SearchScope.OneLevel
                    };
                    var foundOUs = new List<OrganizationalUnit>();

                    foreach (SearchResult searchResult in searcher.FindAll())
                    {
                        var foundOU = new OrganizationalUnit
                        {
                            Name = searchResult.Properties["ou"][0].ToString(),
                            LdapPath = searchResult.Path,
                            ADDomainRefId = domain.ADDomainId,
                        };
                        foundOU.SubOrganizationalUnits = GetChildOUs(foundOU.LdapPath, foundOU);

                        foundOUs.Add(foundOU);
                    }

                    var defaultContainerSearcher = new DirectorySearcher(entry)
                    {
                        Filter = ("(objectCategory=Container)")
                    };
                    defaultContainerSearcher.Filter = ("(CN=Computers)");
                    foreach (SearchResult defaultContainers in defaultContainerSearcher.FindAll())
                    {
                        var foundContainer = new OrganizationalUnit
                        {
                            Name = defaultContainers.Properties["cn"][0].ToString(),
                            LdapPath = defaultContainers.Path,
                            ADDomainRefId = domain.ADDomainId
                        };

                        foundOUs.Add(foundContainer);
                    }

                    unitOfWork.OrganizationalUnitRepository.AddRange(foundOUs);
                }
            }
            await unitOfWork.SaveChangesAsync();
        }

        public List<OrganizationalUnit> GetChildOUs(string ldapPath, OrganizationalUnit parentOU)
        {
            var childOUs = new List<OrganizationalUnit>();

            var childEntry = new DirectoryEntry(ldapPath);
            var childSearcher = new DirectorySearcher(childEntry)
            {
                Filter = ("(objectCategory=organizationalUnit)"), SearchScope = SearchScope.OneLevel
            };

            foreach (SearchResult childResult in childSearcher.FindAll())
            {
                var childOU = new OrganizationalUnit
                {
                    Name = childResult.Properties["ou"][0].ToString(),
                    LdapPath = childResult.Path,
                    ADDomainRefId = parentOU.ADDomainRefId
                };
                childOU.SubOrganizationalUnits = GetChildOUs(childOU.LdapPath, childOU);

                childOUs.Add(childOU);

                unitOfWork.OrganizationalUnitRepository.Add(childOU);
            }

            return childOUs;
        }
    }
}
