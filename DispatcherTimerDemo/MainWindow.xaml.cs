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
using System.Windows.Threading;

namespace DispatcherTimerDemo
{
    public partial class MainWindow : Window
    {
        private const int ElementCount = 1000000;
        private const int PartialRenderCount = 100;
        private const int IntervalMilliseconds = 1;

        private Random random = new Random();
        private int currentIndex = 0;
        private DispatcherTimer timer;
        List<Brush> brushes = new List<Brush>();
        public MainWindow()
        {
            InitializeComponent();
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Pink);
            brushes.Add(Brushes.Yellow);
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.BlanchedAlmond);
            canvas.Loaded += Canvas_Loaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(IntervalMilliseconds);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Render a subset of visual elements on each tick
            for (int i = 0; i < PartialRenderCount; i++)
            {
                if (currentIndex < ElementCount)
                {
                    // Create a new visual element
                    var visualElement = CreateVisualElement();

                    // Add the visual element to the canvas
                    Canvas.SetLeft(visualElement, random.NextDouble() * (canvas.ActualWidth - 10));
                    Canvas.SetTop(visualElement, random.NextDouble() * (canvas.ActualHeight - 10));
                    canvas.Children.Add(visualElement);

                    currentIndex++;
                }
                else
                {
                    // All visual elements have been rendered, stop the timer
                    timer.Stop();
                    break;
                }
            }
        }

        private UIElement CreateVisualElement()
        {
            var ellipse = new Ellipse()
            {
                Width = 5,
                Height = 5,
                Fill = brushes[currentIndex % 5],
            };

            return ellipse;
        }
    }
}
