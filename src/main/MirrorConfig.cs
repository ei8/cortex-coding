using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ei8.Cortex.Coding.Properties;

namespace ei8.Cortex.Coding
{
    public class MirrorConfig
    {
        public string Url { get; set; }
        public string Key { get; set; }

        // TODO:1 transfer elsewhere so Key formulation can be dialect agnostic (ie. code below can be specific to d#)
        public static string ToKeyString(Enum value) => value.ToString();

        public static string ToKeyString(MemberInfo value)
        {
            // get neurULKeyAttribute of root type
            var erka = value.GetCustomAttributes<neurULKeyAttribute>().SingleOrDefault();
            string key;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else if (value is PropertyInfo pi)
                key = $"{MirrorConfig.ToKeyString(pi.DeclaringType)}{Constants.TypeNamePropertyNameSeparator}{pi.Name}";
            else if (value is Type t)
                // assembly qualified name 
                key = Nullable.GetUnderlyingType(t) != null ? Nullable.GetUnderlyingType(t).FullName : t.FullName;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
            return key;
        }

        public static bool TryProcessUrl(string neuronUrl, out string avatarUrl, out Guid id)
        {
            bool result = false;
            avatarUrl = null;
            if (Uri.TryCreate(neuronUrl, UriKind.Absolute, out Uri auri))
            {
                var match = Regex.Match(auri.AbsoluteUri, "(?<AvatarUrl>.*)\\/cortex\\/neurons\\/(?<Id>.*)?");
                if (match.Success)
                {
                    avatarUrl = match.Groups["AvatarUrl"].Value;
                    id = Guid.Parse(match.Groups["Id"].Value);

                    result = true;
                }
            }

            return result;
        }
    }
}
