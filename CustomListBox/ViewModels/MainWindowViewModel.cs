using CustomListBox.Models;
using DevExpress.Xpf.Core.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CustomListBox.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 历史记录信息
        /// </summary>
        public ObservableCollection<HistoryModel> HistoryCollection { get; set; }
        private string promtInfo;

        public string PromtInfo
        {
            get { return promtInfo; }
            set
            {
                if (promtInfo != value)
                {
                    promtInfo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PromtInfo"));
                }
            }
        }
        private string buttonContent;

        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                if (buttonContent != value)
                {
                    buttonContent = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ButtonContent"));
                }
            }
        }

        private HistoryModel currentSelectedItem;

        public HistoryModel CurrentSelectedItem
        {
            get { return currentSelectedItem; }
            set
            {
                if (currentSelectedItem != value)
                {
                    currentSelectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentSelectedItem"));
                }
            }
        }

        private bool isIdle = true;

        public bool IsIdle
        {
            get { return isIdle; }
            set {
                if (isIdle != value)
                {
                    isIdle = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsIdle"));
                }
                
            }
        }

        private bool isShowMarkText = true;

        public bool IsShowMarkText
        {
            get { return isShowMarkText; }
            set
            {
                if (isShowMarkText != value)
                {
                    isShowMarkText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsShowMarkText"));
                }

            }
        }


        [Obsolete]
        public DelegateCommand<object> CommitCommand { get; set; }

        [Obsolete]
        public DelegateCommand<object> ReGenerateCommitCommand { get; set; }
        [Obsolete]
        public DelegateCommand<object> TextChangedCommand { get; set; }

        private const string StartValue = "     ";

        [Obsolete]
        public MainWindowViewModel()
        {
            HistoryCollection = new ObservableCollection<HistoryModel>();
            CommitCommand = new DelegateCommand<object>(Commit);
            ReGenerateCommitCommand = new DelegateCommand<object>(ReGenerate);
            TextChangedCommand = new DelegateCommand<object>(TextChanged);
            ButtonContent = "0/2000 ";
            PromtInfo = StartValue;
            IsShowMarkText = PromtInfo == StartValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void Commit(object obj)
        {
            try
            {
                IsIdle = false;
                var item = new HistoryModel()
                {
                    Content = PromtInfo?.ToString().Trim(),
                    CreateTime = DateTime.Now.ToString("G"),
                    InProcessing = true,
                };
                HistoryCollection.Add(item);

                var listHistory = obj as ListBox;

                ///将光标光标滑动指向到最后一项
                listHistory.ScrollIntoView(item);
                CurrentSelectedItem = item;

                await TimeConsume(item);
            }
            catch (Exception ee)
            {

                throw ee;
            }
            finally
            {
                IsIdle = true;
            }
        }

        private async Task TimeConsume(HistoryModel item)
        {
            await Task.Factory.StartNew(() => { Thread.Sleep(3000); item.InProcessing = false; });
        }

        private void ReGenerate(object obj)
        {
            // 仅调用接口，不重新添加数据
            // focused change
            CurrentSelectedItem = obj as HistoryModel;
        }
        private void TextChanged(object obj)
        {
            if (PromtInfo != null)
            {
                if (!PromtInfo.StartsWith(StartValue))
                {
                    PromtInfo = $"{StartValue}{PromtInfo.TrimStart()}";
                }
                // 仅调用接口，不重新添加数据
                // focused change
                ButtonContent = $"{obj?.ToString().Length}/2000 ";

                IsShowMarkText = obj?.ToString() == StartValue;
            }
        }
    }
}
