using System.Collections.Generic;
using Newtonsoft.Json;
using Readinizer.Backend.Domain.ModelsJson.Converter;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.Models
{
    public class Gpo
    {
        [JsonIgnore]
        public int GpoId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; } = new Identifier();

        [JsonProperty("Path")]
        public Path GpoPath { get; set; }

        [JsonProperty("Enabled")]
        public string Enabled { get; set; }
        
        [JsonProperty("Link")]
        [JsonConverter(typeof(SingleValueArrayConverter<Link>))]
        public List<Link> Link { get; set; }

        public Gpo NotIdentified()
        {
            return new Gpo
            {
                Name = "Undefined",
                GpoIdentifier = new Identifier
                {
                    Id = "No Identifier"
                },
                GpoPath = new Path
                {
                    GpoIdentifier = new Identifier
                    {
                        Id = "No Identifier"
                    }
                },
                Enabled = "Undefined",
                Link = new List<Link>
                {
                    new Link
                    {
                        AppliedOrder = "Undefined",
                        SOMPath = "Undefined"
                    }
                }
            };
        }

        public bool ShouldSerializeGpoIdentifier() => false;

        public bool ShouldSerializeLink() => false;
    }
}
