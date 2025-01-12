using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace devusD365
{
    public static class ActionInvoke
    {
        public static   object[] EmptyObjectArrary = new object[0];
        public static object[] ParseToParams(ActionDescriptor actionDescriptor, string input)
        {
            object[] arrary;
            if (actionDescriptor.Params.Count > 0)
            {
                arrary = new object[actionDescriptor.Params.Count];
                JObject jobject;
                if (string.IsNullOrWhiteSpace(input) || input.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    jobject = new JObject();
                }
                else
                {
                    jobject = JObject.Parse(input);
                }
                for (int i = 0; i < actionDescriptor.Params.Count; i++)
                {
                    ParameterDescriptor parameterDescriptor = actionDescriptor.Params[i];
                    JToken jToken;


                    if (!jobject.TryGetValue(parameterDescriptor.ParamName, StringComparison.OrdinalIgnoreCase, out jToken))
                    {
                        jToken = null;
                    }
                    if (jToken == null || jToken.Type == JTokenType.Null)
                    {
                        if (parameterDescriptor.HasDefaultValue)
                        {
                            arrary[i] = parameterDescriptor.DefaultValue;
                        }
                        else
                        {
                            if (!parameterDescriptor.ParamType.IsValueType)
                            {
                                arrary[i] = null;
                            }
                            else
                            {
                                if (parameterDescriptor.IsNullableOfT)
                                {
                                    throw new InvalidOperationException();
                                }
                                arrary[i] = null;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            arrary[i] = parameterDescriptor.ConvertValue(jToken);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            else
            {
                arrary = EmptyObjectArrary;
            }
            return arrary;
        }

        public static string IvnokeControllerAction(PLuginContext pLugin_context, ActionDescriptor actionDescriptor, string input)
        {
            if (pLugin_context == null)
            {
                throw new Exception();
            }
            if (actionDescriptor == null)
            {
                throw new Exception();
            }
            object[] paramters = ParseToParams(actionDescriptor, input);
            HiddenApiControler hiddenApiControler = (HiddenApiControler)Activator.CreateInstance(actionDescriptor.controllerDescriptor.ControllerType);
            string result;
            try
            {
                hiddenApiControler.InitializeContext(pLugin_context);
                if (actionDescriptor.ReturnType == null || actionDescriptor.ReturnType == typeof(void))
                {
                    try
                    {
                        actionDescriptor.Method.Invoke(hiddenApiControler, paramters);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    result = null;
                }
                else
                {
                    object value;
                    try
                    {
                        value = actionDescriptor.Method.Invoke(hiddenApiControler, paramters);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    result = JsonConvert.SerializeObject(value);
                }
            }
            finally
            {

            }
            return result;
        }

    }
}