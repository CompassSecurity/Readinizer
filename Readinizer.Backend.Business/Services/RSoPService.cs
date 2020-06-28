using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Microsoft.GroupPolicy;
using System.IO;

namespace Readinizer.Backend.Business.Services
{
    public class RsopService : IRsopService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISysmonService sysmonService;
        private readonly IPingService pingService;

        public RsopService(IUnitOfWork unitOfWork, ISysmonService sysmonService, IPingService pingService)
        {
            this.unitOfWork = unitOfWork;
            this.sysmonService = sysmonService;
            this.pingService = pingService;
        }


        public async Task GetRsopOfReachableComputers()
        {
            clearOldRsops();
            List<OrganizationalUnit> allOUs = await unitOfWork.OrganizationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int?> collectedSiteIds = new List<int?>();
            foreach (OrganizationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                var domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);

                if(OU.Computers != null)
                {
                    OU.HasReachableComputer = false;
                    foreach (var computer in OU.Computers)
                    {
                        if (!collectedSiteIds.Contains(computer.SiteRefId) && pingService.IsPingable(computer.IpAddress))
                        {
                            computer.PingSuccessful = true;
                            unitOfWork.ComputerRepository.Update(computer);

                            OU.HasReachableComputer = true;
                            unitOfWork.OrganizationalUnitRepository.Update(OU);

                            collectedSiteIds.Add(computer.SiteRefId);

                            getRSoP(computer.ComputerName + "." + domain.Name,
                                OU.OrganizationalUnitsId, computer.SiteRefId,
                                System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                        }

                    }
                    await unitOfWork.SaveChangesAsync();
                }
                
            }
        }

        public async Task GetRsopOfReachableComputersAndCheckSysmon(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = "Sysmon";
            }

            clearOldRsops();
            List<OrganizationalUnit> allOUs = await unitOfWork.OrganizationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int?> collectedSiteIds = new List<int?>();
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            foreach (OrganizationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                ADDomain domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);
                string domainName = domain.Name;

                if (OU.Computers != null)
                {
                    OU.HasReachableComputer = false;
                    foreach (var computer in OU.Computers)
                    {
                        if (pingService.IsPingable(computer.IpAddress))
                        {
                            if (!collectedSiteIds.Contains(computer.SiteRefId))
                            {
                                computer.PingSuccessful = true;
                                

                                OU.HasReachableComputer = true;
                                unitOfWork.OrganizationalUnitRepository.Update(OU);

                                collectedSiteIds.Add(computer.SiteRefId);

                                getRSoP(computer.ComputerName + "." + domainName,
                                                                OU.OrganizationalUnitsId, computer.SiteRefId,
                                                                user);
                            }

                            computer.isSysmonRunning = sysmonService.IsSysmonRunning(serviceName, user,
                                computer.ComputerName,
                                domainName);

                            unitOfWork.ComputerRepository.Update(computer);
                        }
                    }
                    await unitOfWork.SaveChangesAsync();
                }


            }
        }


        public void getRSoP(string computerPath, int ouId, int? siteId, string user)
        {
            try
            {
                var gpRsop = new GPRsop(RsopMode.Logging, "")
                {
                    LoggingMode = LoggingMode.Computer,
                    LoggingComputer = computerPath,
                    LoggingUser = user
                };
                gpRsop.CreateQueryResults();
                gpRsop.GenerateReportToFile(ReportType.Xml, ConfigurationManager.AppSettings["ReceivedRSoP"] + "\\" + "Ou_" + ouId + "-Site_" + siteId + ".xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void clearOldRsops()
        {
            string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["ReceivedRSoP"]);
            foreach (string filePath in filePaths)
            {

                File.Delete(filePath);
            }
                
        }
    }
}