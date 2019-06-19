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
        private readonly Uri baseUri = new Uri("http://test.vmatrix.org.cn/");
#else
        private readonly Uri baseUri = new Uri("https://vmatrix.org.cn/");
#endif

        public MatrixJsonHttpRequestBuilder()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var filter = new MatrixHttpFilter(new HttpBaseProtocolFilter()); // Adds a custom header to every request and response message.
            this.httpClient = new HttpClient(filter);

            // 使用谷歌浏览器的用户代理
            var userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.81 Safari/537.36";
            if (!this.httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent))
            {
                Debug.Fail("Failed to use Chrome User Agent");
            }

            this.httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
        }

        public MatrixJsonHttpRequestClient Build() => new MatrixJsonHttpRequestClient(httpClient, baseUri);
    }
}
