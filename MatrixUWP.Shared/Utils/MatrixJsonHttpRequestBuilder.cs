#nullable enable
using System;
using Windows.ApplicationModel;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Shared.Utils
{
    public class MatrixJsonHttpRequestBuilder
    {
        private readonly HttpClient httpClient;
#if DEBUG
        private static readonly Uri baseUri = new Uri("https://staging.matrix.moe/");
#else
        private static readonly Uri baseUri = new Uri("https://matrix.sysu.edu.cn/");
#endif

        public MatrixJsonHttpRequestBuilder()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var protocolFilter = new HttpBaseProtocolFilter();
            var filter = new MatrixHttpFilter(protocolFilter); // Adds a custom header to every request and response message.
            httpClient = new HttpClient(filter);

            var version = Package.Current.Id.Version;
            var userAgent = $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36 Edg/84.0.522.59 UWP/{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            httpClient.DefaultRequestHeaders.Referer = baseUri;
            httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
        }

        public MatrixJsonHttpRequestClient Build() => new MatrixJsonHttpRequestClient(httpClient, baseUri);
    }
}
