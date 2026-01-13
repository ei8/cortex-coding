using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Represents a Mirror Configuration.
    /// </summary>
    public class MirrorConfig
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Keys associated with the Mirror.
        /// </summary>
        public IEnumerable<string> Keys { get; set; }

        /// <summary>
        /// Extracts the avatarUrl and neuron ID from the specified URL.
        /// </summary>
        /// <param name="neuronUrl"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
