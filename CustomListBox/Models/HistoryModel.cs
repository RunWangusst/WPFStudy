using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomListBox.Models
{
    public class HistoryModel:INotifyPropertyChanged
    {
        public string Content { get; set; }
        public string CreateTime { get; set; }

        /// <summary>
        /// 是否正在请求中，默认值false
        /// </summary>
        private bool inProcessing;

        public bool InProcessing
        {
            get { return inProcessing; }
            set { 
                if(inProcessing != value)
                {
                    inProcessing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InProcessing"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
