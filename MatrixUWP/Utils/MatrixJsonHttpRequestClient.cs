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
        private bool tokenSaved = false;

        public MatrixJsonHttpRequestClient(HttpClient httpClient, Uri baseUri)
        {
            this.httpClient = httpClient;
            this.baseUri = baseUri;
        }

        private async ValueTask EnsureTokenSavedAsync()
        {
            if (tokenSaved) return;
            try
            {
                var uri = new Uri(baseUri, "/api/users/login");
                await httpClient.GetAsync(uri);
            }
            catch
            {
                return;
            }
            this.tokenSaved = true;
        }

        public async ValueTask<HttpResponseMessage> GetAsync(string path)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(baseUri, path);
            return await httpClient.GetAsync(uri);
        }

        public async ValueTask<HttpResponseMessage> PostAsync<T>(string path, T body)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            return await httpClient.PostAsync(uri, jsonContent);
        }

        public async ValueTask<HttpResponseMessage> PutAsync<T>(string path, T body)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(baseUri, path);
            var jsonContent = new HttpJsonContent<T>(body);
            return await httpClient.PutAsync(uri, jsonContent);
        }

        public async ValueTask<HttpResponseMessage> DeleteAsync(string path)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(baseUri, path);
            return await httpClient.DeleteAsync(uri);
        }
    }
}
