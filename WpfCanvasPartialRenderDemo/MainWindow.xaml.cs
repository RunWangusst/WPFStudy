using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfCanvasPartialRenderDemo
{
    public partial class MainWindow : Window
    {
        private const int ElementCount = 1000000;
        private const int PartialRenderCount = 100;

        private Random random = new Random();
        private VisualCollection visualCollection;
        private bool isRendering = false;
        private int currentIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            visualCollection = new VisualCollection(canvas);
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Pink);
            brushes.Add(Brushes.Yellow);
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.BlanchedAlmond);
            canvas.Loaded += Canvas_Loaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            //// Render a subset of visual elements on each frame
            //for (int i = 0; i < ElementCount; i++)
            //{
            //    // Create a new visual element
            //    var visualElement = CreateVisualElement();
            //    canvas.Children.Add(visualElement);
            //}
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }
        List<Brush> brushes = new List<Brush>();

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!isRendering)
            {
                isRendering = true;

                // Render a subset of visual elements on each frame
                for (int i = 0; i < PartialRenderCount; i++)
                {
                    if (currentIndex < ElementCount)
                    {
                        //// Create a new visual element
                        //var drawingVisual = CreateDrawingVisual();

                        //// Add the drawing visual to the visual collection
                        //visualCollection.Add(drawingVisual);

                        // Create a new visual element
                        var visualElement = CreateVisualElement();
                        canvas.Children.Add(visualElement);

                        currentIndex++;
                    }
                    else
                    {
                        // All visual elements have been rendered, stop rendering
                        CompositionTarget.Rendering -= CompositionTarget_Rendering;
                        break;
                    }
                }

                isRendering = false;
            }
        }
        private UIElement CreateVisualElement()
        {
            double x = random.NextDouble() * (canvas.ActualWidth - 10);
            double y = random.NextDouble() * (canvas.ActualHeight - 10);

            var ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = brushes[currentIndex % 5],
                Margin = new Thickness(x, y, 0, 0)
            };

            return ellipse;
        }
        private DrawingVisual CreateDrawingVisual()
        {
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                double x = random.NextDouble() * (canvas.ActualWidth - 10);
                double y = random.NextDouble() * (canvas.ActualHeight - 10);

                dc.DrawEllipse(Brushes.Blue, null, new Point(x, y), 5, 5);
            }

            return drawingVisual;
        }
    }
}
