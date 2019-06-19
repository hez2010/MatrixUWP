using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MatrixUWP.Extensions
{
    static class ResponseExtensions
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true,
        };

        public static async ValueTask<T> JsonAsync<T>(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Parse<T>(json, options);
        }

        public static async ValueTask<string> TextAsync(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            return await result.Content.ReadAsStringAsync();
        }

        public static async ValueTask<IBuffer> BlobAsync(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var buffer = await result.Content.ReadAsBufferAsync();
            return buffer;
        }
    }
}
