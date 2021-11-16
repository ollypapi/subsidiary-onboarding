using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceBase
{
    public class HttpClientHelper
    {
        private string baseURL = string.Empty;
        private string accessTokenbaseURL = string.Empty;
        public HttpClientHelper()
        {
            //   baseURL = _configuration.GetValue<string>("BASE_URL");
        }
        public async Task<T> GetAsync<T>(string path, AuthenticationHeaderValue auth = default,
            Dictionary<string, string> headers = default)
        {
            var client = new HttpClient();
            if (auth != null)
                client.DefaultRequestHeaders.Authorization = auth;
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    client.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
            }

            var response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
                throw new CustomException("The request was not successful...");
            string result = response.Content.ReadAsStringAsync().Result;
            T returnValue = JsonConvert.DeserializeObject<T>(result);
            return returnValue;
        }


        public async Task<T> PostAsync<T, M>(M detail, string path, AuthenticationHeaderValue auth = default,
             Dictionary<string, string> headers = default)
        {
            var client = new HttpClient();
            // client.BaseAddress = new Uri(baseURL);
            var jsonContent = JsonConvert.SerializeObject(detail);
            var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            if (headers != null)
            {
                foreach (var h in headers)
                {
                    client.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
            }

            if (auth != null)
                client.DefaultRequestHeaders.Authorization = auth;

            Log.Information($"{jsonContent}");
            var response = await client.PostAsync(path, data);
            string result = response.Content.ReadAsStringAsync().Result;
            Log.Information($"{JsonConvert.SerializeObject(response)}");
            if (!response.IsSuccessStatusCode)
            {
                Log.Error(result, "[Error Response]");
                throw new CustomException("Unable to complete operation");
            }
            else
            {
                T returnValue = JsonConvert.DeserializeObject<T>(result);
                return returnValue;
            }
           
               

            
        }
        public async Task<T> PostAsync<T>(Dictionary<string, string> body, string path, AuthenticationHeaderValue auth = default,
               Dictionary<string, string> headers = default)
        {
            var client = new HttpClient();

            var data = new FormUrlEncodedContent(body);

            if (headers != null)
            {
                foreach (var h in headers)
                {
                    client.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
            }

            if (auth != null)
                client.DefaultRequestHeaders.Authorization = auth;

            var response = await client.PostAsync(path, data);
            string result = response.Content.ReadAsStringAsync().Result;

            T returnValue = JsonConvert.DeserializeObject<T>(result);
            return returnValue;
        }

    }
}
