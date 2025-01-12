using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace devusD365
{
    //public interface IHiddenApiControler : IDisposable
    //{
    //    void InitializeContext(ICommand command);
    //}
    public class publicControllerFactory
    {

        private   Assembly controllersAssembly;
        private   ThreadSafeLazy<Dictionary<string, ControllerDescriptor>> controllerTypes;
        public publicControllerFactory(Assembly controllers_assembly)
        {
            controllersAssembly = controllers_assembly;
            controllerTypes = new ThreadSafeLazy<Dictionary<string, ControllerDescriptor>>
                (new Func<Dictionary<string, ControllerDescriptor>>(CreateControllerTypeCache));
        }

        private Dictionary<string, ControllerDescriptor> CreateControllerTypeCache()
        {
            Dictionary<string, ControllerDescriptor> dictionary = new Dictionary<string, ControllerDescriptor>();
            Assembly assembly = this.controllersAssembly;
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            Type[] array = types;
            foreach (Type type in types)
            {
                try
                {
                    if (!type.IsAbstract && type.GetInterfaces().FirstOrDefault()?.Name == typeof(IHiddenApiControler)?.Name)
                    {
                        string controllerNameFromType = ControllerDescriptor.GetControllerNameFromType(type);
                        ControllerDescriptor value = new ControllerDescriptor(type, controllerNameFromType);
                        if (dictionary.ContainsKey(controllerNameFromType))
                        {
                            ControllerDescriptor controllerDescriptor = dictionary[controllerNameFromType];
                        }
                        dictionary.Add(controllerNameFromType, value);
                    }

                }
                catch (Exception)
                {
                    throw;
                }

            }
            return dictionary;
        }

        public ControllerDescriptor GetApiControllerType(string key)
        {
            ControllerDescriptor result;
            if (controllerTypes.Value.TryGetValue(key, out result))
            {
                return result;
            }
            throw new Exception("");
        }
    }
}