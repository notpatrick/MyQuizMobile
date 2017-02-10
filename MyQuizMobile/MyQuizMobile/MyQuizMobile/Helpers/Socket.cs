using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class Socket {
        private const string HostAddress = "ws://h2653223.stratoserver.net/ws";
        //private const string HostAddress = "ws://10.0.2.2:5000/ws";
        private const int ChunkSize = 64 * 1024;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        private readonly CancellationTokenSource _tokenSource;
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();

        public Socket() {
            _tokenSource = new CancellationTokenSource();
            _webSocket.Options.SetBuffer(ChunkSize, ChunkSize);
        }

        public async Task Connect(int surveyId, int timeStamp) {
            try {
                Logger.Info("Socket trying to connect...");
                await _webSocket.ConnectAsync(new Uri($"{HostAddress}/{surveyId}/{timeStamp}"), _tokenSource.Token);
                Logger.Info("Socket connected.");
            } catch (Exception e) {
                Logger.Error(e, "Socket Connect Exception");
                throw;
            }
        }

        public async void ReceiveLoop(Action<string> callback) {
            try {
                while (true) {
                    if (_tokenSource.Token.IsCancellationRequested) {
                        Logger.Info("Token was canceled, break loop");
                        break;
                    }

                    var buffer = new ArraySegment<byte>(new byte[ChunkSize]);
                    var result = await _webSocket.ReceiveAsync(buffer, _tokenSource.Token);

                    if (result.MessageType == WebSocketMessageType.Close) {
                        Logger.Info("Server sent close, canceling token");
                        _tokenSource.Cancel();
                    } else {
                        var resultString = _encoder.GetString(buffer.Array, 0, result.Count);
                        callback(resultString);
                        Logger.Info($"Received a message on Socket, length {result.Count}");
                    }
                }
            } catch (OperationCanceledException) {
                Logger.Info("Operation canceled");
            } catch (Exception e) {
                Logger.Error(e, "Socket ReceiveLoop Exception");
                await Application.Current.MainPage.DisplayAlert("Ups!", "Exception im SocketHandler :(", "Ok");
            }
        }
    }
}