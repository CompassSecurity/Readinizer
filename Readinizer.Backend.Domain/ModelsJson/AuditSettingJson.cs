using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class AuditSettingJson
    {
        public AuditSettingJson()
        {
            SubcategoryName = "Undefined";
            PolicyTarget = "Undefined";
            CurrentSettingValue = 0;
        }

        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("SettingValue")]
        public AuditSetting.AuditSettingValue CurrentSettingValue { get; set; }
    }
}
