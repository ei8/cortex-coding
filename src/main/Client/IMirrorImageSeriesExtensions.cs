using ei8.Cortex.Coding.Mirrors;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ei8.Cortex.Coding.Client
{
    /// <summary>
    /// Provides extension methods to IMirrorImageSeries.
    /// </summary>
    public static class IMirrorImageSeriesExtensions
    {
        /// <summary>
        /// Convert the specified enumerable IMirrorImageSeries to a JSON string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson<T>(this IEnumerable<IMirrorImageSeries<T>> value) where T : IMirrorImage
        {
            JsonSerializerOptions options = IMirrorImageSeriesExtensions.CreateResolver<T>();

            return JsonSerializer.Serialize(value, options);
        }

        /// <summary>
        /// Reads the specified JSON string into the IMirrorImageSeries list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="jsonString"></param>
        public static void ReadJson<T>(this IList<IMirrorImageSeries<T>> value, string jsonString) where T : IMirrorImage
        {
            JsonSerializerOptions options = IMirrorImageSeriesExtensions.CreateResolver<T>();

            var dj = JsonSerializer.Deserialize<IEnumerable<IMirrorImageSeries<T>>>(jsonString, options);

            foreach(var d in dj)
                value.Add(d);
        }

        /// <summary>
        /// Creates IMirrorImageSeries list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<IMirrorImageSeries<T>> CreateList<T>() where T : IMirrorImage => new List<IMirrorImageSeries<T>>();

        private static JsonSerializerOptions CreateResolver<T>() where T : IMirrorImage
        {
            return new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = {
                        (typeInfo) =>
                        {
                            if (typeInfo.Type == typeof(IMirrorImageSeries<T>))
                                typeInfo.CreateObject = () => new MirrorImageSeries<T>();
                        }
                    }
                }
            };
        }
    }
}
