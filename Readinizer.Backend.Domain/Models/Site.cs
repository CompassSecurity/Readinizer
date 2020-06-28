using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class Site
    {
        [JsonIgnore]
        public int SiteId { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<string> Subnets { get; set; }

        [JsonIgnore]
        public virtual ICollection<ADDomain> Domains { get; set; }

        [JsonIgnore]
        public virtual ICollection<Computer> Computers { get; set; }

        [JsonIgnore]
        public virtual List<Rsop> Rsops { get; set; }
    }
}
