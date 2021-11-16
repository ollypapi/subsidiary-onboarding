using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HttpClientHelper
    {
        public HttpClientHelper()
        {
            //   baseURL = _configuration.GetValue<string>("BASE_URL");
        }
        public async Task<T> PostClient<T, M>(M req, RestRequest request, string baseUrl)
        {
            var client = new RestClient(baseUrl);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(req, "application/json");
            // ServiceFunctions.BypassCertificateError();
            var reqObject = JsonConvert.SerializeObject(req);
            Log.Information($"REQUEST: {reqObject}");
            var resObj = await client.ExecuteAsync(request);

            if (resObj.StatusCode == HttpStatusCode.OK)
            {
                var resp = JsonConvert.DeserializeObject<T>(resObj.Content);
                //Log.Information($" {nameof(GetClient)} | SUCCESSFUL  | RequestObject {req} | ResponseObject {resObj.Content}");
                Log.Information($" {nameof(PostClient)} | SUCCESSFUL  | RequestObject {reqObject} | ResponseObject {resObj.Content}");

                return resp;
            }
            else
            {
                Log.Information($" {nameof(PostClient)}  | FAILED  | RequestObject {reqObject} | ResponseObject {resObj.Content}");

                return default;
            }

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
                throw new Exception("The request was not successful...");
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
            var data = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

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
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
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
