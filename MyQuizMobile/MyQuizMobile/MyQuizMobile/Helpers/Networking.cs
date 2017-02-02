using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MYQuizMobile;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(Networking))]

namespace MYQuizMobile {
    public class Networking {
        private const string HostAddress = "http://h2653223.stratoserver.net/";
        private const string ContentType = "application/json";

        private readonly HttpClient _client;

        public Networking() {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(HostAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
        }

        // path="api/groups/1"
        public async Task<T> Get<T>(string path) {
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(resultString);
                return result;
            }
            return default(T);
        }

        public async Task<T> Post<T>(string path, T value) {
            var serializedT = JsonConvert.SerializeObject(value);
            var response = await _client.PostAsync(path, new StringContent(serializedT, Encoding.UTF8, ContentType));

            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            return default(T);
        }
    }
}