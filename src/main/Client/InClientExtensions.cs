using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.Client
{
    /// <summary>
    /// Provides In (Write) extension methods to the HttpClient class.
    /// </summary>
    public static class InClientExtensions
    {
        /// <summary>
        /// Invokes the Post function of the client using the specified parameters (eg. bearerToken, content etc.)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="bearerToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> AuthPostAsync(
            this HttpClient client,
            string requestUri,
            string content,
            string bearerToken,
            CancellationToken token
        )
        {
            if (!string.IsNullOrWhiteSpace(bearerToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var sc = new StringContent(content);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await client.PostAsync(
               requestUri,
               sc,
               token
            );
        }
    }
}
