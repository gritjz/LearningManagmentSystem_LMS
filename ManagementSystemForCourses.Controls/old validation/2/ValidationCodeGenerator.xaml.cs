using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public partial class ValidationCodeGenerator : UserControl, INotifyPropertyChanged
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public event PropertyChangedEventHandler PropertyChanged;
        public void DoNotify([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }





        public ImageSource CodeSource
        {
            get { return (ImageSource)GetValue(CodeSourceProperty); }
            set { SetValue(CodeSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CodeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeSourceProperty =
            DependencyProperty.Register("CodeSource", typeof(ImageSource), typeof(ValidationCodeGenerator), 
                new PropertyMetadata(null));



        //private ImageSource codeSource;

        //public ImageSource CodeSource
        //{
        //    get { return codeSource; }
        //    set { codeSource = value; this.DoNotify(); }
        //}


        private Bitmap codeBitmap;

        public Bitmap CodeBitmap
        {
            get { return codeBitmap; }
            set { codeBitmap = value; /*this.DoNotify();*/ }
        }

        private BitmapImage codeBitmapImage;

        public BitmapImage CodeBitmapImage
        {
            get { return codeBitmapImage; }
            set { codeBitmapImage = value; /*this.DoNotify();*/ }
        }


        private System.Windows.Controls.Image images;

        public System.Windows.Controls.Image Images
        {
            get { return images; }
            set { images = value; }
        }


        static int ImageHeight, ImageWidth;

        public ValidationCodeGenerator()
        {
            InitializeComponent();
            this.SizeChanged += ValidationCodeGenerator_SizeChanged;
        }

        private void ValidationCodeGenerator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageHeight = (int)this.grdRoot.RenderSize.Height;
            ImageWidth = (int)this.grdRoot.RenderSize.Width;
            UpdateImage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Refresh();
        }

        private void Refresh()
        {
           
            this.UpdateImage();
        }

        int i = 0;
        private void UpdateImage() 
        {
            this.CodeBitmap = CreateVerifyCode();
            this.CodeSource = ChangeBitmapToImageSource(this.CodeBitmap);
            this.CodeBitmapImage = this.BitmapToBitmapImage(this.CodeBitmap);
            //this.imgCode.Source = this.CodeSource;
        }

        private Bitmap CreateVerifyCode()
        {
            //Create Bitmap object and draw
            Bitmap bitmap = new Bitmap(ImageWidth, ImageHeight);
            Graphics graph = Graphics.FromImage(bitmap);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.Orange), 0, 0, ImageWidth, ImageHeight);//Fill the Image area
            Font font = new Font(System.Drawing.FontFamily.GenericSerif, 40, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();
            string letters = "QWERTYUIOPLKJHGFDSAZXCVBNM0987654321";//Every verify code is from here
            //StringBuilder sb = new StringBuilder();
            this.Code = "";

            //Create five letters randomly
            for (int i = 0; i < 4; i++)
            {
                string letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
                //sb.Append(letter);
                this.Code += letter;
                graph.DrawString(letter, font, new SolidBrush(System.Drawing.Color.Black), i * 30, r.Next(0, 10));
            }
            //code = sb.ToString();

            //Confuse the background
            System.Drawing.Pen linePen = new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.Black), 2);
            for (int i = 0; i < 5; i++)
            {
                graph.DrawLine(linePen, new System.Drawing.Point(r.Next(0, ImageWidth - 1), r.Next(0, ImageHeight - 1)),
                    new System.Drawing.Point(r.Next(0, ImageWidth - 1), r.Next(0, ImageHeight - 1)));
            }
            return bitmap;
        }
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                //Need to release the hBitmap in time, otherwise the memory will fill up quickly.
                DeleteObject(hBitmap);
                return wpfBitmap;
            }
            catch
            {
                DeleteObject(hBitmap);
                return null;
            }
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
    }
}
