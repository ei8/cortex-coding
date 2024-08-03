using System;
using System.Reflection;

namespace ei8.Cortex.Coding
{
    public class ExternalReference
    {
        public string Url { get; set; }
        public string Key { get; set; }

        public static string ToKeyString(Type value) => Nullable.GetUnderlyingType(value) != null ? Nullable.GetUnderlyingType(value).FullName : value.FullName;
        public static string ToKeyString(Enum value) => value.ToString();
        public static string ToKeyString(PropertyInfo property) =>
            $"{ExternalReference.ToKeyString(property.DeclaringType)}{Constants.TypeNamePropertyNameSeparator}{property.Name}";
    }
}
