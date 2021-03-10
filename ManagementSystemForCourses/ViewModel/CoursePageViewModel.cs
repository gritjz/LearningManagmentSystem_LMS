using ManagementSystemForCourses.DataAccess;
using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.ViewModel
{
    public class CoursePageViewModel
    {
        public ObservableCollection<CategoryItemModel> CategoryCourses { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryTechniques { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryInstructors { get; set; }
        public CoursePageViewModel()
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


    }
}
