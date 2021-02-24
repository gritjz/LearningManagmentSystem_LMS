using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForCourses.Common
{
    public class MD5Generator
    {
        public static string GetMD5String(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] pws = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string pwd = "";
            foreach(var item in pws)
            {
                pwd += item.ToString("X2");
            }
            return pwd;
        }
    }
}
