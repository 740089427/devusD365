using System;
using System.Reflection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace devusD365
{
    /// <summary>
    /// Base class for all plug-in classes.
    /// </summary>    
    public abstract class HostPluginBase : IPlugin
    {

        private publicControllerFactory controllersFactory;
        private ThreadSafeLazy<Assembly> lazyEntityProxyAssembly;
        private int IsolationModel = PluginAssemblyIsolationModel.Sandbox;
        private Assembly proxyTypesAssembly
        {
            get
            {
                return this.lazyEntityProxyAssembly.Value;
            }
        }
        private bool IsRunInSandbox { get { return this.IsolationModel == PluginAssemblyIsolationModel.Sandbox; } }
        public HostPluginBase()
        {
            controllersFactory = new publicControllerFactory(base.GetType().Assembly);
            lazyEntityProxyAssembly = new ThreadSafeLazy<Assembly>(new Func<Assembly>(this.GetEntityProxyAssembly));
        }

        private Assembly GetEntityProxyAssembly()
        {
            Assembly assembly = base.GetType().Assembly;
            if (assembly.GetCustomAttribute<ProxyTypesAssemblyAttribute>() != null)
            {
                return assembly;
            }
            if (!IsRunInSandbox)
            {
                AssemblyName[] assemblyName = assembly.GetReferencedAssemblies();
                foreach (var item in assemblyName)
                {
                    try
                    {
                        string name = item.Name;
                        if (!name.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!name.Equals("System", StringComparison.OrdinalIgnoreCase) && !name.StartsWith("System", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!name.StartsWith("newtonsoft.josn"))
                                {
                                    Assembly assembly2 = Assembly.Load(item);
                                    if (assembly2.GetCustomAttribute<ProxyTypesAssemblyAttribute>() != null)
                                    {
                                        return assembly2;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return null;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            PLuginContext pLuginContext = new PLuginContext(serviceProvider);
            var pluginExecutionContext = pLuginContext.PluginExecutionContext;
            string text = pluginExecutionContext.InputParameters["Api"]?.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("");
            }
            text = text.Trim();
            string[] arrary = text.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string value;

            if (arrary.Length != 2)
            {
                throw new Exception();
            }
            string ControllerName = arrary[0];
            string actionName = arrary[1];
            var GetApiControllerTypeResut = this.controllersFactory.GetApiControllerType(ControllerName);
            if (GetApiControllerTypeResut == null)
            {
                throw new Exception();
            }
            var actionDescriptor = GetApiControllerTypeResut.GetActionDescriptor(actionName);
            var Input = pluginExecutionContext.InputParameters["Input"]?.ToString();
            if (proxyTypesAssembly != null)
            {
                var proxyTypesAssemblyProvider = pluginExecutionContext as IProxyTypesAssemblyProvider;
                if (proxyTypesAssemblyProvider != null)
                {
                    proxyTypesAssemblyProvider.ProxyTypesAssembly = this.proxyTypesAssembly;
                }
            }
            value = ActionInvoke.IvnokeControllerAction(pLuginContext, actionDescriptor, Input);
            pluginExecutionContext.OutputParameters["Output"] = value;
        }
    }
}