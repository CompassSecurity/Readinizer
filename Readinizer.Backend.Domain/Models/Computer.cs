using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class Computer
    {
        [JsonIgnore]
        public int ComputerId { get; set; }

        public string ComputerName { get; set; }

        public bool IsDomainController { get; set; }

        public virtual ICollection<OrganizationalUnit> OrganizationalUnits { get; set; }

        public string IpAddress { get; set; }

        [JsonIgnore]
        public bool PingSuccessful { get; set; }

        [JsonIgnore]
        public int? SiteRefId { get; set; }

        public Site Site { get; set; }

        public bool? isSysmonRunning { get; set; }
    }
}
