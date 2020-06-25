using Newtonsoft.Json;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.Models
{
    public class Policy : GpoSetting
    {
        [JsonIgnore]
        public int PolicyId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("TargetState")]
        public string TargetState { get; set; }

        public string CurrentState { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("ModuleNames")]
        public ModuleNames ModuleNames { get; set; } = new ModuleNames();

        public override bool IsStatusOk => CurrentState.Equals(TargetState);

        public override bool Equals(object obj)
        {
            if (CurrentState != null && GpoIdentifier != null)
            {
                var otherPolicy = obj as Policy;

                if (otherPolicy == null)
                {
                    return false;
                }

                if (ModuleNames.ValueElementData != null)
                {
                    return CurrentState == otherPolicy.CurrentState && ModuleNames.ValueElementData == otherPolicy.ModuleNames.ValueElementData;
                }

                return CurrentState.Equals(otherPolicy.CurrentState);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }

        public bool ShouldSerializeModuleNames() => false;
    }
}
