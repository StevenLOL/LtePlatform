﻿using Lte.Parameters.Entities.Basic;
using LtePlatform.Areas.HelpPage.ModelDescriptions;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using Lte.MySqlFramework.Entities;

namespace LtePlatform.Tests.ModelDescription
{
    [TestFixture]
    public class EnumTypeModelDescriptionTest
    {
        private ModelDescriptionGenerator generator;

        enum SimpleEnum
        {
            Option1,
            Option2
        }

        [Test]
        public void Tewt_SimpleEnum()
        {
            generator = new ModelDescriptionGenerator(new HttpConfiguration());
            var description = generator.GetOrCreateModelDescription(typeof(SimpleEnum));
            Assert.IsTrue(description is EnumTypeModelDescription);
            Assert.AreEqual(description.Name, "SimpleEnum");
            Assert.AreEqual(description.ModelType, typeof(SimpleEnum));
            Assert.IsNull(description.Documentation);
            Assert.AreEqual((description as EnumTypeModelDescription).Values.Count, 2);
            var memberValues = (description as EnumTypeModelDescription).Values;
            Assert.AreEqual(memberValues[0].Name, "Option1");
            Assert.AreEqual(memberValues[0].Value, "0");
            Assert.AreEqual(memberValues[0].Documentation, null);
            Assert.AreEqual(memberValues[1].Name, "Option2");
            Assert.AreEqual(memberValues[1].Value, "1");
            Assert.AreEqual(memberValues[1].Documentation, null);

        }

        class SimpleModelDocumentationProvider : IDocumentationProvider, IModelDocumentationProvider
        {
            public string GetDocumentation(MemberInfo member)
            {
                return "This is a simple member documentation.";
            }

            public string GetDocumentation(Type type)
            {
                return "This is a simple type documentation.";
            }

            public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
            {
                throw new NotImplementedException();
            }

            public string GetDocumentation(HttpActionDescriptor actionDescriptor)
            {
                throw new NotImplementedException();
            }

            public string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
            {
                throw new NotImplementedException();
            }

            public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
            {
                throw new NotImplementedException();
            }
        }

        private void InitializeProvider()
        {
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IDocumentationProvider), new SimpleModelDocumentationProvider());
            generator = new ModelDescriptionGenerator(configuration);
        }

        [Test]
        public void Test_Configuration()
        {
            InitializeProvider();
            Assert.IsNotNull(generator.DocumentationProvider);
        }

        [Test]
        public void Test_SimpleEnum_WithSimpleModelDocumentationProvider()
        {
            InitializeProvider();
            var description = generator.GetOrCreateModelDescription(typeof(SimpleEnum));
            Assert.IsTrue(description is EnumTypeModelDescription);
            Assert.AreEqual(description.Name, "SimpleEnum");
            Assert.AreEqual(description.ModelType, typeof(SimpleEnum));
            Assert.AreEqual(description.Documentation, "This is a simple type documentation.");
        }
    }

    [TestFixture]
    public class CollectionModelDescriptionTest
    {
        [Test]
        public void Test_First()
        {
            var generator = new ModelDescriptionGenerator(new HttpConfiguration());
            var modelDescription = generator.GetOrCreateModelDescription(typeof (Cell));
            Assert.IsNotNull(modelDescription);
            Assert.AreEqual(modelDescription.Name, "Cell");
            Assert.AreEqual(modelDescription.Documentation, null);
            Assert.AreEqual(modelDescription.ParameterDocumentation, null);
        }

        [Test]
        public void Test_Second()
        {
            var generator = new ModelDescriptionGenerator(new HttpConfiguration());
            var modelDescription = generator.GetOrCreateModelDescription(typeof(VipDemandDto));
            Assert.IsNotNull(modelDescription);
            Assert.AreEqual(modelDescription.Name, "VipDemandDto");
            Assert.AreEqual(modelDescription.Documentation, "VIP需求信息数据单元");
            Assert.AreEqual(modelDescription.ParameterDocumentation, null);
        }

        [Test]
        public void Test_Assert_ComplexProvider()
        {
            var generator = new ModelDescriptionGenerator(new HttpConfiguration());
            var provider = ModelProviderFactory.GetProvider(typeof (VipDemandDto));
            Assert.IsInstanceOf<ComplexModelProvider>(provider);
            var doc = generator.CreateDefaultDocumentation(typeof (VipDemandDto));
            Assert.AreEqual(doc, "VIP需求信息数据单元");
        }
    }
}
