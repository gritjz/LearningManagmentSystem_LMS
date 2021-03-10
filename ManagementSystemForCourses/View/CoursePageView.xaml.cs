using ManagementSystemForCourses.Model;
using ManagementSystemForCourses.ViewModel;
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

namespace ManagementSystemForCourses.View
{
    /// <summary>
    /// Interaction logic for CoursePageView.xaml
    /// </summary>
    public partial class CoursePageView : UserControl
    {
        public CoursePageView()
        {
            InitializeComponent();
            this.DataContext = new CoursePageViewModel();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            string instructor = button.Content.ToString();

            //Use ICollectionView to setup filtering. sorting ....
            ICollectionView view = CollectionViewSource.GetDefaultView(this.icCourses.ItemsSource);

            if (instructor != "All")
            {
                view.Filter = new Predicate<object>((o) => 
                {
                    return (o as CourseModel).CourseInstructors.Exists(i => i == instructor);
                });
            }
            else
            {
                view.Filter = null;
                //Sorting function
               // view.SortDescriptions.Add(new SortDescription("CourseName", ListSortDirection.Descending));
            }
        }
    }
}
