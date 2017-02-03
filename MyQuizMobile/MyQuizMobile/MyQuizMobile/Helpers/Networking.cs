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

        // path="api/groups/1"
        public async Task<T> Get<T>(string path) {
            var response = await _client.GetAsync(path);
            if (!response.IsSuccessStatusCode) {
                return default(T);
            }
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(resultString);
            return result;
        }

        public async Task<T> Post<T>(string path, T value) {
            var serializedT = JsonConvert.SerializeObject(value);
            var response = await _client.PostAsync(path, new StringContent(serializedT, Encoding.UTF8, ContentType));
            if (!response.IsSuccessStatusCode) {
                return default(T);
            }
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(resultString);
            return result;
        }

        public async Task<bool> Delete(string path) {
            var response = await _client.DeleteAsync(path);
            return response.IsSuccessStatusCode;
        }
    }
}