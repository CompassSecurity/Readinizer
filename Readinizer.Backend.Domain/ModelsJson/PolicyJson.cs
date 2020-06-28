using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class PolicyJson
    {
        public PolicyJson()
        {
            Name = "Undefined";
            CurrentState = "Undefined";
            Category = "Undefined";
            ModuleNames = new ListBox();
        }

        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("State")]
        public string CurrentState { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Listbox")]
        public ListBox ModuleNames { get; set; }
    }
}
