using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class SecuritySettingParserService : ISecuritySettingParserService
    {
        private readonly IUnitOfWork unitOfWork;
        
        public SecuritySettingParserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<SecuritySettingsParsed>> ParseSecuritySettings(int refId, string type)
        {
            var rsop = new Rsop();
            if (type.Equals("RSoPPot"))
            {

                var rsopPot = unitOfWork.RsopPotRepository.GetByID(refId);
                rsop = rsopPot.Rsops.FirstOrDefault();
            }
            else
            {
                rsop = unitOfWork.RsopRepository.GetByID(refId);
            }

            var GPOs = await unitOfWork.GpoRepository.GetAllEntities();
            var settings = new List<SecuritySettingsParsed>();
            if (rsop != null)
            {
                foreach (var setting in rsop.AuditSettings)
                {
                    var parsedSetting = SecuritySettingFactory(setting.SubcategoryName,
                        setting.CurrentSettingValue.ToString(), setting.TargetSettingValue.ToString());
                    var gopId = setting.GpoIdentifier;

                    ParseSecuritySetting(gopId, parsedSetting, GPOs);

                    settings.Add(parsedSetting);
                }

                foreach (var setting in rsop.Policies)
                {
                    var parsedSetting = SecuritySettingFactory(setting.Name, setting.CurrentState, setting.TargetState);
                    var gopId = setting.GpoIdentifier;

                    ParseSecuritySetting(gopId, parsedSetting, GPOs);

                    settings.Add(parsedSetting);
                }


                foreach (var setting in rsop.RegistrySettings)
                {
                    var parsedSetting = SecuritySettingFactory(setting.Name, setting.CurrentValue.Name,
                        setting.TargetValue.Name);
                    var gopId = setting.GpoIdentifier;

                    ParseSecuritySetting(gopId, parsedSetting, GPOs);

                    settings.Add(parsedSetting);
                }

                foreach (var setting in rsop.SecurityOptions)
                {
                    var parsedSetting = SecuritySettingFactory(setting.Description,
                        setting.CurrentDisplay.DisplayBoolean, setting.TargetDisplay.DisplayBoolean);
                    var gopId = setting.GpoIdentifier;

                    ParseSecuritySetting(gopId, parsedSetting, GPOs);

                    settings.Add(parsedSetting);
                }
            }


            return settings;
        }

        private static SecuritySettingsParsed SecuritySettingFactory(string setting, string value, string target)
        {
            return new SecuritySettingsParsed
            {
                Setting = setting,
                Value = value,
                Target = target
            };
        }

        private static void ParseSecuritySetting(string gopId, SecuritySettingsParsed parsedSetting, List<Gpo> GPOs)
        {
            parsedSetting.GPO = gopId.Equals("NoGpoId") ? "-" : GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(gopId)).Name;

            if (parsedSetting.Value.Equals(parsedSetting.Target))
            {
                parsedSetting.Icon = "Check";
                parsedSetting.Color = "Green";
            }
            else if (parsedSetting.Value.Equals("NotDefined") || parsedSetting.Value.Equals("Undefined"))
            {
                parsedSetting.Icon = "Exclamation";
                parsedSetting.Color = "Orange";
            }
            else
            {
                parsedSetting.Icon = "Close";
                parsedSetting.Color = "Red";
            }
        }
    }
}
