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

        private HttpClient _client;

        public Networking(string deviceId) { Connect(deviceId); }

        private void Connect(string deviceId) {
            try {
                _client = new HttpClient(new HttpClientHandler {UseProxy = false});
                _client.BaseAddress = new Uri(HostAddress);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
                _client.DefaultRequestHeaders.Add("DeviceID", deviceId);
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
        }

        public async Task<T> Get<T>(string path) {
            T result;

            try {
                var response = await _client.GetAsync(path);
                if (!response.IsSuccessStatusCode) {
                    return default(T);
                }
                var serializedResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(serializedResult);
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
            return result;
        }

        public async Task<T> Post<T>(string path, T value) {
            T result;
            try {
                var serializedValue = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
                var response = await _client.PostAsync(path, new StringContent(serializedValue, Encoding.UTF8, ContentType));
                if (!response.IsSuccessStatusCode) {
                    var msg = await response.Content.ReadAsStringAsync();
                    return default(T);
                }
                var serializedResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(serializedResult);
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }

        public async Task<bool> Delete(string path) {
            var result = false;
            try {
                var response = await _client.DeleteAsync(path);
                result = response.IsSuccessStatusCode;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }
    }
}