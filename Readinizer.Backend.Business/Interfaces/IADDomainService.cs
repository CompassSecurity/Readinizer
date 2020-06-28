using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IADDomainService
    {
        Task SearchDomains(string domainName, bool subdomainsChecked);

        bool IsDomainInForest(string fullyQualifiedDomainName);
    }
}
