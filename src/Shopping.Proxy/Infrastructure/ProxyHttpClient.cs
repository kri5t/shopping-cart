using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Shopping.Proxy.Infrastructure.Results;

namespace Shopping.Proxy.Infrastructure
{
    public class ProxyHttpClient
    {
        private readonly IProxyHttpClientConfiguration _configuration;

        internal ProxyHttpClient(IProxyHttpClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.ApiBaseUrl)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }
        
        private HttpContent Serialize(object body)
        {
            return body == null ? new StringContent(string.Empty) 
                                : new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8);
        }

        private async Task<(TModel, bool success, string errorMessage)> DeserializeResponse<TModel>(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return (JsonConvert.DeserializeObject<TModel>(content), true, string.Empty);
            }
            catch (Exception ex)
            {
                return (default(TModel), false, $"Failed to deserialize result: {ex.Message}");
            }
        }

        private async Task<ModelBaseResult<TModel>> MapResponseToResultWithModel<TModel>(HttpResponseMessage httpResponse)
        {
            var result = MapResponseToResult<ModelBaseResult<TModel>>(httpResponse);
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                var deserialization = await DeserializeResponse<TModel>(httpResponse);
                if(deserialization.success) {
                    result.Model = deserialization.Item1;
                }
            }
            return result;
        }
        
        private TResult MapResponseToResult<TResult>(HttpResponseMessage httpResponse) where TResult : BaseResult 
        {
            var result = Activator.CreateInstance<TResult>();
            result.HttpStatusCode = httpResponse.StatusCode;
            result.Message = httpResponse.ReasonPhrase;
            return result;
        }
        
        protected async Task<ModelBaseResult<TModel>> PostAsync<TModel>(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PostAsync(uri, Serialize(body));
                return await MapResponseToResultWithModel<TModel>(response);
            }
        }
        
        protected async Task<BaseResult> PostAsync(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PostAsync(uri, Serialize(body));
                return MapResponseToResult<BaseResult>(response);
            }
        }    
        
        protected async Task<ModelBaseResult<TModel>> PutAsync<TModel>(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PutAsync(uri, Serialize(body));
                return await MapResponseToResultWithModel<TModel>(response);
            }
        }
        
        protected async Task<BaseResult> PutAsync(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PutAsync(uri, Serialize(body));
                return MapResponseToResult<BaseResult>(response);
            }
        }    
        
        protected async Task<ModelBaseResult<TModel>> DeleteAsync<TModel>(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PutAsync(uri, Serialize(body));
                return await MapResponseToResultWithModel<TModel>(response);
            }
        }
        
        protected async Task<BaseResult> DeleteAsync(string uri, object body = null)
        {
            using (var client = GetHttpClient()){
                var response = await client.PutAsync(uri, Serialize(body));
                return MapResponseToResult<BaseResult>(response);
            }
        }    
        
        protected async Task<ModelBaseResult<TModel>> GetAsync<TModel>(string uri, Dictionary<string, string> queryParameters)
        {
            using (var client = GetHttpClient()){
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParameters));
                return await MapResponseToResultWithModel<TModel>(response);
            }
        }
    }
}