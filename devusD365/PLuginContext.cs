using System;
using Microsoft.Xrm.Sdk;

namespace devusD365
{
    public class PLuginContext
    {
        public IOrganizationService OrganizationService { get; private set; }
        public IOrganizationService OrganizationServiceAdmin { get; private set; }
        private IOrganizationServiceFactory ServiceFactory { get; set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public IServiceEndpointNotificationService NotificationService { get; private set; }
        public ITracingService TracingService { get; private set; }
        public PLuginContext(IServiceProvider serviceProvider)
        {
            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            // Obtain the tracing service from the service provider.
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Get the notification service from the service provider.
            NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            // Obtain the organization factory service from the service provider.
            ServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Use the factory to generate the organization service.
            OrganizationService = ServiceFactory.CreateOrganizationService(PluginExecutionContext.UserId);

            // Use the factory to generate the organization service.
            OrganizationServiceAdmin = ServiceFactory.CreateOrganizationService(null);
        }
    }
}