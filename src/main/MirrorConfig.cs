using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding
{
    public class MirrorConfig
    {
        public string Url { get; set; }
        public IEnumerable<string> Keys { get; set; }

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
