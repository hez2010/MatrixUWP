#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MatrixUWP.BackgroundService.Extensions
{
    internal static class JsonExtensions
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

        public static string SerializeJson<T>(this T obj, JsonSerializerSettings? options = null) =>
            options == null ? JsonConvert.SerializeObject(obj) : JsonConvert.SerializeObject(obj, options);

        [return: MaybeNull]
        public static T DeserializeJson<T>(this string str, JsonSerializerSettings? options = null) =>
            options == null ? JsonConvert.DeserializeObject<T>(str) : JsonConvert.DeserializeObject<T>(str, options);
    }
}
