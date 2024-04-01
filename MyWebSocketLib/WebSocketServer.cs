using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebSocketLib
{
    public class WebSocketServer: WebSocketDemoBase
    {
        private HttpListener _listenser;

        public WebSocketServer()
        {

        }

        public async Task Start(string uri)
        {
            _listenser = new HttpListener();
            _listenser.Prefixes.Add(uri);
            _listenser.Start();

            OnMessaging("Server started. Listening for incoming connections...");
            Console.WriteLine("Server started. Listening for incoming connections...");

            while (true)
            {
                HttpListenerContext context = await _listenser.GetContextAsync();

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

            try
            {
                byte[] buffer = new byte[1024];
                while (socket.State == WebSocketState.Open)
                {
                    // 发送心跳消息
                    //await Task.Delay(5000);  // 每5秒发送一次心跳
                    //await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Ping")), WebSocketMessageType.Text, true, CancellationToken.None);

                    // 接收消息
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        OnMessaging($"Received from client: {message}");
                        Console.WriteLine($"Received from client: {message}");

                        // echo message back to client
                        await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                OnMessaging("Exception: " + ex.Message);
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Dispose();
                }
            }
        }


    }
}
