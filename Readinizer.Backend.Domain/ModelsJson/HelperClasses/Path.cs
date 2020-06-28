using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Path
    {
        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; }
    }
}
