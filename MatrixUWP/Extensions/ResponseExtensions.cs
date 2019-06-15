using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MatrixUWP.Extensions
{
    static class ResponseExtensions
    {
        public static async Task<T> JsonAsync<T>(this Task<HttpResponseMessage> response)
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Parse<T>(json);
        }

        public static async Task<string> TextAsync(this Task<HttpResponseMessage> response)
        {
            var result = await response;
            return await result.Content.ReadAsStringAsync();
        }

        public static async Task<IBuffer> BlobAsync(this Task<HttpResponseMessage> response)
        {
            var result = await response;
            var buffer = await result.Content.ReadAsBufferAsync();
            return buffer;
        }
    }
}
