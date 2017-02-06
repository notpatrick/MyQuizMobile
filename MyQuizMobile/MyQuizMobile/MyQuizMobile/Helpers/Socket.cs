using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyQuizMobile
{
    public class Socket {
        private const string HostAddress = "ws://h2653223.stratoserver.net/ws";
        private const int ChunkSize = 4096;
        private bool _isReceiving = false;
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();

        public async Task Connect() {
            try {
                if (_webSocket.State == WebSocketState.Closed ||
                    _webSocket.State == WebSocketState.None) {
                    await _webSocket.ConnectAsync(new Uri(HostAddress), CancellationToken.None);
                }
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
        }

        public async Task Send(string message) {
            // message is the json string to send
            var buffer = _encoder.GetBytes(message);
            try {
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
        }

        // pass action that takes the result string
        //  Action<string> callback = s => { dologichere(); };
        public void StartReceiving(Action<string> callback) {
            if (!_isReceiving) {
                ReceiveLoop(callback);
            }
        }

        private async void ReceiveLoop(Action<string> callback) {
            _isReceiving = true;
            var stringBuffer = new byte[ChunkSize];
            try {
                while (_webSocket.State == WebSocketState.Open) {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(stringBuffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close) {
                        // close when server sends close message
                        await Disconnect();
                    } else {
                        var resultString = _encoder.GetString(stringBuffer, 0, result.Count);
                        callback(resultString);
                    }
                }
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
            _isReceiving = false;
        }

        public async Task Disconnect() {
            try {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            } catch (Exception) {
                // TODO: Handle exception
                throw;
            }
        }
    }
}