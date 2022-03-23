using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace MediaManage.DataBaseHandler
{
    public class MediaManager
    {
        static string sqlFolder = @"C:\Users\clay0\source\repos\MediaManageDB\MediaManage\SQL";
        public static DataTable SQL_SearchByID(string subID, SqlConnectionStringBuilder builder)
        {
            string sql = System.IO.File.ReadAllText(sqlFolder + @"\subID_ID_Video.sql");
            sql = sql.Replace("'__subID__'", string.Join(',', subID));
            DataTable dt = new DataTable();
            DataBaseHandler.SQLToDataBase(sql, builder, dt);
            return dt;
        }

        public static DataTable SQL_SearchBySubTitle(string subTitle, SqlConnectionStringBuilder builder)
        {
            string sql = System.IO.File.ReadAllText(sqlFolder + @"\subTitle_Title_Video.sql");
            sql = sql.Replace("'__subTitle__'", string.Join(',', subTitle));
            DataTable dt = new DataTable();
            DataBaseHandler.SQLToDataBase(sql, builder, dt);
            return dt;
        }

        public static DataTable SQL_SearchByTags(string[] tags, SqlConnectionStringBuilder builder)
        {
            var cat = from tag in tags 
                      select "N'"+tag+"'";
            string sql = System.IO.File.ReadAllText(sqlFolder+@"\tags_VideosTags.sql");
            sql = sql.Replace("'__tags__'", string.Join(',', cat));
            DataTable dt = new DataTable();
            DataBaseHandler.SQLToDataBase(sql, builder, dt);
            return dt;
        }
    }
}
