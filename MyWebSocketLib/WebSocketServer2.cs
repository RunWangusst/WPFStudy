using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebSocketLib
{
    public class WebSocketServer2: WebSocketDemoBase
    {
        private HttpListener _listener;
        private ConcurrentDictionary<WebSocket, string> _connectedClients;

        public WebSocketServer2()
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

            string clientId = context.Request.RemoteEndPoint.ToString();
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

                        // 根据客户端请求回复相应消息
                        string responseMessage = GetResponseMessage(message, clientId);
                        await SendMessage(socket, responseMessage);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                OnMessaging($"Exception from {clientId}: " + ex.Message);
                Console.WriteLine($"Exception from {clientId}: " + ex.Message);
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

        private string GetResponseMessage(string requestMessage, string clientId)
        {
            // 根据不同的客户端请求，返回相应的消息
            // 这里可以根据业务逻辑进行处理，例如根据请求消息生成回复消息
            return $"Response to {clientId}: {requestMessage.ToUpper()}";
        }

        private async Task SendMessage(WebSocket socket, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        static async Task Main(string[] args)
        {
            string uri = "http://localhost:8080/ws/";
            var server = new WebSocketServer();
            await server.Start(uri);
        }
    }

}