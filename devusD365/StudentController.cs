using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace devusD365
{
    public class StudentController : HiddenApiControler
    {
        public virtual string Action001(string id)
        {
            var service = PLugin_context.OrganizationServiceAdmin;
            var fetchXml = @"
<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='name' />
    <attribute name='primarycontactid' />
    <attribute name='telephone1' />
    <attribute name='accountid' />
    <order attribute='name' descending='false' />
  </entity>
</fetch>";
            var name= service.RetrieveMultiple(new FetchExpression(fetchXml))?.Entities?.FirstOrDefault()?.GetAttributeValue<string>("name");
            //var response = (WhoAmIResponse)service.Execute(new WhoAmIRequest());
             return $"12345:{name}";
            //return response.UserId.ToString("N");

        }
    }

}
