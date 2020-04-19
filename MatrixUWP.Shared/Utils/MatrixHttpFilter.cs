#nullable enable
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MatrixUWP.Shared.Utils
{
    public class MatrixHttpFilter : IHttpFilter
    {
        private readonly IHttpFilter innerFilter;
        public MatrixHttpFilter(IHttpFilter filter)
        {
            innerFilter = filter;
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request) => AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
                                                                                                                                        {
                                                                                                                                            cancellationToken.ThrowIfCancellationRequested();

                                                                                                                                            var uri = request.RequestUri;

                                                                                                                                            var filter = new HttpBaseProtocolFilter();
                                                                                                                                            var cookieCollection = filter.CookieManager.GetCookies(uri);

                                                                                                                                            var csrf = cookieCollection.FirstOrDefault(cookie => cookie.Name == "X-CSRF-Token");
                                                                                                                                            if (csrf != null) request.Headers.Add(csrf.Name, csrf.Value);

                                                                                                                                            var response = await innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);
                                                                                                                                            Debug.WriteLine(
                                                                                                                                                $"Sent request: {request.Method.Method} {uri}, with data: {(request.Content == null ? "null" : await request.Content.ReadAsStringAsync())}, with headers: {JsonConvert.SerializeObject(request.Headers)}");
                                                                                                                                            return response;
                                                                                                                                        });

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    innerFilter.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
