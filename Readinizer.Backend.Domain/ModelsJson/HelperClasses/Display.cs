using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Display
    {
        public Display()
        {
            Name = "Undefined";
            DisplayBoolean = "Undefined";
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("DisplayBoolean")]
        public string DisplayBoolean { get; set; }
    }
}
