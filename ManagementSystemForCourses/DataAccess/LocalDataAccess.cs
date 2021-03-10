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
using ManagementSystemForCourses.Model;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts;

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
            if (adapter != null)
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
                //throw new Exception("Failed Connection");
                return false;
            }

        }

        //Data Acquire
        //data validation
        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try
            {
                if (DBConnection())
                {
                    string userSql = "select * from users where user_name=@user_name and password=@pwd and is_validation=1";
                    adapter = new SqlDataAdapter(userSql, conn);
                    adapter.SelectCommand.Parameters.Add(
                        new SqlParameter("@user_name",
                        SqlDbType.VarChar)
                        { Value = userName });
                    adapter.SelectCommand.Parameters.Add(
                       new SqlParameter("@pwd",
                       SqlDbType.VarChar)
                       { Value = MD5Generator.GetMD5String(pwd + "@" + userName) });

                    DataTable table = new DataTable();
                    int counter = adapter.Fill(table);

                    if (counter <= 0)
                        throw new Exception("User name or password is wrong!");
                    DataRow dr = table.Rows[0];
                    if (dr.Field<Int32>("is_can_login") == 0)
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
            return null;
        }

        public List<CourseSeriesModel> GetCoursePlayRecord()
        {
            try
            {
                List<CourseSeriesModel> cModelList = new List<CourseSeriesModel>();
                if (DBConnection())
                {
                    string userSql = @"select a.course_name,a.course_id,b.play_count,b.is_growing,b.growing_rate ,
c.platform_name
from courses a
left join play_record b
on a.course_id = b.course_id
left join platforms c
on b.platform_id = c.platform_id
order by a.course_id,c.platform_id";
                    adapter = new SqlDataAdapter(userSql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    string courseId = "";
                    CourseSeriesModel cModel = null;

                    foreach (DataRow dr in table.AsEnumerable())
                    {
                        string tempId = dr.Field<string>("course_id");
                        if (courseId != tempId)
                        {
                            courseId = tempId;
                            cModel = new CourseSeriesModel();
                            cModelList.Add(cModel);

                            cModel.CourseName = dr.Field<string>("course_name");
                            cModel.SeriesCollection = new LiveCharts.SeriesCollection();
                            cModel.SeriesList = new System.Collections.ObjectModel.ObservableCollection<SeriesModel>();
                        }
                        if (cModel != null)
                        {
                            cModel.SeriesCollection.Add(new PieSeries
                            {
                                Title = dr.Field<string>("platform_name"),
                                Values = new ChartValues<ObservableValue> { new ObservableValue((double)dr.Field<decimal>("play_count")) },
                                DataLabels = false
                            });

                            cModel.SeriesList.Add(new SeriesModel
                            {
                                SeriesName = dr.Field<string>("platform_name"),
                                CurrentViewCount = dr.Field<decimal>("play_count"),
                                IsGrowing = dr.Field<Int32>("is_growing") == 1,
                                GrowingRate = (int)dr.Field<decimal>("growing_rate")
                            });
                        }
                    }
                }
                return cModelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<string> GetInstructorsInfo()
        {
            try
            {
                List<string> result = new List<string>();
                if (DBConnection())
                {
                    string userSql = @"select a.real_name from users a where a.is_teacher = 1";


                    adapter = new SqlDataAdapter(userSql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    if (count > 0)
                    {
                        result = table.AsEnumerable().Select(c => c.Field<string>("real_name")).ToList();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

        }


        public List<CourseModel> GetCoursesInfo()
        {
            try
            {
                List<CourseModel> result = new List<CourseModel>();
                if (DBConnection())
                {
                    string userSql = @"select a.course_id, a.course_name, c.real_name,a.course_url, a.course_cover, a.description from courses a
left join course_teacher_relation b
on a.course_id = b.course_id
left join users c
on b.teacher_id = c.user_id
order by a.course_id";


                    adapter = new SqlDataAdapter(userSql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    if (count > 0)
                    {
                        string courseId = "";
                        CourseModel model = null;
                        foreach (DataRow dr in table.AsEnumerable())
                        {
                            string tempId = dr.Field<string>("course_id");
                            if (courseId != tempId)
                            {
                                model = new CourseModel();
                                model.CourseName = dr.Field<string>("course_name");
                                model.CourseCover = dr.Field<string>("course_cover");
                                model.CourseDescription = dr.Field<string>("description");

                                model.CourseUrl = dr.Field<string>("course_url");
                                model.CourseInstructors = new List<string>();
                                result.Add(model);
                            }
                            if (model != null)
                            {
                                model.CourseInstructors.Add(dr.Field<string>("real_name"));
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

        }

    }

}
