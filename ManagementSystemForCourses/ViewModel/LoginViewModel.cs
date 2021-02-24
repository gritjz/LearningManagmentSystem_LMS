using ManagementSystemForCourses.Common;
using ManagementSystemForCourses.DataAccess;
using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSystemForCourses.ViewModel
{
    public class LoginViewModel: NotifyBase
    {
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public CommandBase CloseWindowCommand { get; set; }
        public CommandBase LoginCommand { get; set; }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; this.DoNotify(); }
        }

        private Visibility vis;

        public Visibility Vis
        {
            get { return vis; }
            set 
            { 
                vis = value; 
                this.DoNotify();
                
            }
        }


        public LoginViewModel()
        {
            this.LoginModel = new LoginModel();

            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) => 
            {
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            this.LoginCommand = new CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin);
            this.LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

        }
        //login logic Validation
        private void DoLogin(object o) 
        {
            this.Vis = Visibility.Visible;
            this.ErrorMessage = "";
            if (string.IsNullOrEmpty(LoginModel.Username))
            {
                this.ErrorMessage = "Please Enter User Name!";
                this.Vis = Visibility.Collapsed;
                return;
            }

            if(string.IsNullOrEmpty(LoginModel.Password))
            {
                this.ErrorMessage = "Please Enter Password!";
                this.Vis = Visibility.Collapsed;
                return;
            }

            if (string.IsNullOrEmpty(LoginModel.ValidataionCode))
            {
                this.ErrorMessage = "Please Enter Validation Code!";
                this.Vis = Visibility.Collapsed;
                return;

            }

            if (LoginModel.ValidataionCode.ToLower() == "etu4")
            {
                this.ErrorMessage = "Incorrect Validation Code!";
                this.Vis = Visibility.Collapsed;
                return;
            }


            Task.Run(new Action(() =>
            {
                try
                {
                    var user = LocalDataAccess.GetInstance().CheckUserInfo(LoginModel.Username, LoginModel.Password);
                    if (user == null)
                    {
                        throw new Exception("Login Failed! User Name or Password is incorrect!");
                    }

                    ////Store DB info into a global variable
                   GlobalValues.UserInfo = user;

                    ////click login button, then commandparameter will execute and jump to main window
                    ////then, program will execute App.xaml.cs
                    ////因为时从子线程中直接更新UI线程创建的对象，所以要用Dispatcher
                    Application.Current.Dispatcher.Invoke(new Action(() => 
                    { 
                        (o as Window).DialogResult = true; 
                    }));
                    

                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                }


            }));
        }


    }
}
