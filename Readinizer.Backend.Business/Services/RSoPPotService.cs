using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class RsopPotService : IRsopPotService
    {
        private readonly IUnitOfWork unitOfWork;
        private static int index;

        public RsopPotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task GenerateRsopPots()
        {
            index = 1;

            var rsops = await unitOfWork.RsopRepository.GetAllEntities();
            var sortedRsopsByDomain = rsops.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = FillRsopPotList(sortedRsopsByDomain);

            unitOfWork.RsopPotRepository.AddRange(rsopPots);

            await unitOfWork.SaveChangesAsync();
        }

        public List<RsopPot> FillRsopPotList(List<Rsop> sortedRsopsByDomain)
        {
            var rsopPots = new List<RsopPot>();
            AddRsopPot(sortedRsopsByDomain.First());

            foreach (var rsop in sortedRsopsByDomain.Skip(1))
            {
                var foundPot = RsopPotsEqual(rsopPots, rsop);

                if (foundPot == null)
                {
                    AddRsopPot(rsop);
                }
            }

            void AddRsopPot(Rsop rsop)
            {
                rsopPots.Add(RsopPotFactory(rsop));
            }

            return rsopPots;
        }

        private static RsopPot RsopPotFactory(Rsop rsop)
        {
            return new RsopPot
            {
                Name = index++ + ". Group of identical security settings",
                DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture),
                Domain = rsop.Domain,
                Rsops = new List<Rsop> { rsop }
            };
        }

        public async Task UpdateRsopPots(List<Rsop> rsops)
        {
            var rsopPots = await unitOfWork.RsopPotRepository.GetAllEntities();

            foreach (var rsop in rsops)
            {
                var foundPot = RsopPotsEqual(rsopPots, rsop);

                if (foundPot != null)
                {
                    foundPot.DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture);
                    unitOfWork.RsopPotRepository.Update(foundPot);
                }
                else if (!rsopPots.Any(x => RsopAndRsopPotsOuEqual(rsop, x.Rsops.First())))
                {
                    foundPot = RsopPotFactory(rsop);
                    rsopPots.Add(foundPot);
                    unitOfWork.RsopPotRepository.Add(foundPot);
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        public RsopPot RsopPotsEqual(List<RsopPot> rsopPots, Rsop currentRsop)
        {
            RsopPot foundPot = null;

            foreach (var pot in rsopPots)
            {
                var rsop = pot.Rsops.FirstOrDefault();
                if (rsop == null) continue;

                var auditSettingsEqual = SettingsEqual(rsop.AuditSettings, currentRsop.AuditSettings);
                if (!auditSettingsEqual) continue;

                var policiesEqual = SettingsEqual(rsop.Policies, currentRsop.Policies);
                if (!policiesEqual) continue;

                var registrySettingsEqual = SettingsEqual(rsop.RegistrySettings, currentRsop.RegistrySettings);
                if (!registrySettingsEqual) continue;

                var securityOptionsEqual = SettingsEqual(rsop.SecurityOptions, currentRsop.SecurityOptions);
                if (!securityOptionsEqual) continue;

                var domainsEqual = rsop.Domain.Equals(currentRsop.Domain);
                if (!domainsEqual) continue;

                if (RsopAndRsopPotsOuEqual(currentRsop, rsop)) continue;

                pot.Rsops.Add(currentRsop);
                foundPot = pot;
                break;
            }

            return foundPot;
        }

        private static bool RsopAndRsopPotsOuEqual(Rsop rsop, Rsop currentRsop)
        {
            var organisationalUnitsEqual = currentRsop.OrganizationalUnit.Name.Equals(rsop.OrganizationalUnit.Name);
            return organisationalUnitsEqual;
        }

        public bool SettingsEqual<T>(ICollection<T> currentSettings, ICollection<T> otherSettings)
        {
            if (currentSettings == null || otherSettings == null)
            {
                return (currentSettings == null && otherSettings == null);
            }

            if (currentSettings.Count != otherSettings.Count)
            {
                return false;
            }

            for (var i = 0; i < currentSettings.Count; i++)
            {
                if (!currentSettings.ElementAt(i).Equals(otherSettings.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
