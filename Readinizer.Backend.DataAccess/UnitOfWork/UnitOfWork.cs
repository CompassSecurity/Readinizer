using System;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Context;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ReadinizerDbContext context = new ReadinizerDbContext();
        private GenericRepository<ADDomain> adDomainRepository;
        private GenericRepository<OrganizationalUnit> organizationalUnitRepository;
        private OrganizationalUnitRepository specificOrganizationalUnitRepository;
        private GenericRepository<Computer> computerRepository;
        private GenericRepository<Site> siteRepository;
        private SiteRepository specificSiteRepository;
        private GenericRepository<Rsop> rsopRepository;
        private GenericRepository<RsopPot> rsopPotRepository;
        private GenericRepository<Gpo> gpoRepository;

        public GenericRepository<ADDomain> ADDomainRepository
        {
            get
            {
                if (adDomainRepository == null)
                {
                    adDomainRepository = new GenericRepository<ADDomain>(context);
                }

                return adDomainRepository;
            }
        }

        public GenericRepository<OrganizationalUnit> OrganizationalUnitRepository
        {
            get
            {
                if (organizationalUnitRepository == null)
                {
                    organizationalUnitRepository = new GenericRepository<OrganizationalUnit>(context);
                }

                return organizationalUnitRepository;
            }
        }

        public OrganizationalUnitRepository SpecificOrganizationalUnitRepository
        {
            get
            {
                if (specificOrganizationalUnitRepository == null)
                {
                    specificOrganizationalUnitRepository = new OrganizationalUnitRepository(context);
                }

                return specificOrganizationalUnitRepository;
            }
        }

        public GenericRepository<Computer> ComputerRepository
        {
            get
            {
                if (computerRepository == null)
                {
                    computerRepository = new GenericRepository<Computer>(context);
                }

                return computerRepository;
            }
        }

        public GenericRepository<Site> SiteRepository
        {
            get
            {
                if (siteRepository == null)
                {
                    siteRepository = new GenericRepository<Site>(context);
                }

                return siteRepository;
            }
        }

        public SiteRepository SpecificSiteRepository
        {
            get
            {
                if (specificSiteRepository == null)
                {
                    specificSiteRepository = new SiteRepository(context);
                }

                return specificSiteRepository;
            }
        }

        public GenericRepository<Rsop> RsopRepository
        {
            get
            {
                if (rsopRepository == null)
                {
                    rsopRepository = new GenericRepository<Rsop>(context);
                }

                return rsopRepository;
            }
        }

        public GenericRepository<RsopPot> RsopPotRepository
        {
            get
            {
                if (rsopPotRepository == null)
                {
                    rsopPotRepository = new GenericRepository<RsopPot>(context);
                }

                return rsopPotRepository;
            }
        }

        public GenericRepository<Gpo> GpoRepository
        {
            get
            {
                if (gpoRepository == null)
                {
                    gpoRepository = new GenericRepository<Gpo>(context);
                }

                return gpoRepository;
            }
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
