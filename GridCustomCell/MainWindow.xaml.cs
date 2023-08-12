using GridCustomCell.Models;
using GridCustomCell.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridCustomCell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            InitData();
            grid.ItemsSource = Items;
        }

        public List<InputReferenceItem> Items = new List<InputReferenceItem>();

        private void InitData()
        {
            for (int i = 0; i < 100; i++)
            {
                Items.Add(new InputReferenceItem()
                {
                    ReferenceColumnName = $"Name_{i}",
                    ReferenceValue = new Random(100).Next().ToString()
                });
            }
        }
    }
}
