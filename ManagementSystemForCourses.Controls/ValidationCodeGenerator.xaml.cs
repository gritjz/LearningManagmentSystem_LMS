using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ValidationCodeGenerator.xaml
    /// </summary>
    public partial class ValidationCodeGenerator : UserControl
    {
       
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
              DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ValidationCodeGenerator),
              new FrameworkPropertyMetadata(null));


        public string ValidationCode
        {
            get { return (string)GetValue(ValidationCodeProperty); }
            set { SetValue(ValidationCodeProperty, value); }
        }

        public static readonly DependencyProperty ValidationCodeProperty =
            DependencyProperty.Register("ValidationCode", typeof(string), typeof(ValidationCodeGenerator),
                new PropertyMetadata(null));


        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ValidationCodeGenerator).UpdateCode();
        }


        public ValidationCodeGenerator()
        {
            InitializeComponent();
           
            this.Loaded += ValidationCodeGenerator_Loaded; ;
            this.SizeChanged += ValidationCodeGenerator_SizeChanged;
        }

        public static int ImageWidth, ImageHeight;
        private void ValidationCodeGenerator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageWidth = (int)this.grdRoot.RenderSize.Width;
            ImageHeight = (int)this.grdRoot.RenderSize.Height;
            UpdateCodeImage(ValidationCode);
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateCode();
        }
        private void ValidationCodeGenerator_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.ValidationCode == null)
                //this.UpdateCode();
            //UpdateCodeImage(ValidationCode);
        }
        public void UpdateCode()
        {
            ValidationCode = CreateCode(4);
            ImageSource = CreateValidationCodeImage(ValidationCode, ImageWidth, ImageHeight);
        }

        public void UpdateCodeImage(string code)
        {
            ImageSource = CreateValidationCodeImage(code, ImageWidth, ImageHeight);
        }

        private static string CreateCode(int strLength)
        {
            var strCode = "abcdefhkmnprstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789"; ;
            var _charArray = strCode.ToCharArray();
            var randomCode = "";
            int temp = -1;
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < strLength; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(strCode.Length - 1);
                if (!string.IsNullOrWhiteSpace(randomCode))
                {
                    while (randomCode.ToLower().Contains(_charArray[t].ToString().ToLower()))
                    {
                        t = rand.Next(strCode.Length - 1);
                    }
                }
                if (temp == t)
                {
                    return CreateCode(strLength);
                }
                temp = t;

                randomCode += _charArray[t];
            }
            return randomCode;
        }

        private ImageSource CreateValidationCodeImage(string code, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            if (width <= 0 || height <= 0)
                return null;

            DrawingVisual drawingVisual = new DrawingVisual();

            Random random = new Random(Guid.NewGuid().GetHashCode());

            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Red, new Pen(Brushes.Silver, 1D), new Rect(new Size(70, 23)));
                FormattedText formattedText = new FormattedText(code,
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Arial"), FontStyles.Oblique, FontWeights.Bold, FontStretches.Normal),
                    20.001D, new LinearGradientBrush(Colors.Green, Colors.DarkRed, 1.2D))
                {
                    MaxLineCount = 1,
                    TextAlignment = TextAlignment.Justify,
                    Trimming = TextTrimming.CharacterEllipsis
                };

                dc.DrawText(formattedText, new Point(3D, 0.1D));

                for (int i = 0; i < 10; i++)
                {
                    int x1 = random.Next(width - 1);
                    int y1 = random.Next(height - 1);
                    int x2 = random.Next(width - 1);
                    int y2 = random.Next(height - 1);

                    dc.DrawGeometry(Brushes.Silver, new Pen(Brushes.Silver, 0.5D), new LineGeometry(new Point(x1, y1), new Point(x2, y2)));
                }

                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(width - 1);
                    int y = random.Next(height - 1);
                    SolidColorBrush c = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
                    dc.DrawGeometry(c, new Pen(c, 1D), new LineGeometry(new Point(x - 0.5, y - 0.5), new Point(x + 0.5, y + 0.5)));
                }

                dc.Close();
            }

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(70, 23, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(drawingVisual);
            return BitmapFrame.Create(renderBitmap);
        }
    }
}
