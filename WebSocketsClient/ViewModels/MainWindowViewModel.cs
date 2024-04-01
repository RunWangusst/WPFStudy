using MyWebSocketLib;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;

namespace WebSocketsClient.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private WebSocketClient socketsClient;
        private RichTextBox textBox;

        private string uri;

        public string Uri
        {
            get { return uri; }
            set
            {
                uri = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Uri"));
                }
            }
        }


        private string peerID;

        public string PeerID
        {
            get { return peerID; }
            set
            {
                peerID = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PeerID"));
                }
            }
        }

        private string clientID;

        public string ClientID
        {
            get { return clientID; }
            set
            {
                clientID = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ClientID"));
                }
            }
        }

        private string msg;

        public string Msg
        {
            get { return msg; }
            set
            {
                msg = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Msg"));
                }
            }
        }
        private string inputMsg;

        public string InputMsg
        {
            get { return inputMsg; }
            set
            {
                inputMsg = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("InputMsg"));
                }
            }
        }

        public DelegateCommand<object> ConnectCommand { get; set; }

        public DelegateCommand<object> DisConnectCommand { get; set; }
        public DelegateCommand<object> SendMessageCommand { get; set; }


        public MainWindowViewModel()
        {
            ConnectCommand = new DelegateCommand<object>(ConnectServer);
            DisConnectCommand = new DelegateCommand<object>(DisConnectServer);
            SendMessageCommand = new DelegateCommand<object>(SendMessage);
            Uri = "ws://localhost:8080/ws/";  // 初始化值
            ClientID = "wangrun";
        }


        private void SendMessage(object obj)
        {
            if (null != socketsClient)
            {
                socketsClient.Send(inputMsg);
            }
        }
        private void ConnectServer(object obj)
        {
            textBox = obj as RichTextBox;
            Start(Uri);
        }

        private void DisConnectServer(object obj)
        {
            socketsClient.Disconnect();
        }

        private async void Start(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                Msg = "请输入 Uri 信息";
                return;
            }

            socketsClient = new WebSocketClient();
            socketsClient.MessagingCallback += UpdateMsg;
            await socketsClient.ConnectAsync(uri, ClientID, PeerID);
            Msg = "Server started....";
        }

        private void UpdateMsg(string callbackMsg)
        {
            Msg = callbackMsg;

            textBox?.AppendText(callbackMsg + "\r\n");
        }
    }
}
