using System.Linq;
using Readinizer.Backend.DataAccess.Context;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class SiteRepository : GenericRepository<Site>
    {
        public SiteRepository(ReadinizerDbContext context) : base(context)
        {
        }

        public virtual Site GetOrganisationalUnitByName(string name)
        {
            return context.Set<Site>().FirstOrDefault(x => x.Name == name);
        }
    }
}
