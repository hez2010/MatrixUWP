using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Utils
{
    class MatrixJsonHttpRequestBuilder
    {
        private readonly HttpClient httpClient;
#if DEBUG
        internal static readonly Uri baseUri = new Uri("http://test.vmatrix.org.cn/");
#else
        internal static readonly Uri baseUri = new Uri("https://vmatrix.org.cn/");
#endif

        public MatrixJsonHttpRequestBuilder()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var filter = new MatrixHttpFilter(new HttpBaseProtocolFilter { CookieUsageBehavior = HttpCookieUsageBehavior.Default }); // Adds a custom header to every request and response message.
            this.httpClient = new HttpClient(filter);

            // 使用谷歌浏览器的用户代理
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3907.0 Safari/537.36 Edg/79.0.279.0";

            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            this.httpClient.DefaultRequestHeaders.Referer = baseUri;
            this.httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
        }

        public MatrixJsonHttpRequestClient Build() => new MatrixJsonHttpRequestClient(httpClient, baseUri);
    }
}
