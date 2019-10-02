﻿using System;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Utils
{
    class MatrixJsonHttpRequestBuilder
    {
        private readonly HttpClient httpClient;
#if DEBUG
        public static readonly Uri BaseUri = new Uri("https://test.vmatrix.org.cn/");
#else
        public static readonly Uri BaseUri = new Uri("https://vmatrix.org.cn/");
#endif

        public MatrixJsonHttpRequestBuilder()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var filter = new MatrixHttpFilter(new HttpBaseProtocolFilter()); // Adds a custom header to every request and response message.
            this.httpClient = new HttpClient(filter);

            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3907.0 Safari/537.36 Edg/79.0.279.0";

            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            this.httpClient.DefaultRequestHeaders.Referer = BaseUri;
            this.httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
        }

        public MatrixJsonHttpRequestClient Build() => new MatrixJsonHttpRequestClient(httpClient, BaseUri);
    }
}
