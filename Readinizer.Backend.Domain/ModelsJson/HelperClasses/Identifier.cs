using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Identifier
    {
        [JsonProperty("#text")]
        public string Id { get; set; }
    }
}
