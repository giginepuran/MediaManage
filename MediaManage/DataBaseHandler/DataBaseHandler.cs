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
    public partial class DataBaseHandler
    {
        public static SqlConnectionStringBuilder DataBaseBuilder(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                return builder;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static void SQLToDataBase<T>(string sql, SqlConnectionStringBuilder builder, T obj, Action<SqlDataReader,T> Action)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Action(reader, obj);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void SQLToDataBase(string sql, SqlConnectionStringBuilder builder, DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dataTable);
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void SQLToDataBase(string sql, SqlConnectionStringBuilder builder)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
