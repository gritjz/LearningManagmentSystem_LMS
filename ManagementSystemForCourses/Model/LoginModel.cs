using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ManagementSystemForCourses.Model
{
    public class LoginModel : NotifyBase
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
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.DoNotify();
            }
        }

        private string validataionInputcode;

        public string ValidataionInputCode
        {
            get
            {
                return validataionInputcode;
            }
            set
            {

                validataionInputcode = value;
                this.DoNotify();
            }
        }

        private ImageSource _validationCodeSource;

        public ImageSource ValidationCodeSource
        {
            get { return _validationCodeSource; }
            set
            {
                _validationCodeSource = value;
                this.DoNotify();
            }
        }

        private string _validationCode;

        public string ValidationCode
        {
            get { return _validationCode; }
            set
            {
                _validationCode = value;
                this.DoNotify();
            }
        }

        //private int _validationCodeWidth;

        //public int ValidationCodeWidth
        //{
        //    get { return _validationCodeWidth; }
        //    set { _validationCodeWidth = value; 
        //        this.DoNotify(); }
        //}

        //private int _validationCodeHeight;

        //public int ValidationCodeHeight
        //{
        //    get { return _validationCodeHeight; }
        //    set { _validationCodeHeight = value; 
        //        this.DoNotify(); }
        //}
    }
}
