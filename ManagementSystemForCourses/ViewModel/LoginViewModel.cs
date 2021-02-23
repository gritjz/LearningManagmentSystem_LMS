using ManagementSystemForCourses.Common;
using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSystemForCourses.ViewModel
{
    public class LoginViewModel
    {
        public LoginModel LoginModel { get; set; }
        public CommandBase CloseWindowCommand { get; set; }

        public LoginViewModel()
        {
            this.LoginModel = new LoginModel();
            this.LoginModel.Password = "123123";

            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) => 
            {
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }


    }
}
