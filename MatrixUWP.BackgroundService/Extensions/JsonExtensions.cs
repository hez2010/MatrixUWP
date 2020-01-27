#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Extensions
{
    static class JsonExtensions
    {
        public static string SerializeJson<T>(this T obj, JsonSerializerSettings? options = null)
        {
            if (options == null) return JsonConvert.SerializeObject(obj);
            return JsonConvert.SerializeObject(obj, options);
        }
        public static T DeserializeJson<T>(this string str, JsonSerializerSettings? options = null)
        {
            if (options == null) return JsonConvert.DeserializeObject<T>(str);
            return JsonConvert.DeserializeObject<T>(str, options);
        }
    }
}
