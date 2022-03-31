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
    using classes;
    public class MediaManager
    {
        static string sqlFolder = @"C:\Users\clay0\source\repos\MediaManageDB\MediaManage\SQL";
        public static DataTable SQL_SearchBy_ITT(SqlConnectionStringBuilder builder, 
            string youtubeID, string title, string tagString)
        {
            DataTable dt = new DataTable();
            string sql;
            if (tagString != "")
            {
                sql = System.IO.File.ReadAllText(sqlFolder + @"\SearchBy\ID_Title_Tag.sql");
                sql = sql.Replace("__subID__", youtubeID);
                sql = sql.Replace("__subTitle__", title);
                var tags = from tag in tagString.Split(',')
                           select $"N'{tag}'";
                sql = sql.Replace("'__tags__'", string.Join(',', tags));
            }
            else
            {
                sql = System.IO.File.ReadAllText(sqlFolder + @"\SearchBy\ID_Title.sql");
                sql = sql.Replace("__subID__", youtubeID);
                sql = sql.Replace("__subTitle__", title);
            }
            DataBaseHandler.SQLToDataBase(sql, builder, dt);
            return dt;
        }

        public static void SQL_DeleteYTID(SqlConnectionStringBuilder builder, string youtubeID)
        {
            string sql = System.IO.File.ReadAllText(sqlFolder + @"\Delete_YTID.sql");
            sql = sql.Replace("__YoutubeID__", youtubeID);
            DataBaseHandler.SQLToDataBase(sql, builder);
        }

        public static void SQL_CreateYTID(SqlConnectionStringBuilder builder, Info info)
        {
            string sql = "";
            // add to Table - Video
            sql = $"INSERT INTO Video(YoutubeID, Title) VALUES('{info.YoutubeID}', N'{info.Title}');";
            DataBaseHandler.SQLToDataBase(sql, builder);

            // add to Table - VideosTags
            List<String> tagList = new List<string>();
            MergeTag(builder, tagList);
            var adds = from tag in info.TagString.Split(',')
                       group tag by tagList.Contains(tag) into g
                       select new
                       {
                           Exist = g.Key,
                           Tags = from tag in g.AsEnumerable()
                                  where tag != ""
                                  select $"N'{tag}'" // nvarchar format in sql
                       };
            IEnumerable<string> tags = Enumerable.Empty<string>();
            foreach (var add in adds)
            {
                tags = tags.Concat(add.Tags);
                if (!add.Exist) // add.Exist == true, if tag exists in Table - VideoTag
                {
                    foreach (var tag in add.Tags)
                    {
                        sql = System.IO.File.ReadAllText(sqlFolder + @"\VideoTag\Add.sql");
                        sql = sql.Replace("N'__TagName__'", tag);
                        DataBaseHandler.SQLToDataBase(sql, builder);
                    }
                }
            }
            sql = System.IO.File.ReadAllText(sqlFolder + @"\VideosTags\Add.sql");
            sql = sql.Replace("'__tags__'", string.Join(',', tags));
            sql = sql.Replace("__YoutubeID__", info.YoutubeID);
            DataBaseHandler.SQLToDataBase(sql, builder);
        }

        public static void SQL_UpdateInfo(SqlConnectionStringBuilder builder, Info oldInfo, Info newInfo)
        {
            if (oldInfo.YoutubeID != newInfo.YoutubeID)
            {
                SQL_DeleteYTID(builder, oldInfo.YoutubeID);
                SQL_CreateYTID(builder, newInfo);
            }
            else
            {
                Update_Video(builder, oldInfo, newInfo);
                Update_VideosTags(builder, oldInfo, newInfo);
            }
        }

        public static void MergeTag(SqlConnectionStringBuilder builder, List<string> tagList)
        {
            string sql = "SELECT TagName FROM VideoTag";
            DataTable tb = new DataTable();
            DataBaseHandler.SQLToDataBase(sql, builder, tb);
            foreach (DataRow dr in tb.AsEnumerable())
            {
                if (dr["TagName"].ToString() is not string tagName)
                    continue;
                if (!tagList.Contains(tagName))
                    tagList.Add(tagName);
            }
        }

        private static void Update_VideosTags(SqlConnectionStringBuilder builder, Info oldInfo, Info newInfo)
        {
            var oldTags = oldInfo.TagString.Split(',').ToList();
            var newTags = newInfo.TagString.Split(',').ToList();
            List<String> tagList = new List<string>();
            MergeTag(builder, tagList);
            var both = (from tag1 in oldTags
                        join tag2 in newTags
                        on tag1 equals tag2
                        select tag1).ToList();
            string sql = "";

            // remove from Table - VideosTags
            var removeTags = from tag in oldTags
                             where !both.Contains(tag)
                             select $"N'{tag}'";
            sql = System.IO.File.ReadAllText(sqlFolder + @"\VideosTags\Remove.sql");
            sql = sql.Replace("'__tags__'", string.Join(',', removeTags));
            sql = sql.Replace("__YoutubeID__", newInfo.YoutubeID);
            DataBaseHandler.SQLToDataBase(sql, builder);

            // add to Table - VideosTags & Table - VideoTag if there is new tag
            var adds = from tag in newTags
                         where !both.Contains(tag)
                         group tag by tagList.Contains(tag) into g
                         select new 
                         { 
                             Exist = g.Key,
                             Tags = from tag in g.AsEnumerable() 
                                    select $"N'{tag}'" // nvarchar format in sql
                         };
            IEnumerable<string> tags = Enumerable.Empty<string>();
            foreach (var add in adds)
            {
                tags = tags.Concat(add.Tags);
                if (!add.Exist) // add.Exist == true, if tag exists in Table - VideoTag
                {
                    foreach (var tag in add.Tags)
                    {
                        sql = System.IO.File.ReadAllText(sqlFolder + @"\VideoTag\Add.sql");
                        sql = sql.Replace("N'__TagName__'", tag);
                        DataBaseHandler.SQLToDataBase(sql, builder);
                    }
                }
            }
            sql = System.IO.File.ReadAllText(sqlFolder + @"\VideosTags\Add.sql");
            sql = sql.Replace("'__tags__'", string.Join(',', tags));
            sql = sql.Replace("__YoutubeID__", newInfo.YoutubeID);
            DataBaseHandler.SQLToDataBase(sql, builder);
        }

        private static void Update_Video(SqlConnectionStringBuilder builder, Info oldInfo, Info newInfo)
        {
            string sql = System.IO.File.ReadAllText(sqlFolder + @"\Video\Update.sql");
            sql = sql.Replace("__NewYoutubeID__", newInfo.YoutubeID);
            sql = sql.Replace("__NewTitle__", newInfo.Title);
            sql = sql.Replace("__YoutubeID__", oldInfo.YoutubeID);
            DataBaseHandler.SQLToDataBase(sql, builder);
        }
    }
}
