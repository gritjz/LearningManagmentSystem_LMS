
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
        LoginViewModel loginVM = null;
        public LoginView()
        {
            InitializeComponent();
            loginVM = new LoginViewModel();
            this.DataContext = loginVM;
            
        }

       
        private void WinTopMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // var res = this.FindResource("RD1") as ResourceDictionary;
            // ValidationCodeGenerator res = (ValidationCodeGenerator)this.FindResource["PART_validcode"];
            //this.txtValid.Style = Application.Current.FindResource("LoginViewValidateCodeTextBoxStyle") as Style;

            // object obItem = this.FindResource("PART_validcode");

           
          //  (VisualTreeHelper.GetChild(this, i) as FrameworkElement).GetType().Name == "ValidationCodeGenerator"
        }


        static string GetTypeDescription(object obj)
        {
            return obj.GetType().Name;
        }

        /// <summary>
        /// 获取逻辑树
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeViewItem GetLogicTree(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }
            //创建逻辑树的节点
            TreeViewItem treeItem = new TreeViewItem { Header = GetTypeDescription(obj), IsExpanded = true };

            //循环遍历，获取逻辑树的所有子节点
            foreach (var child in LogicalTreeHelper.GetChildren(obj))
            {
                //递归调用
                var item = GetLogicTree(child as DependencyObject);
                if (item != null)
                {
                    treeItem.Items.Add(item);
                }
            }

            return treeItem;
        }

        /// <summary>
        /// 获取可视树
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeViewItem GetVisualTree(FrameworkElement obj)
        {
            if (obj == null)
            {
                return null;
            }

            TreeViewItem treeItem = new TreeViewItem { Header =GetTypeDescription(obj), IsExpanded = true };

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                var item = GetVisualTree(child);
                if (child!=null&&child.GetType().Name == "ValidationCodeGenerator")
                {
                    break;
                }
                if (item != null)
                {
                    treeItem.Items.Add(item);
                }
            }

            return treeItem;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.tvLogicTree.Items.Add(LoginView.GetLogicTree(this));
            this.tvVisualTree.Items.Add(LoginView.GetVisualTree(this));


           // TextBox hh = (TextBox)this.GetChildren<ValidationCodeGenerator>(null,null).FirstOrDefault();
        }

      

        //public IEnumerable<T> GetChildren<T>(this FrameworkElement element, Func<FrameworkElement, bool> pred = null) where T : FrameworkElement
        //{
        //    int childrenCount = VisualTreeHelper.GetChildrenCount(element);
        //    for (int i = 0; i < childrenCount; i++)
        //    {
        //        FrameworkElement child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
        //        if (child != null && (pred?.Invoke(child) ?? true) && child is T)
        //        {
        //            yield return child as T;
        //        }
        //        else
        //        {
        //            foreach (var item in GetChildren<T>(child, e => true))
        //            {
        //                yield return item;
        //            }
        //        }
        //    }
        //}

    }
}
