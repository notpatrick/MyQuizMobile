using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MyQuizMobile {
    public class Socket {
        //private const string HostAddress = "ws://h2653223.stratoserver.net/ws";
        private const string HostAddress = "ws://10.0.2.2:5000/ws";
        private const int ChunkSize = 1024;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        public CancellationToken Cts = CancellationToken.None;
        public ClientWebSocket WebSocket = new ClientWebSocket();
        private bool _finished;

        public Socket()
        { WebSocket.Options.SetBuffer(ChunkSize, ChunkSize); }

        public async Task Connect(int surveyId) {
            try {
                await WebSocket.ConnectAsync(new Uri($"{HostAddress}/{surveyId}"), Cts);
            } catch (Exception e) {
                logger.Error(e, "ReceiveLoop Exception");
            }
        }

        public async void ReceiveLoop(Action<string> callback) {
            
            // TODO : EXCEPTION IF IT RECEIVES MESSAGE AFTER NEW WEBOSCKET IS CREATED
            try {
                while (!_finished && WebSocket.State == WebSocketState.Open) {

                    var buffer = new ArraySegment<byte>(new byte[ChunkSize]);

                    WebSocketReceiveResult result;

                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            logger.Info("Received close message on Socket");
                            break;
                        } else if (result.MessageType == WebSocketMessageType.Text)
                        {
                            using (var reader = new StreamReader(ms, Encoding.UTF8)) {
                                var resultString = await reader.ReadToEndAsync();
                                callback(resultString);
                                
                                logger.Info("Received a message on Socket");

                            }
                        }


                    }


                    //var stringBuffer = System.Net.WebSockets.WebSocket.CreateClientBuffer(ChunkSize, ChunkSize);
                    //var result = await WebSocket.ReceiveAsync(stringBuffer, Cts);

                    //if (result.MessageType == WebSocketMessageType.Close) {
                    //    logger.Info("Received close message on Socket");
                    //    break;
                    //} else {
                    //    var resultString = _encoder.GetString(stringBuffer.Array, stringBuffer.Offset, result.Count);
                    //    if (!string.IsNullOrWhiteSpace(resultString)) {
                    //        callback(resultString);
                    //        logger.Info("Received a message on Socket");
                    //    }
                    //}
                }
            } catch (Exception e) {
                logger.Error(e, "ReceiveLoop Exception");
            }
        }

        public async void Close() {
            _finished = true;
            if(WebSocket != null && WebSocket.State == WebSocketState.Open)
                await WebSocket.SendAsync(new ArraySegment<byte>(new byte[ChunkSize]), WebSocketMessageType.Close, true, CancellationToken.None );
            //await WebSocket.CloseOutputAsync(WebSocketCloseStatus.EndpointUnavailable, "", CancellationToken.None);
            
        }
    }
}