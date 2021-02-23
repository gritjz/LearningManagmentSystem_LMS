using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.Model
{
    public class LoginModel: NotifyBase
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get {
                //if (password ==null) 
                //{ 
                //    password = ""; 
                //}
                return password; 
            }
            set { password = value; }
        }

        private string validataioncode;

        public string ValidataionCode
        {
            get { return validataioncode; }
            set { validataioncode = value; }
        }

    }
}
