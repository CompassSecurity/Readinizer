using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Link
    {
        [JsonProperty("SOMPath")]
        public string SOMPath { get; set; }

        [JsonProperty("AppliedOrder")]
        public string AppliedOrder { get; set; }
    }
}
