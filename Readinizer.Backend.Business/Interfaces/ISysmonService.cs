namespace Readinizer.Backend.Business.Interfaces
{
    public interface ISysmonService
    {
        bool IsSysmonRunning(string serviceName, string user, string computername, string domain);
    }
}
