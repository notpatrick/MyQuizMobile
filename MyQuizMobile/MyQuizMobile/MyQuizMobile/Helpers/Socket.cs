using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MyQuizMobile {
    public class Socket {
        private const string HostAddress = "ws://h2653223.stratoserver.net/ws";
        //private const string HostAddress = "ws://10.0.2.2:5000/ws";
        private const int ChunkSize = 64 * 1024;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        public CancellationTokenSource cts;
        public ClientWebSocket WebSocket = new ClientWebSocket();

        public Socket() { cts = new CancellationTokenSource(); }

        public async Task Connect(int surveyId) {
            try {
                await WebSocket.ConnectAsync(new Uri($"{HostAddress}/{surveyId}"), cts.Token);
            } catch (Exception e) {
                logger.Error(e, "ReceiveLoop Exception");
            }
        }

        public async void ReceiveLoop(Action<string> callback) {
            // TODO : EXCEPTION IF IT RECEIVES MESSAGE AFTER NEW WEBOSCKET IS CREATED
            try {
                while (WebSocket.State == WebSocketState.Open) {
                    var stringBuffer = new ArraySegment<byte>(new byte[ChunkSize]);
                    var result = await WebSocket.ReceiveAsync(stringBuffer, cts.Token);

                    if (result.MessageType == WebSocketMessageType.Close) {
                        logger.Info("Received close message on Socket");
                        break;
                    }

                    var resultString = _encoder.GetString(stringBuffer.Array, stringBuffer.Offset, result.Count);
                    if (string.IsNullOrWhiteSpace(resultString)) {
                        continue;
                    }
                    callback(resultString);
                    logger.Info("Received a message on Socket");
                }
            } catch (Exception e) {
                logger.Error(e, "ReceiveLoop Exception");
            }
        }

        public async Task Close() {
            cts.Cancel();
            await WebSocket.CloseOutputAsync(WebSocketCloseStatus.EndpointUnavailable, "", CancellationToken.None);
        }
    }
}