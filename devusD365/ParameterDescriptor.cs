using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace devusD365
{
    public class ParameterDescriptor
    {
        public ActionDescriptor ActionDescriptor { get; private set; }
        public ParameterInfo ParameterInfo { get; private set; }
        public string ParamName { get; private set; }
        public bool HasDefaultValue { get; private set; }
        public object DefaultValue { get; private set; }
        public bool IsOptional { get; private set; }
        public bool IsOut { get; private set; }

        public Type ParamType { get; private set; }
        public bool IsNullableOfT { get; private set; }
        public Type UnderlineType { get; set; }

        public ParameterDescriptor(ActionDescriptor action, ParameterInfo param)
        {
            ActionDescriptor = action;
            ParameterInfo = param;
            ParamName = param.Name;
            HasDefaultValue = param.HasDefaultValue;
            IsOptional = param.IsOptional;
            IsOut = param.IsOut;
            ParamType = param.ParameterType;
            IsNullableOfT = IsNullableTypeOfGeneric(param.ParameterType);
            UnderlineType = GetUnderlineType(param.ParameterType);
        }

        public static bool IsNullableTypeOfGeneric(Type type)
        {
            return type.IsGenericType && !type.IsGenericTypeDefinition
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Type GetUnderlineType(Type type)
        {
            if (IsNullableTypeOfGeneric(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        public object ConvertValue(JToken token)
        {
            if (token != null && token.Type != JTokenType.Null)
            {
                object result;
                try
                {
                    if (this.UnderlineType.IsEnum)
                    {
                        if (token.Type == JTokenType.String)
                        {
                            result = Enum.Parse(this.UnderlineType, token.Value<string>());
                        }
                        else
                        {
                            if (token.Type != JTokenType.Integer)
                            {
                                throw new Exception();
                            }
                            result = Enum.ToObject(this.UnderlineType, token.Value<int>());
                        }
                    }
                    else
                    {
                        object obj = token.ToObject(this.UnderlineType);
                        if (obj != null)
                        {
                            return obj;
                        }
                        if (!this.UnderlineType.IsAssignableFrom(obj.GetType()))
                        {
                            throw new ApplicationException("");
                        }
                        result = obj;
                    }
                }
                catch (Exception)
                {

                    throw new ApplicationException("");
                }
                return result;
            }
            if (this.IsNullableOfT)
            {
                return null;
            }
            throw new ApplicationException("");
        }
    }
}
