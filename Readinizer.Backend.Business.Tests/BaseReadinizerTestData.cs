using System.Collections.Generic;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Tests
{
    public class BaseReadinizerTestData : BaseReadinizerTestSettingData
    {
        public static Gpo ReadinizerGoodGpo = new Gpo
        {
            Name = "Readinizer Good GPO",
            Enabled = "true",
            GpoIdentifier = new Identifier
            {
                Id = "1"
            },
            Link = new List<Link>
            {
                new Link
                {
                    SOMPath = ""
                }
            },
        };

        public static Gpo ReadinizerBadGpo = new Gpo
        {
            Name = "Readinizer Bad GPO",
            Enabled = "true",
            GpoIdentifier = new Identifier
            {
                Id = "2"
            },
            Link = new List<Link>
            {
                new Link
                {
                    SOMPath = ""
                }
            },
        };

        public static ADDomain ReadinizerDomain = new ADDomain
        {
            Name = "readinizer.ch",
            IsAvailable = true,
            IsForestRoot = true,
            IsTreeRoot = false,
            ParentId = null,
        };

        public static OrganizationalUnit ReadinizerOu = new OrganizationalUnit
        {
            Name = "Readinizer Ou",
            ADDomain = ReadinizerDomain,
            Computers = new List<Computer>
            {
                new Computer
                {
                    ComputerName = "ReadinizerWS01",
                    IpAddress = "10.0.0.9",
                    IsDomainController = false,
                    isSysmonRunning = true,
                    PingSuccessful = true,
                }
            },
            HasReachableComputer = true,
            LdapPath = "test\\path",
        };

        public static OrganizationalUnit ReadinizerSalesOu = new OrganizationalUnit
        {
            Name = "Readinizer Sales Ou",
            ADDomain = ReadinizerDomain,
            Computers = new List<Computer>
            {
                new Computer
                {
                    ComputerName = "ReadinizerWS02",
                    IpAddress = "10.0.1.9",
                    IsDomainController = false,
                    isSysmonRunning = true,
                    PingSuccessful = true,
                }
            },
            HasReachableComputer = true,
            LdapPath = "test\\path",
        };

        public static Rsop GoodRsopReadinizerOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganizationalUnit = ReadinizerOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceSuccessAndFailure,
                LogoffSuccess,
                ProcessCreationSuccess
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineEnabled,
                ModuleLoggingEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyEnabled
            }
        };

        public static Rsop BadRsopReadinizerOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganizationalUnit = ReadinizerOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceFailure,
                LogoffFailure,
                ProcessCreationNoAuditing
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionNotEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineNotEnabled,
                ModuleLoggingNotEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyNotEnabled
            }
        };

        public static Rsop GoodRsopSalesOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganizationalUnit = ReadinizerSalesOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceSuccessAndFailure,
                LogoffSuccess,
                ProcessCreationSuccess
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineEnabled,
                ModuleLoggingEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyEnabled
            }
        };

        public static Rsop BadRsopSalesOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerBadGpo
            },
            Domain = ReadinizerDomain,
            OrganizationalUnit = ReadinizerSalesOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceFailure,
                ProcessCreationNoAuditing,
                LogoffFailure
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionNotEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineNotEnabled,
                ModuleLoggingNotEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyNotEnabled
            }
        };

        public static List<Rsop> RsopsNotEqualDifferentOus = new List<Rsop>
        {
            GoodRsopReadinizerOu,
            BadRsopSalesOu
        };

        public static List<Rsop> RsopsNotEqualSameOus = new List<Rsop>
        {
            GoodRsopReadinizerOu,
            BadRsopReadinizerOu
        };

        public static List<Rsop> RsopsEqualDifferentOus = new List<Rsop>
        {
            GoodRsopReadinizerOu,
            GoodRsopSalesOu
        };

        public static List<Rsop> RsopsEqualSameOus = new List<Rsop>
        {
            GoodRsopReadinizerOu,
            GoodRsopReadinizerOu
        };

        public static RsopPot RsopPotGoodReadinizerOu = new RsopPot
        {
            Rsops = new List<Rsop>
            {
                GoodRsopReadinizerOu
            },
            Name = "1",
            DateTime = "6.6.19",
            Domain = ReadinizerDomain,
        };

        public static List<AuditSetting> RecommendedAuditSettings = new List<AuditSetting>
        {
            KerberosAuthServiceSuccessAndFailure,
            KerberosServiceTicketOpSuccessAndFailure,
            ComputerAccountManagementSuccess,
            OtherAccountManagementEventsSuccess,
            SecurityGroupManagementSuccess,
            UserAccountManagementSuccessAndFailure,
            ProcessCreationSuccess,
            ProcessTerminationSuccess,
            AccountLockoutFailure,
            GroupMembershipSuccess,
            LogoffSuccess,
            LogonSuccessAndFailure,
            OtherLogonLogoffEventsSuccessAndFailure,
            SpecialLogonSuccess,
            FileShareSuccessAndFailure,
            FileSystemSuccessAndFailure,
            HandleManipulationSuccess,
            KernelObjectSuccessAndFailure,
            OtherObjectAccessEventsSuccessAndFailure,
            RegistrySuccessAndFailure,
            SAMSuccessAndFailure,
            AuditPolicyChangeSuccess,
            MPSSVCRuleLevelPolicyChangeSuccess,
            NonSensitivePrivilegeUseSuccessAndFailure,
            SensitivePrivilegeUseSuccessAndFailure,
            SecuritySystemExtensionSuccess,
            SystemIntegritySuccessAndFailure,
            DirectoryServiceChangesSuccess
        };

        public static List<Policy> RecommendedPolicies = new List<Policy>
        {
            IncludeCommandLineEnabled,
            ModuleLoggingEnabled,
            ScriptBlockLoggingEnabled
        };

        public static List<RegistrySetting> RecommendedRegistrySettings = new List<RegistrySetting>
        {
            LsassEnabled,
            LsaProtectionEnabled
        };

        public static List<SecurityOption> RecommendedSecurityOptions = new List<SecurityOption>
        {
            ForceAuditPolicyEnabled
        };
    }   
}
