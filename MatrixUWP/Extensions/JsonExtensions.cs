#nullable enable
#if FXJSON
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace MatrixUWP.Extensions
{
    static class JsonExtensions
    {
#if FXJSON
        public static string SerializeJson<T>(this T obj, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.ToString(obj, options);
        }
#else
        public static string SerializeJson<T>(this T obj, JsonSerializerSettings? options = null)
        {
            if (options == null) return JsonConvert.SerializeObject(obj);
            return JsonConvert.SerializeObject(obj, options);
        }
#endif

#if FXJSON
        public static T DeserializeJson<T>(this string str, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Parse<T>(str, options);
        }
#else
        public static T DeserializeJson<T>(this string str, JsonSerializerSettings? options = null)
        {
            if (options == null) return JsonConvert.DeserializeObject<T>(str);
            return JsonConvert.DeserializeObject<T>(str, options);
        }
#endif
    }
}
