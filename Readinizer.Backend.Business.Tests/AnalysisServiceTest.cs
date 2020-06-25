using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess.UnitOfWork;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Tests
{
    [TestClass]
    public class AnalysisServiceTest : BaseReadinizerTestData
    {
        public static AnalysisService analysisService { get; set; } = new AnalysisService(new UnitOfWork());

        [TestMethod]
        public void XmlToJson_CheckIfJson_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");

            var rsopJson = analysisService.XmlToJson(doc);

            Assert.IsInstanceOfType(rsopJson, typeof(JObject));
        }

        [TestMethod]
        public void XmlToJson_CheckIfJsonEmpty_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\emptyValid.xml");

            var rsopJson = analysisService.XmlToJson(doc);

            Assert.IsInstanceOfType(rsopJson, typeof(JObject));
        }

        [TestMethod]
        public void XmlToJson_CheckIfJson_WithoutNamespaces_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var regex = "q[0-9]:";

            var rsopJson = analysisService.XmlToJson(doc);

            var hasNamespaces = Regex.Match(rsopJson.ToString(), regex, RegexOptions.IgnoreCase);
            Assert.IsFalse(hasNamespaces.Success);
        }

        [TestMethod]
        public void AnalyseAuditSettings_GoodRsop_NumberOfAuditSettingsIs28_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var auditSettings = analysisService.AnalyseAuditSettings(rsopJson);

            Assert.AreEqual(28, auditSettings.Count);
        }

        [TestMethod]
        public void AnalyseAuditSettings_GoodRsop_AllItemsAreNotNull_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var auditSettings = analysisService.AnalyseAuditSettings(rsopJson);

            CollectionAssert.AllItemsAreNotNull(auditSettings);
        }

        [TestMethod]
        public void AnalyseAuditSettings_GoodRsop_AreCorrectType_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var auditSettings = analysisService.AnalyseAuditSettings(rsopJson);

            CollectionAssert.AllItemsAreInstancesOfType(auditSettings, typeof(AuditSetting));
        }

        [TestMethod]
        public void AnalyseAuditSettings_GoodRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var auditSettings = analysisService.AnalyseAuditSettings(rsopJson);

            CollectionAssert.AreEqual(RecommendedAuditSettings, auditSettings);
        }

        [TestMethod]
        public void AnalysePolicies_GoodRsop_NumberOfPoliciesIs3_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var policies = analysisService.AnalysePolicies(rsopJson);

            Assert.AreEqual(3, policies.Count);
        }

        [TestMethod]
        public void AnalysePolicies_GoodRsop_AllItemsAreNotNull_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var policies = analysisService.AnalysePolicies(rsopJson);

            CollectionAssert.AllItemsAreNotNull(policies);
        }

        [TestMethod]
        public void AnalysePolicies_GoodRsop_AreCorrectType_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var policies = analysisService.AnalysePolicies(rsopJson);

            CollectionAssert.AllItemsAreInstancesOfType(policies, typeof(Policy));
        }

        [TestMethod]
        public void AnalysePolicies_GoodRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var policies = analysisService.AnalysePolicies(rsopJson);

            CollectionAssert.AreEqual(RecommendedPolicies, policies);
        }

        [TestMethod]
        public void AnalyseSecurityOptions_GoodRsop_NumberOfPoliciesIs1_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var securityOptions = analysisService.AnalyseSecurityOptions(rsopJson);

            Assert.AreEqual(1, securityOptions.Count);
        }

        [TestMethod]
        public void AnalyseSecurityOptions_GoodRsop_AllItemsAreNotNull_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var securityOptions = analysisService.AnalyseSecurityOptions(rsopJson);

            CollectionAssert.AllItemsAreNotNull(securityOptions);
        }

        [TestMethod]
        public void AnalyseSecurityOptions_GoodRsop_AreCorrectType_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var securityOptions = analysisService.AnalyseSecurityOptions(rsopJson);

            CollectionAssert.AllItemsAreInstancesOfType(securityOptions, typeof(SecurityOption));
        }

        [TestMethod]
        public void AnalyseSecurityOptions_GoodRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var securityOptions = analysisService.AnalyseSecurityOptions(rsopJson);

            CollectionAssert.AreEqual(RecommendedSecurityOptions, securityOptions);
        }

        [TestMethod]
        public void AnalyseRegistrySettings_GoodRsop_NumberOfPoliciesIs2_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var registrySettings = analysisService.AnalyseRegistrySetting(rsopJson);

            Assert.AreEqual(2, registrySettings.Count);
        }

        [TestMethod]
        public void AnalyseRegistrySettings_GoodRsop_AllItemsAreNotNull_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var registrySettings = analysisService.AnalyseRegistrySetting(rsopJson);

            CollectionAssert.AllItemsAreNotNull(registrySettings);
        }

        [TestMethod]
        public void AnalyseRegistrySettings_GoodRsop_AreCorrectType_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var registrySettings = analysisService.AnalyseRegistrySetting(rsopJson);

            CollectionAssert.AllItemsAreInstancesOfType(registrySettings, typeof(RegistrySetting));
        }

        [TestMethod]
        public void AnalyseRegistrySettings_GoodRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\recommended_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var registrySettings = analysisService.AnalyseRegistrySetting(rsopJson);

            CollectionAssert.AreEqual(RecommendedRegistrySettings, registrySettings);
        }
        
        [TestMethod]
        public void AnalyseAuditSettings_BadRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\bad_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var auditSettings = analysisService.AnalyseAuditSettings(rsopJson);

            CollectionAssert.AreNotEqual(RecommendedAuditSettings, auditSettings);
        }

        [TestMethod]
        public void AnalysePolicies_BadRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\bad_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var policies = analysisService.AnalysePolicies(rsopJson);

            CollectionAssert.AreNotEqual(RecommendedPolicies, policies);
        }

        [TestMethod]
        public void RegistrySettings_BadRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\bad_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var registrySettings = analysisService.AnalyseRegistrySetting(rsopJson);

            CollectionAssert.AreNotEqual(RecommendedRegistrySettings, registrySettings);
        }

        [TestMethod]
        public void AnalyseSecurityOptions_BadRsop_AllSettingsAreEqual_Test()
        {
            var doc = new XmlDocument();
            doc.Load("..\\..\\TestRsopXml\\bad_rsop.xml");
            var rsopJson = analysisService.XmlToJson(doc);

            var securityOptions = analysisService.AnalyseSecurityOptions(rsopJson);

            CollectionAssert.AreNotEqual(RecommendedSecurityOptions, securityOptions);
        }
    }
}
