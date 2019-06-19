using System;
#if FXJSON
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MatrixUWP.Extensions
{
    static class ResponseExtensions
    {
#if FXJSON
        public static async ValueTask<T> JsonAsync<T>(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Parse<T>(json);
        }
#else
        public static async ValueTask<T> JsonAsync<T>(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
#endif

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
