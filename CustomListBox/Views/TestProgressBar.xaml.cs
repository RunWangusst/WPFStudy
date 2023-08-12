using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CustomListBox.Views
{
    /// <summary>
    /// TestProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class TestProgressBar : Window
    {
        DispatcherTimer updateTimer;
        ProgressBar pb;

        public TestProgressBar()
        {
            InitializeComponent();
            InitPd();
        }

        private void InitPd()
        {
            // 此段代码防止在窗体 cs 文件的初始化构造函数即可
            // 此处代码仅仅做一个范例，你可以循环执行以下代码创建多个圆形进度条
            // 也可以通过 WEB SERVICE 获取所需的数据，然后循环创建进度条
            pb = new ProgressBar();
            var pd = new ProgressData();

            pb.Value = 240;
            pb.Style = (Style)FindResource("styleProgressBar");
            pb.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            pb.Background = new SolidColorBrush(Colors.Gray);

            pd.title = "XX001";
            pd.content = "线路板";
            pd.progress = "剩余 13 天";

            pb.DataContext = pd;
            uiProgressBars.Children.Add(pb);

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(10);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (pb == null)
                return;

            if (pb.Value < 360)
                pb.Value++;
            else
                pb.Value = 0;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            updateTimer = null;
        }
    }
    /// <summary>
    /// 声明圆形进度条的数据对象
    /// </summary>
    public class ProgressData
    {
        public string title { set; get; }
        public string content { set; get; }
        public string progress { set; get; }
    }

}
