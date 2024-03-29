﻿using CustomListBox.ViewModels;
using CustomListBox.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace CustomListBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer m_Timer1 = new System.Windows.Threading.DispatcherTimer();

        double m_Percent = 0;
        bool m_IsStart = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            m_Timer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            m_Timer1.Tick += M_Timer1_Tick;
        }

        private void M_Timer1_Tick(object sender, EventArgs e)
        {
            m_Percent += 0.01;
            if (m_Percent > 1)
            {
                m_Percent = 1;
                m_Timer1.Stop();
                m_IsStart = false;
                StartChange(m_IsStart);
            }
            circleProgressBar.CurrentValue1 = m_Percent;
        }

        /// <summary>
        /// UI变化
        /// </summary>
        /// <param name="bState"></param>
        private void StartChange(bool bState)
        {
            if (bState)
                btnStart.Content = "停止";
            else
                btnStart.Content = "开始";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (m_IsStart)
            {
                m_Timer1.Stop();
                m_IsStart = false;

            }
            else
            {
                m_Percent = 0;
                m_Timer1.Start();
                m_IsStart = true;

            }
            StartChange(m_IsStart);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_Timer1.Stop();
        }

        private void testProgressBar_Click(object sender, RoutedEventArgs e)
        {
            TestProgressBar bar = new TestProgressBar();
            bar.ShowDialog();
        }

        private void testArc_Click(object sender, RoutedEventArgs e)
        {
            TestArc testArc = new TestArc();
            testArc.ShowDialog();
        }

        private void promptInfoTxt_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void promptInfoTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
