using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfCanvasPartialRenderDemo
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Item> items;

        public List<Item> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GenerateItems();
        }

        private void GenerateItems()
        {
            List<Brush> brushes = new List<Brush>();
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Pink);
            brushes.Add(Brushes.Yellow);
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.BlanchedAlmond);

            // Generate a large number of items
            List<Item> generatedItems = new List<Item>();
            for (int i = 0; i < 10000; i++)
            {
                generatedItems.Add(new Item { X = i * 50, Y = i * 50, BrushR = brushes[i % 5] });
            }
            Items = generatedItems;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class Item
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Brush BrushR { get; set; }
    }
}
