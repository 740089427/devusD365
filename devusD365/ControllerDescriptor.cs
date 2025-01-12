using System;
using System.Collections.Generic;
using System.Reflection;

namespace devusD365
{
    public class ControllerDescriptor
    {
        private static readonly string ControllerPostfix = "Controller";
        public string ControllerName { get; private set; }
        public Type ControllerType { get; private set; }

        public Dictionary<string, ActionDescriptor> Actions { get; private set; }

        public ControllerDescriptor(Type controllerType,string controllerName)
        {
            ControllerName = controllerName;
            ControllerType = controllerType;
            Actions = new Dictionary<string, ActionDescriptor>(StringComparer.OrdinalIgnoreCase);
            MethodInfo[] methods = this.ControllerType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                ActionDescriptor actionDescriptor = new ActionDescriptor(this, method);
                this.Actions.Add(actionDescriptor.ActionName, actionDescriptor);
            }
        }

        public ActionDescriptor GetActionDescriptor(string actionName)
        {
            ActionDescriptor result;
            if (Actions.TryGetValue(actionName, out result))
            {
                return result;
            }
            throw new NotImplementedException();
        }

        public static string GetControllerNameFromType(Type controllerType)
        {
            string name = controllerType.Name;
            if (name.EndsWith(ControllerDescriptor.ControllerPostfix, StringComparison.OrdinalIgnoreCase))
            {
                return name.Substring(0, name.Length - ControllerPostfix.Length);
            }
            throw new Exception();
        }

    }
}
