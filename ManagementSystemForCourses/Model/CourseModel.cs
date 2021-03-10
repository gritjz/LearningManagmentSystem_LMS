using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.Model
{
    public class CourseModel
    {
        public string CourseName { get; set; }

        public string CourseCover { get; set; }

        public string CourseUrl { get; set; }

        public string CourseDescription { get; set; }

        public List<string> CourseInstructors { get; set; }

        public bool IsShowSkeleton { get; set; }
    }
}
