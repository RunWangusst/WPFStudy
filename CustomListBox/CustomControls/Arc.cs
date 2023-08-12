using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CustomListBox.CustomControls
{
    public class Arc : Shape
    {
        static DispatcherTimer dispatcherTimer;

        public Arc()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dis = sender as DispatcherTimer;
            if (null == dis)
                return;
            var arc = dis.Tag as Arc;
            if (null == arc)
                return;
            if (arc.EndAngle < 360)
            {
                arc.EndAngle += 2;
            }
            else
            {
                arc.EndAngle = 0;
            }
        }

        private Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(Arc), new PropertyMetadata((Geometry)Geometry.Parse(""), (s, e) =>
            {
                if (s is Arc arc && e.NewValue.ToString() != e.OldValue.ToString())
                {
                    arc.InvalidateVisual();
                }
            }));

        public Rect Rect
        {
            get { return (Rect)GetValue(RectProperty); }
            set { SetValue(RectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectProperty =
            DependencyProperty.Register("Rect", typeof(Rect), typeof(Arc), new PropertyMetadata(default(Rect), propertyChangedCallback));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new PropertyMetadata(0.0, propertyChangedCallback));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new PropertyMetadata(0.0, propertyChangedCallback));



        public bool IsBeginStory
        {
            get { return (bool)GetValue(IsBeginStoryProperty); }
            set { SetValue(IsBeginStoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBeginStory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBeginStoryProperty =
            DependencyProperty.Register("IsBeginStory", typeof(bool), typeof(Arc), new PropertyMetadata(false, BeginStory));

        private static void BeginStory(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Arc arc)
            {
                if (arc.IsBeginStory)
                {
                    dispatcherTimer.Tag = arc;
                    if (!dispatcherTimer.IsEnabled)
                        dispatcherTimer.Start();
                }
                else
                {
                    dispatcherTimer.Stop();
                    arc.EndAngle = 0;
                }
            }
        }


        protected override Geometry DefiningGeometry { get { return (Geometry)GetValue(DataProperty); } }

        private static void propertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Arc arc)
            {
                // 判断绘制的圆弧是否可见，以提高性能
                if (arc.Rect.Width > 0 && arc.Rect.Height > 0 && arc.StartAngle != arc.EndAngle)
                {
                    double ellipseA = arc.Rect.Width / 2;
                    double ellipseB = arc.Rect.Height / 2;

                    // 起止角度间隔大于等于360°，直接画圆

                    if (Math.Abs(arc.StartAngle - arc.EndAngle) >= 360)
                    {
                        string data = $"M{arc.Rect.X + ellipseA * 2},{arc.Rect.Y + ellipseB + 0.001} A{ellipseA},{ellipseB} 0,1,1 {arc.Rect.X + ellipseA * 2},{arc.Rect.Y + ellipseB}z";
                        arc.Data = (Geometry)Geometry.Parse(data);
                        return;
                    }

                    // 椭圆公式：X²/a²+Y²/b²=1
                    // Rect与椭圆各参数的对应关系：
                    // Rect.X与Rect.Y分别是椭圆外接矩形相对Arc区域的左上角的偏移量;
                    // Rect.Width与Rect.Height分别是椭圆外接矩形的宽和高
                    // 以下根据StartAngle和EndAngle算出圆弧在矩形函数曲线中的起始点和终止点

                    // 判断绘制方向
                    bool clockWise = arc.EndAngle > arc.StartAngle;

                    // 判断优弧/劣弧
                    bool majorArc = Math.Abs(arc.StartAngle - arc.EndAngle) % 360 >= 180;

                    // 将起始点和终止点转化为椭圆上的坐标
                    double rad = 0;

                    Point startPoint = new Point();
                    if ((90 - arc.StartAngle) % 360 == 0)
                    {
                        startPoint.X = 0;
                        startPoint.Y = ellipseB;
                    }
                    else if ((270 - arc.StartAngle) % 360 == 0)
                    {
                        startPoint.X = 0;
                        startPoint.Y = -ellipseB;
                    }
                    else
                    {
                        rad = GetRadian(arc.StartAngle);
                        startPoint.X = ellipseA * ellipseB / Math.Sqrt(Math.Pow(ellipseB, 2) + Math.Pow(ellipseA * Math.Tan(rad), 2));
                        startPoint.X *= Math.Cos(rad) > 0 ? 1 : -1;
                        startPoint.Y = startPoint.X * Math.Tan(rad);
                    }

                    Point endPoint = new Point();
                    if ((90 - arc.EndAngle) % 360 == 0)
                    {
                        endPoint.X = 0;
                        endPoint.Y = ellipseB;
                    }
                    else if ((270 - arc.EndAngle) % 360 == 0)
                    {
                        endPoint.X = 0;
                        endPoint.Y = -ellipseB;
                    }
                    else
                    {
                        rad = GetRadian(arc.EndAngle);
                        endPoint.X = ellipseA * ellipseB / Math.Sqrt(Math.Pow(ellipseB, 2) + Math.Pow(ellipseA * Math.Tan(rad), 2));
                        endPoint.X *= Math.Cos(rad) > 0 ? 1 : -1;
                        endPoint.Y = endPoint.X * Math.Tan(rad);
                    }

                    string pathData = $"M{startPoint.X + ellipseA + arc.Rect.X},{startPoint.Y + ellipseB + arc.Rect.Y} ";
                    pathData += $"A{ellipseA},{ellipseB} ";
                    pathData += $"0,{(majorArc ? "1" : "0")},{(clockWise ? "1" : "0")} ";
                    pathData += $"{endPoint.X + ellipseA + arc.Rect.X},{endPoint.Y + ellipseB + arc.Rect.Y}";
                    arc.Data = (Geometry)Geometry.Parse(pathData);
                }
                else
                {
                    arc.Data = (Geometry)Geometry.Parse("");
                }
            }
        }

        /// <summary>
        /// 将角度转化为弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static double GetRadian(double angle)
        {
            return angle / 180.0 * Math.PI;
        }
    }
}
