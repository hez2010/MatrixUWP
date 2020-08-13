#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MatrixUWP.Shared.Extensions
{
    public static class ResponseExtensions
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings
            = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters =
                {
                    new StringEnumConverter()
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                MaxDepth = 1024,
                Error = (sender, args) =>
                {
#if FAIL_ON_DEBUG
                    Debug.Fail(args.ErrorContext.Error.Message, args.ErrorContext.Error.StackTrace);
#endif
                    args.ErrorContext.Handled = true;
                }
            };

        public static async ValueTask<T?> JsonAsync<T>(this ValueTask<HttpResponseMessage> response) where T : class
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
#if DEBUG
            Debug.WriteLine($"Got response: {response.Result.StatusCode}, with data: {json}, with headers: {response.Result.Headers.SerializeJson()}");
#endif
            return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
        }

        public static async ValueTask<string> TextAsync(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var text = await result.Content.ReadAsStringAsync();
#if DEBUG
            Debug.WriteLine($"Got response: {response.Result.StatusCode}, with data: {text}, with headers: {response.Result.Headers.SerializeJson()}");
#endif
            return text;
        }

        public static async ValueTask<IBuffer> BlobAsync(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var buffer = await result.Content.ReadAsBufferAsync();
#if DEBUG
            Debug.WriteLine($"Got response: {response.Result.StatusCode}, with data: [Blob], with headers: {response.Result.Headers.SerializeJson()}");
#endif
            return buffer;
        }
    }
}
