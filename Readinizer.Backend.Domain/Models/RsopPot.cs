using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class RsopPot
    {
        [JsonIgnore]
        public int RsopPotId { get; set; }

        public string Name { get; set; }

        public string DateTime { get; set; }

        public int? DomainRefId { get; set; }

        public virtual ADDomain Domain { get; set; }

        public virtual ICollection<Rsop> Rsops { get; set; }
    }
}
