using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MYQuizMobile {
    public class Networking {
        private const string HostAddress = "http://h2653223.stratoserver.net/";
        private const string ContentType = "application/json";

        private readonly HttpClient _client;

        public Networking() {
            _client = new HttpClient(new HttpClientHandler {
                UseProxy = false
            });
            _client.BaseAddress = new Uri(HostAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
        }

        public async Task<T> Get<T>(string path) {
            var response = await _client.GetAsync(path);
            if (!response.IsSuccessStatusCode) {
                return default(T);
            }
            var serializedResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(serializedResult);
            return result;
        }

        public async Task<T> Post<T>(string path, T value) {
            var serializedValue = JsonConvert.SerializeObject(value);
            var response = await _client.PostAsync(path, new StringContent(serializedValue, Encoding.UTF8, ContentType));
            if (!response.IsSuccessStatusCode) {
                return default(T);
            }
            var serializedResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(serializedResult);
            return result;
        }

        public async Task<bool> Delete(string path) {
            var response = await _client.DeleteAsync(path);
            return response.IsSuccessStatusCode;
        }
    }
}