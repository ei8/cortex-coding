using ei8.Cortex.Coding.Mirrors;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.Client
{
    /// <summary>
    /// Provides Out (Read) extension methods to the HttpClient class.
    /// </summary>
    public static class OutClientExtensions
    {
        /// <summary>
        /// Invokes the Get function of the client using the specified parameters (eg. bearerToken, content etc.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="bearerToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<(IEnumerable<IMirrorImageSeries<T>> Result, HttpResponseMessage Response)> AuthGetMirrorImageSeriesAsync<T>(
            this HttpClient client, 
            string requestUri, 
            string bearerToken,
            CancellationToken token
        )
            where T : IMirrorImage
        {
            if (!string.IsNullOrWhiteSpace(bearerToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await client.GetAsync(
                requestUri,
                token
            );

            IList<IMirrorImageSeries<T>> resultSeries = null;
            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                resultSeries = IMirrorImageSeriesExtensions.CreateList<T>();
                resultSeries.ReadJson(responseText);
            }

            return (resultSeries, response);
        }
    }
}
