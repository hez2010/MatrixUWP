#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace MatrixUWP.BackgroundService.Extensions
{
    static class JsonExtensions
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings
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
