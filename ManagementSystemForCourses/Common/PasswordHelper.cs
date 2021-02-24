using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ManagementSystemForCourses.Common
{
    public class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password", 
                typeof(string), 
                typeof(PasswordHelper),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnpropertyChanged)));

        public static string GetPassword(DependencyObject d)
        {
            return d.GetValue(PasswordProperty).ToString();
        }
        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }


        public static readonly DependencyProperty AttachProperty =
           DependencyProperty.RegisterAttached(
               "Attach", 
               typeof(bool), 
               typeof(PasswordHelper), 
               new FrameworkPropertyMetadata(default(bool), new PropertyChangedCallback(OnAttached)));
        public static bool GetAttach(DependencyObject d)
        {
            return (bool)d.GetValue(AttachProperty);
        }
        public static void SetAttach(DependencyObject d, bool value)
        {
            d.SetValue(AttachProperty, value);
        }

        static bool isUpdating = false;

        private static void OnpropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox password = d as PasswordBox;
            password.PasswordChanged -= Password_PasswordChanged;
            if (!isUpdating)
                password.Password = e.NewValue?.ToString();//password is not null
            password.PasswordChanged += Password_PasswordChanged;
        }

        private static void OnAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox password = d as PasswordBox;
            password.PasswordChanged += Password_PasswordChanged;
        }

        private static void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pass = sender as PasswordBox;
            isUpdating = true;
            SetPassword(pass, pass.Password);
            isUpdating = false;
        }
    }
}
