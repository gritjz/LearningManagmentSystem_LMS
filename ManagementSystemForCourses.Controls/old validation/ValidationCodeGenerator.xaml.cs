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

        private string validationCode;

        public string ValidationCode
        {
            get { return validationCode; }
            set { 
                validationCode = value; this.DoNotify();
            }
        }

        private Bitmap validationCodeImage;

        public Bitmap ValidationCodeImage
        {
            get { return validationCodeImage; }
            set { validationCodeImage = value; this.DoNotify(); }
        }


        static int ImageHeight, ImageWidth;

        public event PropertyChangedEventHandler PropertyChanged;
        public void DoNotify([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public ValidationCodeGenerator()
        {
            InitializeComponent();
            ImageHeight = (int)imageCode.Height;
            ImageWidth = (int)imageCode.Width;
            ValidationCode = GetImage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns>bitmap</returns>
        public static Bitmap CreateVerifyCode(out string code)
        {
            //Create Bitmap object and draw
            Bitmap bitmap = new Bitmap(ImageWidth, ImageHeight);
            Graphics graph = Graphics.FromImage(bitmap);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.Orange), 0, 0, ImageWidth, ImageHeight);//Fill the Image area
            Font font = new Font(System.Drawing.FontFamily.GenericSerif, 40, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();
            string letters = "QWERTYUIOPLKJHGFDSAZXCVBNM0987654321";//Every verify code is from here
            //StringBuilder sb = new StringBuilder();
            code = "";

            //Create five letters randomly
            for (int i = 0; i < 4; i++)
            {
                string letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
                //sb.Append(letter);
                code += letter;
                graph.DrawString(letter, font, new SolidBrush(System.Drawing.Color.Black), i * 30, r.Next(0, 10));
            }
            //code = sb.ToString();

            //Confuse the background
            System.Drawing.Pen linePen = new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.Black), 2);
            for (int i = 0; i < 4; i++)
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

        private void CheckCode()
        {

            string text_code = "";
            if (text_code == "")
            {
                MessageBox.Show("Please input the check code.", "提示");
                ValidationCode = GetImage();//Update the verify code and then input again
            }
            else if (text_code != ValidationCode)
            {
                MessageBox.Show("Check code is error, please input correctly.", "提示");
                ValidationCode = GetImage();//Update the verify code and then input again
            }
            else
            {
                MessageBox.Show("Bingo~", "提示");
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.GetImage();
        }

        public string GetImage()
        {
            string code = "";
            Bitmap bitmap = CreateVerifyCode(out code);
            ImageSource imageSource = ChangeBitmapToImageSource(bitmap);
            imageCode.Source = imageSource;
            return code;
        }

    }
}
