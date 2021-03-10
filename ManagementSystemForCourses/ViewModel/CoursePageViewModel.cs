using ManagementSystemForCourses.Common;
using ManagementSystemForCourses.DataAccess;
using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSystemForCourses.ViewModel
{
    public class CoursePageViewModel
    {
        public ObservableCollection<CategoryItemModel> CategoryCourses { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryTechniques { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryInstructors { get; set; }
        public ObservableCollection<CourseModel> CourseList { get; set; } = new ObservableCollection<CourseModel>();
        private List<CourseModel> CourseAll;
        public CommandBase OpenUrlCmd { get; set; }
        public CommandBase InstructorFilterCmd { get; set; }

        public CoursePageViewModel()
        {
            this.InitCommand();
            this.InitCourseCategory();
            this.InitCourseList();
        }

        private void InitCommand()
        {
            this.OpenUrlCmd = new CommandBase();
            this.OpenUrlCmd.DoCanExecute = new Func<object, bool>((o) => true);
            this.OpenUrlCmd.DoExecute = new Action<object>((o) => { System.Diagnostics.Process.Start(o.ToString()); });

            this.InstructorFilterCmd = new CommandBase();
            this.InstructorFilterCmd.DoCanExecute = new Func<object, bool>((o) => true);
            this.InstructorFilterCmd.DoExecute = new Action<object>(DoFilter); //delegate to DoFilter()
        }

        private void InitCourseCategory()
        {
            this.CategoryCourses = new ObservableCollection<CategoryItemModel>();
            this.CategoryCourses.Add(new CategoryItemModel("All", true));
            this.CategoryCourses.Add(new CategoryItemModel("Open Courses"));
            this.CategoryCourses.Add(new CategoryItemModel("VIP Courses"));

            this.CategoryTechniques = new ObservableCollection<CategoryItemModel>();
            this.CategoryTechniques.Add(new CategoryItemModel("All", true));
            this.CategoryTechniques.Add(new CategoryItemModel(".Net"));
            this.CategoryTechniques.Add(new CategoryItemModel("Java"));

            this.CategoryInstructors = new ObservableCollection<CategoryItemModel>();
            this.CategoryInstructors.Add(new CategoryItemModel("All", true));
            foreach (var item in LocalDataAccess.GetInstance().GetInstructorsInfo())
                this.CategoryInstructors.Add(new CategoryItemModel(item));
        }

        private void InitCourseList()
        {
            for (int i = 0; i < 10; ++i)
            {
                CourseList.Add(new CourseModel { IsShowSkeleton = true });
            }
            Task.Run(new Action(async () =>
           {
               CourseAll = LocalDataAccess.GetInstance().GetCoursesInfo();
               await Task.Delay(4000);

               Application.Current.Dispatcher.Invoke(new Action(() =>
                   {
                       CourseList.Clear();
                       foreach (var item in CourseAll)
                       {
                           CourseList.Add(item);
                       }

                   }));
           }));
        }

        private void DoFilter(object o)
        {
            string instructor = o.ToString();
            var course= CourseAll;
            if (instructor != "All") 
            {
                course = CourseAll.Where(c => c.CourseInstructors.Exists(e => e == instructor)).ToList();
            }
            CourseList.Clear();
            foreach (var item in course)
            {
                CourseList.Add(item);
            }
        }
    }
}
