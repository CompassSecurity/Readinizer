using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class ModuleNames
    {
        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("ValueElementData")]
        public string ValueElementData { get; set; }
    }
}
