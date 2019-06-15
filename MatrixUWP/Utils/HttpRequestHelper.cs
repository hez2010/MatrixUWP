using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Utils
{
    class MatrixHttpFilter : IHttpFilter
    {
        private readonly IHttpFilter innerFilter;
        public MatrixHttpFilter(IHttpFilter filter)
        {
            innerFilter = filter ?? throw new ArgumentNullException(nameof(filter));
        }
        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var uri = request.RequestUri;

                var filter = new HttpBaseProtocolFilter();
                var cookieCollection = filter.CookieManager.GetCookies(uri);

                var csrf = cookieCollection.FirstOrDefault(cookie => cookie.Name == "X-CSRF-Token");
                if (csrf != null) request.Headers.Add(csrf.Name, csrf.Value);
                var response = await innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                return response;
            });
        }

        public void Dispose()
        {
            innerFilter.Dispose();
        }
    }

    class MatrixJsonHttpRequest
    {
        private readonly HttpClient httpClient;
        private bool inited;
        public static Uri BaseUri = new Uri("https://vmatrix.org.cn/");
        public MatrixJsonHttpRequest()
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

        public async Task InitAsync()
        {
            var uri = new Uri(BaseUri, "/api/users/login");
            Debug.WriteLine($"Initial Requesting: GET {uri}");
            await httpClient.GetAsync(uri);
            this.inited = true;
        }

        private async Task EnsureInitedAsync()
        {
            while (!this.inited) await Task.Delay(50);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            await EnsureInitedAsync();
            var meta = $"GET {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri uri, T body)
        {
            await EnsureInitedAsync();
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"POST {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.PostAsync(uri, jsonContent);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(Uri uri, T body)
        {
            await EnsureInitedAsync();
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"PUT {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.PutAsync(uri, jsonContent);
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri uri)
        {
            await EnsureInitedAsync();
            string meta = $"DELETE {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.DeleteAsync(uri);
        }
    }
}
