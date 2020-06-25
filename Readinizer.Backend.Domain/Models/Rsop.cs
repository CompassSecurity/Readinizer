using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class Rsop
    {
        [JsonIgnore]
        public int RsopId { get; set; }

        [JsonIgnore]
        public int? DomainRefId { get; set; }

        [JsonIgnore]
        public virtual ADDomain Domain { get; set; }

        [JsonIgnore]
        public int? OURefId { get; set; }

        [JsonIgnore]
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }

        [JsonIgnore]
        public int? SiteRefId { get; set; }

        public virtual Site Site { get; set; }

        [JsonIgnore]
        public int? RsopPotRefId { get; set; }

        [JsonIgnore]
        public virtual RsopPot RsopPot { get; set; }

        public double RsopPercentage
        {
            get
            {
                var counterAuditSettings = AuditSettings.Count(auditSetting => auditSetting.TargetSettingValue == auditSetting.CurrentSettingValue);
                var counterPolicies = Policies.Count(policy => policy.TargetState == policy.CurrentState);
                var counterRegistrySettings = RegistrySettings.Count(registrySetting => registrySetting.IsPresent && registrySetting.CurrentValue.Number == registrySetting.TargetValue.Number
                                                                                                                  && registrySetting.CurrentValue.Element.Modules == registrySetting.TargetValue.Element.Modules);
                var counterSecurityOptions = SecurityOptions.Count(securityOption => securityOption.TargetDisplay.DisplayBoolean == securityOption.CurrentDisplay.DisplayBoolean);

                var overallCounter = counterAuditSettings + counterPolicies + counterRegistrySettings + counterSecurityOptions;
                var sumOfSettings = AuditSettings.Count + Policies.Count + RegistrySettings.Count + SecurityOptions.Count;

                return Math.Round(((double)overallCounter / (double)sumOfSettings) * 100);
            }
        }

        public virtual ICollection<AuditSetting> AuditSettings { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }

        public virtual ICollection<RegistrySetting> RegistrySettings { get; set; }

        public virtual ICollection<SecurityOption> SecurityOptions { get; set; }

        public virtual ICollection<Gpo> Gpos { get; set; }
    }
}
