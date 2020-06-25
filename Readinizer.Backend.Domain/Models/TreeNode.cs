using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class TreeNode
    {
        [JsonIgnore]
        public Dictionary<string, int> TypeRefIdDictionary { get; set; }

        public string Identifier { get; set; }

        public string Description { get; set; }

        public double AnalysisPercentage { get; set; }

        [JsonIgnore]
        public bool IsRSoP { get; set; }

        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        public List<OrganizationalUnit> OrganizationalUnits { get; set; }

        public Rsop Rsop { get; set; }
    }
}
