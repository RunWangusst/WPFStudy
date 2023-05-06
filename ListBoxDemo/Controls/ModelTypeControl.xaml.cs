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

namespace ListBoxDemo.Controls
{
    /// <summary>
    /// ModelTypeControl.xaml 的交互逻辑
    /// </summary>
    public partial class ModelTypeControl : UserControl
    {
        public ModelTypeControl()
        {
            InitializeComponent();
        }


        private Uri imageSource;
        public Uri ImageSource
        {
            get { return imageSource; }
            set { 
                if(value != imageSource)
                {
                    img.Source = value;
                }
                imageSource = value;
            }
        }

        private string modelType;
        public string ModelType
        {
            get { return modelType; }
            set
            {
                if (value != modelType)
                {
                    modelTypeTxt.Text = value;
                }
                modelType = value;
            }
        }
    }
}
