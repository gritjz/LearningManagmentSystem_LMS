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
            set { username = value; this.DoNotify(); }
        }

        private string password;

        public string Password
        {
            get {
                return password; 
            }
            set 
            { 
                password = value;
                this.DoNotify();
            }
        }

        private string validataioncode;

        public string ValidataionCode
        {
            get 
            {
                return validataioncode; 
            }
            set { 
                validataioncode = value; 
                this.DoNotify(); 
            }
        }

    }
}
