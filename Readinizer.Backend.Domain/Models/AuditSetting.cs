using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class AuditSetting : GpoSetting
    {
        [JsonIgnore]
        public int AuditSettingId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        [JsonProperty("SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("TargetSettingValue")]
        public AuditSettingValue TargetSettingValue { get; set; }

        [JsonProperty("SettingValue")]
        public AuditSettingValue CurrentSettingValue { get; set; }

        public override bool IsStatusOk => CurrentSettingValue.Equals(TargetSettingValue);

        public override bool Equals(object obj)
        {
            if (GpoIdentifier != null && SubcategoryName != null)
            {
                if (!(obj is AuditSetting auditSetting))
                {
                    return false;
                }

                return SubcategoryName.Equals(auditSetting.SubcategoryName) && CurrentSettingValue.Equals(auditSetting.CurrentSettingValue);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public enum AuditSettingValue
        {
            NoAuditing,
            Success,
            Failure,
            SuccessAndFailure
        }
    }
}
