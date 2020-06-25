using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class OrganizationalUnit
    {
        [JsonIgnore]
        public int OrganizationalUnitsId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string LdapPath { get; set; }

        [JsonIgnore]
        public int? ParentId { get; set; }

        [JsonIgnore]
        public int ADDomainRefId { get; set; }

        public virtual ADDomain ADDomain { get; set; }

        [JsonIgnore]
        public virtual List<Rsop> Rsops { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrganizationalUnit> SubOrganizationalUnits { get; set; }

        [JsonIgnore]
        public virtual ICollection<Computer> Computers { get; set; }

        [JsonIgnore]
        public bool? HasReachableComputer { get; set; }
    }
}
