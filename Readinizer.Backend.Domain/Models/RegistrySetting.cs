using Newtonsoft.Json;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.Models
{
    public class RegistrySetting : GpoSetting
    {
        [JsonIgnore]
        public int RegistrySettingId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("KeyPath")]
        public string KeyPath { get; set; }

        [JsonProperty("TargetValue")]
        public Value TargetValue { get; set; }

        public Value CurrentValue { get; set; } = new Value();

        public override bool IsStatusOk => CurrentValue.Number.Equals(TargetValue.Number);

        public override bool Equals(object obj)
        {
            if (GpoIdentifier != null)
            {
                var registrySetting = obj as RegistrySetting;

                if (registrySetting == null)
                {
                    return false;
                }

                return CurrentValue.Name == registrySetting.CurrentValue.Name && CurrentValue.Number == registrySetting.CurrentValue.Number;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }
    }
}
