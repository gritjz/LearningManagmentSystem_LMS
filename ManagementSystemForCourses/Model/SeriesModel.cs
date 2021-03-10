using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.Model
{
    public class SeriesModel
    {

        public string SeriesName { get; set; }

        public decimal CurrentViewCount { get; set; }

        public bool IsGrowing { get; set; }

        public int GrowingRate { get; set; }
    }
}
