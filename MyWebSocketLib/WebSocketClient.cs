using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebSocketLib
{
    public class WebSocketClient: WebSocketDemoBase
    {
        // 客户端 WebSocket 实例
        private ClientWebSocket _clientWebSocket;
        // 控制是否取消连接
        private CancellationTokenSource _cancellationTokenSource;


        private string _clientId;  // 用于记录客户端标识

        // 标识是否需要重连
        private bool _shouldConnect;

        public WebSocketClient()
        {
            _clientWebSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            _shouldConnect = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="clientId">自身id</param>
        /// <param name="peerId">对方id</param>
        /// <returns></returns>
        public async Task ConnectAsync(string uri, string clientId,string peerId)
        {
            while (_shouldConnect)
            {
                try
                {
                    _clientId = clientId;
                    uri = $"{uri}?clientId={clientId}&peerId={peerId}";
                    // 连接到服务端
                    await _clientWebSocket.ConnectAsync(new Uri(uri), _cancellationTokenSource.Token);
                    OnMessaging("Connected to server");
                    Console.WriteLine($"{_clientId} Connected to server");

                    Console.WriteLine("Local endpoint: " + _clientWebSocket.Options);
                    OnMessaging("Connected to server");

                    // 等待接受服务端响应
                    await Recevie();
                    
                }
                catch (Exception ex)
                {
                    OnMessaging($"Error connecting to server: {ex.Message}");
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                    _shouldConnect=false;
                }
            }
        }

        private async Task Recevie()
        {
            byte[] buffer = new byte[1024];
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult webSocketReceiveResult = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                if (webSocketReceiveResult != null && webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, webSocketReceiveResult.Count);
                    OnMessaging($"Received message: {message}");
                    Console.WriteLine($"Received message: {message}");
                }
            }
        }

        public async Task Send(string message)
        {
            message = $"{_clientId}:{message}";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer),WebSocketMessageType.Text,true,_cancellationTokenSource.Token);
        }

        public async Task Disconnect()
        {
            _shouldConnect = true;
            _cancellationTokenSource.Cancel();
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", _cancellationTokenSource.Token);
        }


    }
}
