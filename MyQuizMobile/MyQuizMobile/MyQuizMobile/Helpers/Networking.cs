using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Xamarin.Forms;

namespace MYQuizMobile {
    public class Networking {
        private const string HostAddress = "http://h2653223.stratoserver.net/";
        //private const string HostAddress = "http://10.0.2.2:5000/";
        private const string ContentType = "application/json";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private HttpClient _client;

        public Networking(string deviceId) { Connect(deviceId); }

        private async void Connect(string deviceId) {
            try {
                logger.Info($"Creating connection with DeviceID = {deviceId}");
                _client = new HttpClient(new HttpClientHandler {UseProxy = false});
                _client.BaseAddress = new Uri(HostAddress);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
                _client.DefaultRequestHeaders.Add("DeviceID", deviceId);
            } catch (Exception e) {
                logger.Error(e, "Connect Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", $"Exception {e.Message}", "Ok");
            }
        }

        public async Task<T> Get<T>(string path) {
            var result = default(T);

            try {
                var response = await _client.GetAsync(path);
                if (!response.IsSuccessStatusCode) {
                    var msg = await response.Content.ReadAsStringAsync();
                    logger.Warn(msg);
                    return default(T);
                }
                var serializedResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(serializedResult);
                logger.Info("Successful GET");
            } catch (Exception e) {
                logger.Error(e, "Get Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", $"Exception while GETTING", "Ok");
            }
            return result;
        }

        public async Task<T> Post<T>(string path, T value) {
            var result = default(T);
            try {
                var serializedValue = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
                var response = await _client.PostAsync(path, new StringContent(serializedValue, Encoding.UTF8, ContentType));
                if (!response.IsSuccessStatusCode) {
                    var msg = await response.Content.ReadAsStringAsync();
                    logger.Warn(msg);
                    return default(T);
                }
                var serializedResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(serializedResult);
                logger.Info("Successful POST");
            } catch (Exception e) {
                logger.Error(e, "Post Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", $"Exception while POSTING", "Ok");
            }
            return result;
        }

        public async Task<T> Put<T>(string path, T value) {
            var result = default(T);
            try {
                var serializedValue = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
                var response = await _client.PutAsync(path, new StringContent(serializedValue, Encoding.UTF8, ContentType));
                if (!response.IsSuccessStatusCode) {
                    var msg = await response.Content.ReadAsStringAsync();
                    logger.Warn(msg);
                    return default(T);
                }
                var serializedResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(serializedResult);
                logger.Info("Successful PUT");
            } catch (Exception e) {
                logger.Error(e, "Post Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", $"Exception while PUTTING", "Ok");
            }
            return result;
        }

        public async Task<bool> Delete(string path) {
            var result = false;
            try {
                var response = await _client.DeleteAsync(path);
                result = response.IsSuccessStatusCode;
                if (result) {
                    logger.Info("Successful DELETE");
                } else {
                    var msg = await response.Content.ReadAsStringAsync();
                    logger.Warn(msg);
                }
            } catch (Exception e) {
                logger.Error(e, "Delete Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", $"Exception while DELETING", "Ok");
            }
            return result;
        }
    }
}