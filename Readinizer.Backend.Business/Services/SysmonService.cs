using System.Management;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.Business.Services
{
    public class SysmonService : ISysmonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPingService pingService;

        public SysmonService(IUnitOfWork unitOfWork, IPingService pingService)
        {
            this.unitOfWork = unitOfWork;
            this.pingService = pingService;
        }

        public bool IsSysmonRunning(string serviceName, string user, string computerName, string domain)
        {
            var op = new ConnectionOptions();
            var scope = new ManagementScope(@"\\" + computerName +"."+ domain + "\\root\\cimv2", op);
            scope.Connect();
            var path = new ManagementPath("Win32_Service");
            var services = new ManagementClass(scope, path, null);

            foreach (var service in services.GetInstances())
            { 
                if (service.GetPropertyValue("Name").ToString().Equals(serviceName) && service.GetPropertyValue("State").ToString().ToLower().Equals("running"))
                { 
                    return true;
                }
            }
            return false;
        }
    }
}
