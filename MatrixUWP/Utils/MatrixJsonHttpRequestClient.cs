#nullable enable
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MatrixUWP.Utils
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

        public async ValueTask<HttpResponseMessage> PostMultiPartAsync(string path, IDictionary<string, IHttpContent> contents)
        {
            await EnsureTokenSavedAsync();
            var uri = new Uri(BaseUri, path);
            using var multipartData = new HttpMultipartFormDataContent();
            foreach (var i in contents)
            {
                if (string.IsNullOrEmpty(i.Key)) multipartData.Add(i.Value);
                else multipartData.Add(i.Value, i.Key);
            }
            return await httpClient.PostAsync(uri, multipartData);
        }
    }
}
