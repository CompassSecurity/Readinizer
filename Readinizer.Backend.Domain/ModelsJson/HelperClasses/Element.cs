using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Element
    {
        public Element()
        {
            Modules = "Undefined";
        }

        [JsonProperty("Data")]
        public string Modules { get; set; }
    }
}
