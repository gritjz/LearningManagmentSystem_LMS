
using ManagementSystemForCourses.Controls;
using ManagementSystemForCourses.ViewModel;
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
using System.Windows.Shapes;

namespace ManagementSystemForCourses.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        LoginViewModel loginViewModel = null;
        bool firstFocus = true;
        static int ImageWidth, ImageHeight;
        public LoginView()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;
        }

        private void WinTopMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (loginViewModel != null && firstFocus)
            {
                loginViewModel.UpdateValidationCode(4, ImageWidth, ImageHeight);
                firstFocus = false;
            }
        }

        private void Code_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (loginViewModel != null)
                loginViewModel.UpdateValidationCode(4, ImageWidth, ImageHeight);
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            //Get Validation code image width and height
            ImageWidth = (int)(sender as Border).RenderSize.Width;
            ImageHeight = (int)(sender as Border).RenderSize.Height;
        }
    }
}
