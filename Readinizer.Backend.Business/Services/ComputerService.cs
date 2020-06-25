using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Threading.Tasks;
using NetTools;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IUnitOfWork unitOfWork;


        public ComputerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task GetComputers()
        {
            var allOUs = await unitOfWork.OrganizationalUnitRepository.GetAllEntities();
            var allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            var sites = await unitOfWork.SiteRepository.GetAllEntities();
            var DCnames = getDcNames();

            foreach (OrganizationalUnit OU in allOUs)
            {
                var entry = new DirectoryEntry(OU.LdapPath);
                var searcher = new DirectorySearcher(entry)
                {
                    Filter = ("(objectClass=computer)"), SearchScope = SearchScope.OneLevel
                };

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    var foundMember = new Computer
                    {
                        OrganizationalUnits = new List<OrganizationalUnit>(),
                        ComputerName = searchResult.GetDirectoryEntry().Name.Remove(0, "CN=".Length)
                    };
                    foundMember.IsDomainController = DCnames.Contains(foundMember.ComputerName);
                    try
                    {
                        foundMember.IpAddress = getIP(foundMember, OU, allDomains);
                    }
                    catch (Exception e)
                    {
                        foundMember.IpAddress = null;
                    }

                    foundMember.OrganizationalUnits.Add(OU);

                    unitOfWork.ComputerRepository.Add(foundMember);
                }
            }
            await unitOfWork.SaveChangesAsync();
        }

        private static List<string> getDcNames()
        {
            var DCs = new List<string>();
            
            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                try
                {
                    foreach (DomainController dc in domain.DomainControllers)
                    {
                        var dcName = dc.Name.Remove((dc.Name.Length - (dc.Domain.Name.Length + 1)),
                            dc.Domain.Name.Length + 1);
                        DCs.Add(dcName);
                    }
                }
                catch (Exception)
                {
                    return DCs;
                }

            }

            return DCs;
        }

        private static string getIP(Computer foundMember, OrganizationalUnit OU, List<ADDomain> allDomains)
        {
            foreach (var domain in allDomains)
            {
                if (domain.ADDomainId.Equals(OU.ADDomainRefId))
                {
                    foreach (var address in Dns.GetHostEntry(foundMember.ComputerName + "." + domain.Name)
                        .AddressList)
                    {
                        if (!address.IsIPv6LinkLocal)
                        {
                            return foundMember.IpAddress = address.ToString();
                        }
                    }
                }
            }

            return null;
        }

        int getSite(Computer foundMember, List<Site> sites)
        {
            foreach (Site site in sites)
            {
                foreach (string subnet in site.Subnets)
                {
                    if (IsInRange(foundMember.IpAddress, subnet))
                    {
                        return site.SiteId;
                    }
                }
            }
            return 0;
        }

        public bool IsInRange(string address, string subnet)
        {
            if(address == null)
            {
                return false;
            }
            var range = IPAddressRange.Parse(subnet);

            return range.Contains(IPAddress.Parse(address));

        }

    }
}
