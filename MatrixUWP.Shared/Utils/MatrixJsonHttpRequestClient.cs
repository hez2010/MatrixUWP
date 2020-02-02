#nullable enable
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace MatrixUWP.Shared.Utils
{
    public class MatrixJsonHttpRequestClient
    {
        private readonly HttpClient httpClient;
        public readonly Uri BaseUri;
        private bool tokenSaved = false;

        public MatrixJsonHttpRequestClient(HttpClient httpClient, Uri baseUri)
        {
            this.httpClient = httpClient;
            BaseUri = baseUri;
        }

        private async ValueTask EnsureTokenSavedAsync()
        {
            if (tokenSaved) return;
            try
            {
                var uri = new Uri(BaseUri, "/api/users/login");
                await httpClient.GetAsync(uri);
            }
            catch
            {
                return;
            }
            tokenSaved = true;
        }

        public async ValueTask<HttpResponseMessage> GetAsync(string path)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(BaseUri, path);
            return await httpClient.GetAsync(uri);
        }

        public async ValueTask<HttpResponseMessage> PostJsonAsync<T>(string path, T body)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(BaseUri, path);
            using var jsonContent = new HttpJsonContent<T>(body);
            return await httpClient.PostAsync(uri, jsonContent);
        }

        public async ValueTask<HttpResponseMessage> PostFileAsync(string path, string name, StorageFile file)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(BaseUri, path);
            using var multipartData = new HttpMultipartFormDataContent();
            using var content = new HttpBufferContent(await FileIO.ReadBufferAsync(file));
            content.Headers.ContentType = HttpMediaTypeHeaderValue.Parse(file.ContentType);
            multipartData.Add(content, name, file.Name);
            return await httpClient.PostAsync(uri, multipartData);
        }
    }
}
