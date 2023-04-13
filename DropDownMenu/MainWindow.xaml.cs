using DropDownMenu.CustomControls;
using DropDownMenu.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace DropDownMenu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            var menuRegister = new List<SubItem>
            {
                new SubItem("客户", new UserControlCustomers()),
                new SubItem("供应商", new UserControlProviders()),
                new SubItem("员工"),
                new SubItem("产品")
            };
            var item6 = new ItemMenu("登记", menuRegister, PackIconKind.Register);

            var menuSchedule = new List<SubItem>
            {
                new SubItem("服务"),
                new SubItem("会议")
            };
            var item1 = new ItemMenu("预约", menuSchedule, PackIconKind.Schedule);

            var menuReports = new List<SubItem>
            {
                new SubItem("客户"),
                new SubItem("供应商"),
                new SubItem("产品"),
                new SubItem("库存"),
                new SubItem("销售额")
            };
            var item2 = new ItemMenu("报告", menuReports, PackIconKind.FileReport);

            var menuExpenses = new List<SubItem>
            {
                new SubItem("固定资产"),
                new SubItem("流动资金")
            };
            var item3 = new ItemMenu("费用", menuExpenses, PackIconKind.ShoppingBasket);

            var menuFinancial = new List<SubItem>();
            menuFinancial.Add(new SubItem("现金流"));
            var item4 = new ItemMenu("财务", menuFinancial, PackIconKind.ScaleBalance);

            Menu.Children.Add(new UserControlMenuItem(item6, this));
            Menu.Children.Add(new UserControlMenuItem(item1, this));
            Menu.Children.Add(new UserControlMenuItem(item2, this));
            Menu.Children.Add(new UserControlMenuItem(item3, this));
            Menu.Children.Add(new UserControlMenuItem(item4, this));
        }
        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);

            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }

    }
}
