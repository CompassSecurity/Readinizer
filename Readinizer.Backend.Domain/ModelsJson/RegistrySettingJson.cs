using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class RegistrySettingJson
    {
        public RegistrySettingJson()
        {
            KeyPath = "Undefined";
            CurrentValue = new Value();
        }

        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("KeyPath")]
        public string KeyPath { get; set; }

        [JsonProperty("Value")]
        public Value CurrentValue { get; set; }
    }
}
