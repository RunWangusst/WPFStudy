using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebSocketLib
{
    public class WebSocketServerForClientToClient : WebSocketDemoBase
    {
        private HttpListener _listener;
        private ConcurrentDictionary<WebSocket, string> _connectedClients;

        public WebSocketServerForClientToClient()
        {
            _listener = new HttpListener();
            _connectedClients = new ConcurrentDictionary<WebSocket, string>();
        }

        public async Task Start(string uri)
        {
            _listener.Prefixes.Add(uri);
            _listener.Start();
            Console.WriteLine("Server started. Listening for incoming connections...");
            OnMessaging("Server started. Listening for incoming connections...");

            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    ProcessWebSocketRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async void ProcessWebSocketRequest(HttpListenerContext context)
        {
            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
            WebSocket socket = webSocketContext.WebSocket;

            //string clientId = context.Request.RemoteEndPoint.ToString();

            // 获取客户端标识（假设客户端在连接时发送了一个标识）
            string clientId = context.Request.QueryString["clientId"];
            string peerId = context.Request.QueryString["peerId"];
            if (string.IsNullOrWhiteSpace(clientId))
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
                return;
            }

            // 将客户端连接信息存储到字典中

            _connectedClients.TryAdd(socket, clientId);
            Console.WriteLine($"Client {clientId} connected.");
            OnMessaging($"Client {clientId} connected.");

            try
            {
                byte[] buffer = new byte[1024];
                while (socket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received from {clientId}: {message}");
                        OnMessaging($"Received from {clientId}: {message}");

                        // 解析消息，查找目标客户端并发送消息
                        await SendMessage(peerId, message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception from {clientId}: " + ex.Message);
                OnMessaging($"Exception from {clientId}: " + ex.Message);
            }
            finally
            {
                _connectedClients.TryRemove(socket, out _);
                OnMessaging($"Client {clientId} disconnected.");
                Console.WriteLine($"Client {clientId} disconnected.");
                if (socket != null)
                    socket.Dispose();
            }
        }

        private async Task SendMessage(string clientId, string message)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value == clientId)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await client.Key.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    byte[] buffer = Encoding.UTF8.GetBytes($"{clientId} is offline now.");
                    await client.Key.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        static async Task Main(string[] args)
        {
            string uri = "http://localhost:8080/ws/";
            var server = new WebSocketServer();
            await server.Start(uri);
        }
    }
}
