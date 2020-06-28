using Newtonsoft.Json;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.Models
{
    public class SecurityOption : GpoSetting
    {
        [JsonIgnore]
        public int SecurityOptionId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("TargetDisplay")]
        public Display TargetDisplay { get; set; }

        public Display CurrentDisplay { get; set; } = new Display();

        public override bool IsStatusOk => CurrentDisplay.DisplayBoolean.Equals(TargetDisplay.DisplayBoolean);

        public override bool Equals(object obj)
        {
            if (CurrentDisplay.Name != null && CurrentDisplay.DisplayBoolean != null)
            {
                var securityOption = obj as SecurityOption;

                if (securityOption == null)
                {
                    return false;
                }

                return CurrentDisplay.Name == securityOption.CurrentDisplay.Name &&
                       CurrentDisplay.DisplayBoolean == securityOption.CurrentDisplay.DisplayBoolean;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode() * 17;
        }
    }
}
