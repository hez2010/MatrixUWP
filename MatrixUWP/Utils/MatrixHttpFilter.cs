using System;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Storage.Streams;
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
}
