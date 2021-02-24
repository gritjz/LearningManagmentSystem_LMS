using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.DataAccess.DataEntity
{
    public class UserEntity
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string PassWord { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
    }
}
