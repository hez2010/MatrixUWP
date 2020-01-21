#nullable enable
using System;
using Windows.ApplicationModel;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Utils
{
    class MatrixJsonHttpRequestBuilder
    {
        private readonly HttpClient httpClient;
#if DEBUG
        private static readonly Uri baseUri = new Uri("https://test.vmatrix.org.cn/");
#else
        private static readonly Uri baseUri = new Uri("https://vmatrix.org.cn/");
#endif

        public MatrixJsonHttpRequestBuilder()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var filter = new MatrixHttpFilter(new HttpBaseProtocolFilter()); // Adds a custom header to every request and response message.
            httpClient = new HttpClient(filter);

            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36 Edg/79.0.309.68";

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            httpClient.DefaultRequestHeaders.Referer = baseUri;
            httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
            httpClient.DefaultRequestHeaders.Add("RequestClient", "UWP");

            var version = Package.Current.Id.Version;
            httpClient.DefaultRequestHeaders.Add("RequestClientVersion", $"{ version.Major}.{ version.Minor}.{ version.Build}.{ version.Revision}");
        }

        public MatrixJsonHttpRequestClient Build()
        {
            return new MatrixJsonHttpRequestClient(httpClient, baseUri);
        }
    }
}
