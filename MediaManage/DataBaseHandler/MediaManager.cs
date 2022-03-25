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
        public static DataTable SQL_SearchBy_ITT(SqlConnectionStringBuilder builder, 
            string youtubeID, string title, string tagString)
        {
            string sql = System.IO.File.ReadAllText(sqlFolder + @"\SearchBy_ITT_template.sql");
            sql = sql.Replace("__subID__", youtubeID);
            sql = sql.Replace("__subTitle__", title);
            var tags = from tag in tagString.Split(',')
                       select $"N'{tag}'";
            sql = sql.Replace("'__tags__'", string.Join(',', tags));
            DataTable dt = new DataTable();
            DataBaseHandler.SQLToDataBase(sql, builder, dt);
            return dt;
        }
    }
}
