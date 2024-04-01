using MyWebSocketLib;
using MyWebSocketLib.Enums;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WebSocketsServer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        private WebSocketServerForClientToClient socketServer;
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

        private RunningStatus status;

        public RunningStatus Status
        {
            get { return status; }
            set { 
                status = value; 
                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }


        public DelegateCommand<object> StartOrStopServerCommand { get; set; }
        public MainWindowViewModel()
        {
            StartOrStopServerCommand = new DelegateCommand<object>(StartOrStop);
            Uri = "http://localhost:8080/ws/";  // 初始化值

        }

        private void StartOrStop(object obj)
        {
            textBox = obj as RichTextBox;
            Start(Uri);
        }

        private async void Start(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                Msg = "请输入 Uri 信息";
                return;
            }

            socketServer = new WebSocketServerForClientToClient();
            socketServer.MessagingCallback += UpdateMsg;
            await socketServer.Start(uri);
            Msg = "Server started....";
        }

        private void UpdateMsg(string callbackMsg)
        {
            Msg = callbackMsg;

            textBox?.AppendText(callbackMsg + "\r\n");
        }
    }
}
