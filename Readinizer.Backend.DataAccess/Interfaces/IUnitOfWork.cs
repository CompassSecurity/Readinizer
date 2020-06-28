using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        GenericRepository<ADDomain> ADDomainRepository { get; }
        GenericRepository<OrganizationalUnit> OrganizationalUnitRepository { get; }
        OrganizationalUnitRepository SpecificOrganizationalUnitRepository { get; }
        GenericRepository<Computer> ComputerRepository { get; }
        GenericRepository<Site> SiteRepository { get; }
        SiteRepository SpecificSiteRepository { get; }
        GenericRepository<Rsop> RsopRepository { get; }
        GenericRepository<RsopPot> RsopPotRepository { get; }
        GenericRepository<Gpo> GpoRepository { get; }
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        void Dispose();
    }
}
