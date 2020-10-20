using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using prometheus.model.gym;
// using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QGym.API.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        // private static readonly HttpClient Client = new HttpClient();
        // private HttpClient _httpClient;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IOptions<PaymentSettings> appSettings;
        // private string baseUri;

        // public HttpClientHelper(HttpClient httpClient, IOptions<PaymentSettings> appSettings)
        public HttpClientHelper(IOptions<PaymentSettings> appSettings)
        {
            // this._httpClient = httpClient;
            this.appSettings = appSettings;
        }
        /// <summary>
        /// Llamada Generica de un Post
        /// </summary>
        /// <typeparam name="TT">Respuesta</typeparam>
        /// <typeparam name="T">Request</typeparam>
        /// <param name="request"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<TT> PostAsync<TT, T>(T request, string uri, Dictionary<string, string> headers = null)
        {
            var content = CreateContent(request);
            Addheaders(headers);

            var response = await _httpClient.PostAsync(this.appSettings.Value.BaseUriPayment + uri, content);
            TT result = await GetResponse<TT>(response);

            return result;
        }

        public async Task<T> GetAsync<T>(string resquest, Dictionary<string, string> headers = null)
        {
            Addheaders(headers);
            var response = await _httpClient.GetAsync(this.appSettings.Value.BaseUriPayment + resquest);
            return await GetResponse<T>(response);
        }

        private static StringContent CreateContent<T>(T request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private static async Task<TT> GetResponse<TT>(HttpResponseMessage responst)
        {
            var contentRead = await responst.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TT>(contentRead);
            return result;
        }

        private void Addheaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }


    }
}
