using System;
using System.Linq;
using System.Reflection;
using ei8.Cortex.Coding.Properties;

namespace ei8.Cortex.Coding
{
    public class ExternalReference
    {
        public string Url { get; set; }
        public string Key { get; set; }

        public static string ToKeyString(Enum value) => value.ToString();

        public static string ToKeyString(MemberInfo value)
        {
            // get ExternalReferenceKeyAttribute of root type
            var erka = value.GetCustomAttributes<neurULKeyAttribute>().SingleOrDefault();
            string key;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else if (value is PropertyInfo pi)
                key = $"{ExternalReference.ToKeyString(pi.DeclaringType)}{Constants.TypeNamePropertyNameSeparator}{pi.Name}";
            else if (value is Type t)
                // assembly qualified name 
                key = Nullable.GetUnderlyingType(t) != null ? Nullable.GetUnderlyingType(t).FullName : t.FullName;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
            return key;
        }
    }
}
