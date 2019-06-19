using MatrixUWP.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MatrixUWP.Utils
{
    class MatrixJsonHttpRequestClient
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        public MatrixJsonHttpRequestClient(HttpClient httpClient, Uri baseUri)
        {
            this.httpClient = httpClient;
            this.baseUri = baseUri;
        }

        public async ValueTask<HttpResponseMessage> GetAsync(string path)
        {
            var uri = new Uri(baseUri, path);
            var meta = $"GET {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.GetAsync(uri);
        }

        public async ValueTask<HttpResponseMessage> PostAsync<T>(string path, T body)
        {
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"POST {uri}";
            Debug.WriteLine($"Requesting: {meta}, with data {body.SerializeJson()}");
            return await httpClient.PostAsync(uri, jsonContent);
        }

        public async ValueTask<HttpResponseMessage> PutAsync<T>(string path, T body)
        {
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"PUT {uri}";
            Debug.WriteLine($"Requesting: {meta}, with data {body.SerializeJson()}");
            return await httpClient.PutAsync(uri, jsonContent);
        }

        public async ValueTask<HttpResponseMessage> DeleteAsync(string path)
        {
            var uri = new Uri(baseUri, path);
            string meta = $"DELETE {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.DeleteAsync(uri);
        }
    }
}
