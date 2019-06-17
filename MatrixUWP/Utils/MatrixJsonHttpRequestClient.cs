using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
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

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            var uri = new Uri(baseUri, path);
            var meta = $"GET {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string path, T body)
        {
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"POST {uri}";
            Debug.WriteLine($"Requesting: {meta}, with data {JsonSerializer.ToString(body)}");
            return await httpClient.PostAsync(uri, jsonContent);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string path, T body)
        {
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            var meta = $"PUT {uri}";
            Debug.WriteLine($"Requesting: {meta}, with data {JsonSerializer.ToString(body)}");
            return await httpClient.PutAsync(uri, jsonContent);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path)
        {
            var uri = new Uri(baseUri, path);
            string meta = $"DELETE {uri}";
            Debug.WriteLine($"Requesting: {meta}");
            return await httpClient.DeleteAsync(uri);
        }
    }
}
