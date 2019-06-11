using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace MatrixUWP.Utils
{
    class MatrixHttpRequest
    {
        private readonly HttpClient httpClient;
        public MatrixHttpRequest()
        {
            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            var filter = new PlugInFilter(new HttpBaseProtocolFilter()); // Adds a custom header to every request and response message.
            this.httpClient = new HttpClient(filter);

            // 使用谷歌浏览器的用户代理
            string userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.81 Safari/537.36";
            if (!this.httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent))
            {
                Debug.Fail("Failed to use Chrome User Agent");
            }

            this.httpClient.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            HttpResponseMessage response = null;
            string meta = $"GET {uri}";
            try
            {
                Debug.WriteLine($"Requesting: {meta}");
                response = await httpClient.GetAsync(uri);
            }
            catch (Exception e)
            {
                //throw new MatrixException.NetworkError(meta, e);
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(Uri uri, JObject body)
        {
            IHttpContent jsonContent = new HttpJsonContent(body);
            HttpResponseMessage response = null;
            string meta = $"POST {uri}";
            try
            {
                Debug.WriteLine($"Requesting: {meta}");
                Debug.WriteLine($"with body: {JsonConvert.SerializeObject(body, Formatting.Indented)}");
                response = await httpClient.PostAsync(uri, jsonContent);
            }
            catch (Exception e)
            {
                //throw new MatrixException.NetworkError(meta, e);
            }
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(Uri uri, JObject body)
        {
            IHttpContent jsonContent = new HttpJsonContent(body);
            HttpResponseMessage response = null;
            string meta = $"PUT {uri}";
            try
            {
                Debug.WriteLine($"Requesting: {meta}");
                Debug.WriteLine($"with body: {JsonConvert.SerializeObject(body, Formatting.Indented)}");
                response = await httpClient.PutAsync(uri, jsonContent);
            }
            catch (Exception e)
            {
                //throw new MatrixException.NetworkError(meta, e);
            }
            return response;
        }
    }
    class HttpJsonRequest : MatrixHttpRequest
    {

        private async Task<JObject> parseResponseAsJson(HttpResponseMessage response)
        {
            var text = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(text) as JObject;
        }
        public async new Task<JObject> GetAsync(Uri uri)
        {
            var response = await base.GetAsync(uri);
            return await parseResponseAsJson(response);
        }
        public async new Task<JObject> PostAsync(Uri uri, JObject body)
        {
            var response = await base.PostAsync(uri, body);
            return await parseResponseAsJson(response);
        }
        public async new Task<JObject> PutAsync(Uri uri, JObject body)
        {
            var response = await base.PutAsync(uri, body);
            return await parseResponseAsJson(response);
        }
    }

    // 我也是从网上 copy 下来的啊
    // 这个是改 headers 用的
    public sealed class PlugInFilter : IHttpFilter
    {
        private IHttpFilter innerFilter;

        public PlugInFilter(IHttpFilter innerFilter)
        {
            this.innerFilter = innerFilter ?? throw new ArgumentException("innerFilter cannot be null.");
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                Uri requestUri = request.RequestUri;

                HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
                HttpCookieCollection cookieCollection = filter.CookieManager.GetCookies(requestUri);

                var csrf = cookieCollection.FirstOrDefault(cookie => cookie.Name == "X-CSRF-Token");
                if (csrf != null)
                {
                    request.Headers.Add(csrf.Name, csrf.Value);
                    Debug.Write("csrf token added");
                }


                //Debug.WriteLine(text);
                //request.Headers.Add("Custom-Header", "CustomRequestValue");
                HttpResponseMessage response = await innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);
                cancellationToken.ThrowIfCancellationRequested();
                //response.Headers.Add("Custom-Header", "CustomResponseValue");
                return response;
            });
        }

        public void Dispose()
        {
            innerFilter.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    // 我也是从网上 copy 下来的啊
    // 这个大概是发 json 用的
    class HttpJsonContent : IHttpContent
    {
        JObject json;

        public HttpContentHeaderCollection Headers { get; }

        public HttpJsonContent(JObject json)
        {
            this.json = json;
            Headers = new HttpContentHeaderCollection
            {
                ContentType = new HttpMediaTypeHeaderValue("application/json")
                {
                    CharSet = "UTF-8"
                }
            };
        }

        public IAsyncOperationWithProgress<ulong, ulong> BufferAllAsync()
        {
            return AsyncInfo.Run<ulong, ulong>((cancellationToken, progress) =>
            {
                return Task.Run(() =>
                {
                    ulong length = GetLength();

                    // Report progress.
                    progress.Report(length);

                    // Just return the size in bytes.
                    return length;
                });
            });
        }

        public IAsyncOperationWithProgress<IBuffer, ulong> ReadAsBufferAsync()
        {
            return AsyncInfo.Run<IBuffer, ulong>((cancellationToken, progress) =>
            {
                return Task.Run(() =>
                {
                    DataWriter writer = new DataWriter();
                    writer.WriteString(JsonConvert.SerializeObject(json));

                    // Make sure that the DataWriter destructor does not free the buffer.
                    IBuffer buffer = writer.DetachBuffer();

                    // Report progress.
                    progress.Report(buffer.Length);

                    return buffer;
                });
            });
        }

        public IAsyncOperationWithProgress<IInputStream, ulong> ReadAsInputStreamAsync()
        {
            return AsyncInfo.Run<IInputStream, ulong>(async (cancellationToken, progress) =>
            {
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream);
                writer.WriteString(JsonConvert.SerializeObject(json));

                uint bytesStored = await writer.StoreAsync().AsTask(cancellationToken);

                // Make sure that the DataWriter destructor does not close the stream.
                writer.DetachStream();

                // Report progress.
                progress.Report(randomAccessStream.Size);

                return randomAccessStream.GetInputStreamAt(0);
            });
        }

        public IAsyncOperationWithProgress<string, ulong> ReadAsStringAsync()
        {
            return AsyncInfo.Run<string, ulong>((cancellationToken, progress) =>
            {
                return Task<string>.Run(() =>
                {
                    string jsonString = JsonConvert.SerializeObject(json);

                    // Report progress (length of string).
                    progress.Report((ulong)jsonString.Length);

                    return jsonString;
                });
            });
        }

        public bool TryComputeLength(out ulong length)
        {
            length = GetLength();
            return true;
        }

        public IAsyncOperationWithProgress<ulong, ulong> WriteToStreamAsync(IOutputStream outputStream)
        {
            return AsyncInfo.Run<ulong, ulong>(async (cancellationToken, progress) =>
            {
                DataWriter writer = new DataWriter(outputStream);
                writer.WriteString(JsonConvert.SerializeObject(json));
                uint bytesWritten = await writer.StoreAsync().AsTask(cancellationToken);

                // Make sure that DataWriter destructor does not close the stream.
                writer.DetachStream();

                // Report progress.
                progress.Report(bytesWritten);

                return bytesWritten;
            });
        }

        public void Dispose()
        {
            return;
        }

        private ulong GetLength()
        {
            DataWriter writer = new DataWriter();
            writer.WriteString(JsonConvert.SerializeObject(json));

            IBuffer buffer = writer.DetachBuffer();
            return buffer.Length;
        }
    }

}
