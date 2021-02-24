using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ManagementSystemForCourses.DataAccess.DataEntity;
using System.Data;
using ManagementSystemForCourses.Common;

namespace ManagementSystemForCourses.DataAccess
{
    public class LocalDataAccess
    {
        private static LocalDataAccess instance;

        private LocalDataAccess()
        { }

        public static LocalDataAccess GetInstance()
        {
            return instance ?? (instance = new LocalDataAccess());
        }

        SqlConnection conn;
        SqlCommand comm;
        SqlDataAdapter adapter;

        private void Dispose() 
        {
            if(adapter!=null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        //Connection initialization
        private bool DBConnection()
        {
            //'db' is the same name of database setup in App.config
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            if (conn == null)
                conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Data Acquire
        //data validation
        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try { 
            if(DBConnection())
            {
                string userSql = 
                    "select * from users where user_name@user_name and password@pwd and is_validation=1";
                adapter = new SqlDataAdapter(userSql, conn);
                adapter.SelectCommand.Parameters.Add(
                    new SqlParameter("@user_name", 
                    SqlDbType.VarChar){ Value = userName });
                adapter.SelectCommand.Parameters.Add(
                   new SqlParameter("@pwd",
                   SqlDbType.VarChar)
                   { Value = MD5Generator.GetMD5String(pwd+"@"+ userName)});

                DataTable table = new DataTable();
                int counter = adapter.Fill(table);

                    if (counter <= 0)
                        throw new Exception("User name or password is wrong!");
                    DataRow dr = table.Rows[0];
                    if(dr.Field<Int32>("is_can_login") == 0)
                    {
                        throw new Exception("Unauthorized User!");
                    }
                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = dr.Field<string>("user_name");
                    userInfo.RealName = dr.Field<string>("real_name");
                    //password needs MD5 
                    userInfo.PassWord = dr.Field<string>("password");
                    userInfo.Avatar = dr.Field<string>("avatar");
                    userInfo.Gender = dr.Field<Int32>("gender");
                    return userInfo;
            }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
            return null;
        }

    }

}
