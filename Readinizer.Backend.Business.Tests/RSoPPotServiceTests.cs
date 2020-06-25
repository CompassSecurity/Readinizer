using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess.UnitOfWork;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Tests
{
    [TestClass()]
    public class RsoPPotServiceTests : BaseReadinizerTestData
    {
        public static RsopPotService rsopPotService { get; set; } = new RsopPotService(new UnitOfWork());

        [TestMethod()]
        public void FillRsopPotList_NotEquallySettings_DifferentOus_Test()
        {
            var sortedRsopsByDomain = RsopsNotEqualDifferentOus.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = rsopPotService.FillRsopPotList(sortedRsopsByDomain);
            Assert.AreEqual(2, rsopPots.Count);
        }

        [TestMethod()]
        public void FillRsopPotList_EquallySettings_DifferentOus_Test()
        {
            var sortedRsopsByDomain = RsopsEqualDifferentOus.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = rsopPotService.FillRsopPotList(sortedRsopsByDomain);
            Assert.AreEqual(1, rsopPots.Count);
        }

        [TestMethod()]
        public void FillRsopPotList_NotEquallySettings_SameOus_Test()
        {
            var sortedRsopsByDomain = RsopsNotEqualSameOus.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = rsopPotService.FillRsopPotList(sortedRsopsByDomain);
            Assert.AreEqual(2, rsopPots.Count);
        }

        [TestMethod()]
        public void FillRsopPotList_EquallySettings_SameOus_Test()
        {
            var sortedRsopsByDomain = RsopsEqualSameOus.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = rsopPotService.FillRsopPotList(sortedRsopsByDomain);
            Assert.AreEqual(2, rsopPots.Count);
        }

        [TestMethod()]
        public void RsopPotsEqual_DifferentRsops_SameOu_Test()
        {
            var rsopPotList = new List<RsopPot> { RsopPotGoodReadinizerOu };
            var rsopPotsAreEqual = rsopPotService.RsopPotsEqual(rsopPotList, BadRsopReadinizerOu) != null;
            Assert.IsFalse(rsopPotsAreEqual);
        }

        [TestMethod()]
        public void RsopPotsEqual_SameRsops_SameOu_Test()
        {
            var rsopPotList = new List<RsopPot> { RsopPotGoodReadinizerOu };
            var rsopPotsAreEqual = rsopPotService.RsopPotsEqual(rsopPotList, GoodRsopReadinizerOu) != null;
            Assert.IsFalse(rsopPotsAreEqual);
        }

        [TestMethod()]
        public void RsopPotsEqual_SameRsops_DifferentOus_Test()
        {
            var rsopPotList = new List<RsopPot> { RsopPotGoodReadinizerOu };
            var rsopPotsAreEqual = rsopPotService.RsopPotsEqual(rsopPotList, GoodRsopSalesOu) != null;
            Assert.IsTrue(rsopPotsAreEqual);
        }

        [TestMethod()]
        public void RsopPotsEqual_DifferentRsops_DifferentOus_Test()
        {
            var rsopPotList = new List<RsopPot> { RsopPotGoodReadinizerOu };
            var rsopPotsAreEqual = rsopPotService.RsopPotsEqual(rsopPotList, BadRsopSalesOu) != null;
            Assert.IsFalse(rsopPotsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_SameAuditSettings_Test()
        {
            var auditSettingsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.AuditSettings, GoodRsopSalesOu.AuditSettings);
            Assert.IsTrue(auditSettingsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_SamePolicies_Test()
        {
            var policiesAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.Policies, GoodRsopSalesOu.Policies);
            Assert.IsTrue(policiesAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_SameSecurityOptions_Test()
        {
            var securityOptionsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.SecurityOptions, GoodRsopSalesOu.SecurityOptions);
            Assert.IsTrue(securityOptionsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_SameRegistrySettings_Test()
        {
            var registrySettingsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.RegistrySettings, GoodRsopSalesOu.RegistrySettings);
            Assert.IsTrue(registrySettingsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_DifferentAuditSettings_Test()
        {
            var auditSettingsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.AuditSettings, BadRsopSalesOu.AuditSettings);
            Assert.IsFalse(auditSettingsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_DifferentPolicies_Test()
        {
            var policiesAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.Policies, BadRsopSalesOu.Policies);
            Assert.IsFalse(policiesAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_DifferentSecurityOptions_Test()
        {
            var securityOptionsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.SecurityOptions, BadRsopSalesOu.SecurityOptions);
            Assert.IsFalse(securityOptionsAreEqual);
        }

        [TestMethod()]
        public void SettingsEqual_DifferentRegistrySettings_Test()
        {
            var registrySettingsAreEqual =
                rsopPotService.SettingsEqual(GoodRsopReadinizerOu.RegistrySettings, BadRsopSalesOu.RegistrySettings);
            Assert.IsFalse(registrySettingsAreEqual);
        }
    }
}