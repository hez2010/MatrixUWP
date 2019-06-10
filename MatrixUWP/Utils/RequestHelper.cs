using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Utils
{
    public class RequestHelper
    {
        private static readonly HttpClient client = new HttpClient(new HttpBaseProtocolFilter
        {
            AllowAutoRedirect = false,
            AllowUI = true,
            AutomaticDecompression = true,
            CookieUsageBehavior = HttpCookieUsageBehavior.Default,
            MaxVersion = HttpVersion.Http20
        });

        private static readonly Uri baseUri = new Uri("https://vmatrix.org.cn/");

        public static async Task<T> GetJsonAsync<T>(string relativeUri, IDictionary<string, string>? @params)
        {
            var param = new StringBuilder();
            if (@params != null)
            {
                if (@params.Count > 0) param.Append("?");
                for (var i = 0; i < @params.Count; i++)
                {
                    var item = @params.ElementAt(i);
                    param.Append(Uri.EscapeDataString($"{item.Key}"));
                    param.Append("=");
                    param.Append(Uri.EscapeDataString($"{item.Value}"));
                    if (i != @params.Count - 1) param.Append("&");
                }
            }
            var result = await client.TryGetAsync(new Uri(baseUri, $"{relativeUri}{param.ToString()}"));
            if (result.Succeeded)
            {
                var str = await result.ResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(str);
            }
            return default;
        }
        public static async Task<bool> GetAsync(string relativeUri, IDictionary<string, string>? @params, IOutputStream output)
        {
            var param = new StringBuilder();
            if (@params != null)
            {
                if (@params.Count > 0) param.Append("?");
                for (var i = 0; i < @params.Count; i++)
                {
                    var item = @params.ElementAt(i);
                    param.Append(Uri.EscapeDataString($"{item.Key}"));
                    param.Append("=");
                    param.Append(Uri.EscapeDataString($"{item.Value}"));
                    if (i != @params.Count - 1) param.Append("&");
                }
            }
            var result = await client.TryGetAsync(new Uri(baseUri, $"{relativeUri}{param.ToString()}"));
            if (result.Succeeded)
            {
                await result.ResponseMessage.Content.WriteToStreamAsync(output);
            }
            return result.Succeeded;
        }

        public static async Task<T> PostJsonAsync<T>(string relativeUri, dynamic data)
        {
            var content = new HttpStringContent(JsonConvert.SerializeObject(data), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            var result = await client.TryPostAsync(new Uri(baseUri, relativeUri), content);

            if (result.Succeeded)
            {
                var str = await result.ResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(str);
            }

            return default;
        }
    }
}
