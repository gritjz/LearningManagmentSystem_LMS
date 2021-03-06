using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ManagementSystemForCourses.Controls
{
    /// <summary>
    /// Interaction logic for InstrumentPanel.xaml
    /// </summary>
    public partial class InstrumentPanel : UserControl
    {
        //Dependency Property , Dependency object
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(InstrumentPanel),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChanged)));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as InstrumentPanel).Refresh();
        }

        public InstrumentPanel()
        {
            InitializeComponent();

            this.SizeChanged += InstrumentPanel_SizeChanged;
        }

      

        private void InstrumentPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            this.backEllipse.Width = minSize;
            this.backEllipse.Height = minSize;
            Refresh();
        }

        private void Refresh()
        {
            double radius = this.backEllipse.Width / 2;
            if (double.IsNaN(radius))
                return;

            this.mainCanvas.Children.Clear();

            double min = 0, max = 100;
            double scaleCounter = 10;

            double step = 270.0 / (max - min);

            for (int i = 0; i < max - min; ++i)
            {
                Line lineScale = new Line();

                lineScale.X1 = radius - (radius - 13) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 13) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);

                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 1;

                this.mainCanvas.Children.Add(lineScale);

            }
            step = 270.0 / scaleCounter;
            int scaleText = (int)min;
            for (int i = 0; i <= scaleCounter; ++i)
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 20) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 20) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 1;
                this.mainCanvas.Children.Add(lineScale);


                //hahahsdwqe
                TextBlock textScale = new TextBlock();
                textScale.Width = 34;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = 14;
                textScale.Text = (scaleText + (max - min) / scaleCounter * i).ToString();
                textScale.Foreground = Brushes.White;
                Canvas.SetLeft(textScale, radius - (radius - 36) * Math.Cos((i * step - 45) * Math.PI / 180) - 17);
                Canvas.SetTop(textScale, radius - (radius - 36) * Math.Sin((i * step - 45) * Math.PI / 180) - 10);
                this.mainCanvas.Children.Add(textScale);


                //
                string sData = "M{0} {1} A{0} {0} 0 1 1 {1} {2}";
                sData = string.Format(sData, radius / 2, radius, radius * 1.5);
                var converter = TypeDescriptor.GetConverter(typeof(Geometry));
                this.circle.Data = (Geometry)converter.ConvertFrom(sData);


                //
                sData = "M{0},{1},{1},{2},{1},{3}";
                sData = string.Format(sData, radius * 0.3, radius, radius - 8, radius + 5);
                this.pointer.Data = (Geometry)converter.ConvertFrom(sData);

            }
        }
    }
}
