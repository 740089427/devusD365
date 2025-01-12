using System;
using System.Collections.Generic;
using System.Reflection;

namespace devusD365
{
    public class ActionDescriptor
    {
        public ControllerDescriptor controllerDescriptor { get; private set; }
        public string ActionName { get; private set; }
        public MethodInfo Method { get; private set; }
        public List<ParameterDescriptor> Params { get; private set; }
        public Dictionary<string, ParameterDescriptor> ParamDict { get; private set; }
        public Type ReturnType { get; private set; }
        public string FullPath { get; private set; }

        public ActionDescriptor(ControllerDescriptor controller, MethodInfo method)
        {
            controllerDescriptor = controller;
            ActionName = method.Name;
            Method = method;
            FullPath = controllerDescriptor.ControllerName + "/" + ActionName;
            ReturnType = method.ReturnType;
            Params = new List<ParameterDescriptor>();
            ParamDict = new Dictionary<string, ParameterDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in method.GetParameters())
            {
                ParameterDescriptor parameterDescriptor = new ParameterDescriptor(this, item);
                Params.Add(parameterDescriptor);
                ParamDict.Add(parameterDescriptor.ParamName, parameterDescriptor);
            }
        }
    }
}
