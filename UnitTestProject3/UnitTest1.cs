using devusD365;
using FakeXrmEasy;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Description;

namespace UnitTestProject3
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        [Obsolete]
        public void TestMethod1()
        {
            var context = new XrmFakedContext();
            var tracing = new XrmFakedTracingService();

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = "123";
            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters["Api"] = "Student/Action001";
            inputParameters["Input"] = JsonConvert.SerializeObject(dictionary);

            var plugCtx = context.GetDefaultPluginContext();
            plugCtx.MessageName = "new_stuhost";
            plugCtx.InputParameters = inputParameters;
            var form = context.ExecutePluginWith<StudentHostPlugin>(plugCtx);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["Connect"].ConnectionString);
            //WhoAmIExample(service);
            OrganizationRequest Req = new OrganizationRequest("new_stuhost");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = "123";
            ParameterCollection inputParameters = new ParameterCollection();

            Req["Api"] = "Student/Action001";
            Req["Input"] = JsonConvert.SerializeObject(dictionary);
            OrganizationResponse Response = service.Execute(Req);
            var Output = Response.Results["Output"];
        }
        static void WhoAmIExample(IOrganizationService service)
        {
            service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["Connect"].ConnectionString);
            var response = (WhoAmIResponse)service.Execute(new WhoAmIRequest());

            Console.WriteLine($"OrganizationId:{response.OrganizationId}");
            Console.WriteLine($"BusinessUnitId:{response.BusinessUnitId}");
            Console.WriteLine($"UserId:{response.UserId}");

        }


    }
}