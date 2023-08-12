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

namespace WpfCanvasPartialRenderDemoScrollView
{
    public partial class MainWindow : Window
    {
        List<Brush> brushes = new List<Brush>();
        private const int ElementCount = 100000;
        private const int PartialRenderCount = 100;
        private const int ViewportBufferSize = 100;

        private Random random = new Random();
        private int currentIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            canvas.SizeChanged += Canvas_SizeChanged;
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Pink);
            brushes.Add(Brushes.Yellow);
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.BlanchedAlmond);
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateViewport();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateViewport();
        }

        private void UpdateViewport()
        {
            double viewportTop = scrollViewer.VerticalOffset;
            double viewportBottom = viewportTop + scrollViewer.ViewportHeight;

            // Remove visual elements outside the viewport buffer range
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                var visualElement = canvas.Children[i] as UIElement;
                if (visualElement != null)
                {
                    double elementTop = Canvas.GetTop(visualElement);
                    double elementBottom = elementTop + visualElement.RenderSize.Height;

                    if (elementBottom < viewportTop - ViewportBufferSize ||
                        elementTop > viewportBottom + ViewportBufferSize)
                    {
                        canvas.Children.Remove(visualElement);
                    }
                }
            }

            // Render visual elements within the viewport range
            for (int i = 0; i < PartialRenderCount; i++)
            {
                if (currentIndex < ElementCount)
                {
                    double elementTop = random.NextDouble() * (canvas.ActualHeight - 10);
                    double elementLeft = random.NextDouble() * (canvas.ActualWidth - 10);

                    if (elementTop >= viewportTop - ViewportBufferSize &&
                        elementTop <= viewportBottom + ViewportBufferSize)
                    {
                        var visualElement = CreateVisualElement();
                        Canvas.SetTop(visualElement, elementTop);
                        Canvas.SetLeft(visualElement, elementLeft);
                        canvas.Children.Add(visualElement);
                    }

                    currentIndex++;
                }
                else
                {
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
                Fill = brushes[new Random().Next(0,5)]
            };

            return ellipse;
        }
    }
}
