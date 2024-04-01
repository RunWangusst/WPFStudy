using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebSocketLib
{
    public class WebSocketDemoBase
    {
        public delegate void MessageCallbackEventHandler(string msg);
        public event MessageCallbackEventHandler MessagingCallback;


        public virtual void OnMessaging(string msg)
        {
            MessagingCallback?.Invoke(msg);
        }

    }
}
