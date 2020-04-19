#nullable enable
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace MatrixUWP.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static string SerializeJson<T>(this T obj, JsonSerializerSettings? options = null) => options == null ? JsonConvert.SerializeObject(obj) : JsonConvert.SerializeObject(obj, options);

        [return: MaybeNull]
        public static T DeserializeJson<T>(this string str, JsonSerializerSettings? options = null) => options == null ? JsonConvert.DeserializeObject<T>(str) : JsonConvert.DeserializeObject<T>(str, options);
    }
}
