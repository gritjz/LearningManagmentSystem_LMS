using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ManagementSystemForCourses.Controls
{
    public class ValidationCoder
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static string CreateCode(int length)
        {
            var strCode = "abcdefhkmnprstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789"; ;
            var _charArray = strCode.ToCharArray();
            var randomCode = "";
            int temp = -1;
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < length; i++)
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
                    return CreateCode(length);
                }
                temp = t;

                randomCode += _charArray[t];
            }
            return randomCode;
        }


        public static ImageSource CreateValidationCodeImage(string code, int width, int height)
        {

            if (string.IsNullOrWhiteSpace(code))
                return null;
            if (width <= 0 || height <= 0)
                return null;

            Bitmap bitmap = new Bitmap(width, height);

            Graphics graph = Graphics.FromImage(bitmap);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.Orange), 0, 0, width, height);//Fill the Image background
            Font font = new Font(System.Drawing.FontFamily.GenericSerif, 28, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();

            for (int i = 0; i < code.Length; i++)
            {
                graph.DrawString(code[i].ToString(), font,
                    new SolidBrush(
                        System.Drawing.Color.Black),
                    i* (width / (code.Length+1)),
                    0/*r.Next(0, height)*/);
            }
          
            //Confuse the background
            System.Drawing.Pen linePen = new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.Black), 2);
            for (int i = 0; i < 5; i++)
            {
                graph.DrawLine(linePen,
                    new System.Drawing.Point(r.Next(0, width - 1), r.Next(0, height - 1)),
                    new System.Drawing.Point(r.Next(0, width - 1), r.Next(0, height - 1))
                    );
            }
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

    }
}
