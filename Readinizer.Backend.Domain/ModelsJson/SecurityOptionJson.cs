using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class SecurityOptionJson
    {
        public SecurityOptionJson()
        {
            KeyName = "Undefined";
            CurrentSettingNumber = "Undefined";
            CurrentDisplay = new Display();
        }

        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("SettingNumber")]
        public string CurrentSettingNumber { get; set; }

        [JsonProperty("Display")]
        public Display CurrentDisplay { get; set; }
    }
}
