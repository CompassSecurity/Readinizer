using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IRsopService
    {

        Task GetRsopOfReachableComputers();

        Task GetRsopOfReachableComputersAndCheckSysmon(string serviceName);
    }
}
