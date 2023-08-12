using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CustomListBox.Converters
{
    public class IsLargeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SetValue((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置百分百，输入小数，自动乘100
        /// </summary>
        /// <param name="percentValue"></param>
        private bool SetValue(double angel)
        {
            ////数字显示
            //lbValue.Content = (percentValue * 100).ToString("0") + "%";

            /***********************************************
            * 整个环形进度条使用Path来绘制，采用三角函数来计算
            * 环形根据角度来分别绘制，以90度划分，方便计算比例
            ***********************************************/

            bool isLagreCircle = false; //是否优势弧，即大于180度的弧形

            //小于90度
            if (angel <= 90)
            {
                /*****************
                          *
                          *   *
                          * * ra
                   * * * * * * * * *
                          *
                          *
                          *
                ******************/

            }

            else if (angel <= 180)
            {
                /*****************
                          *
                          *  
                          * 
                   * * * * * * * * *
                          * * ra
                          *  *
                          *   *
                ******************/

            }

            else if (angel <= 270)
            {
                /*****************
                          *
                          *  
                          * 
                   * * * * * * * * *
                        * *
                       *ra*
                      *   *
                ******************/
                isLagreCircle = true; //优势弧
            }

            else if (angel < 360)
            {
                /*****************
                      *   *
                       *  *  
                     ra * * 
                   * * * * * * * * *
                          *
                          *
                          *
                ******************/
                isLagreCircle = true; //优势弧
            }
            else
            {
                isLagreCircle = true; //优势弧
            }

            return isLagreCircle;
        }
    }
}
