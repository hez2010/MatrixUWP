#nullable enable
using System;
#if FXJSON
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using System.Diagnostics;

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
        private static readonly JsonSerializerSettings jsonSerializerSettings
            = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        static ResponseExtensions()
        {
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        public static async ValueTask<T> JsonAsync<T>(this ValueTask<HttpResponseMessage> response)
        {
            var result = await response;
            var json = await result.Content.ReadAsStringAsync();
#if DEBUG
            Debug.WriteLine($"Got response: {response.Result.StatusCode}, with data: {json}, with headers: {response.Result.Headers.SerializeJson()}");
#endif
            return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
        }
#endif

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
