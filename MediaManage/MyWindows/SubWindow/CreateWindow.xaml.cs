using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace MediaManage.SubWindow
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    using System.IO;
    using Classes;
    using Dialogs;
    using DataBaseHandler;

    public partial class CreateWindow : Window
    {
        public Info Info { get; set; }
        private SqlConnectionStringBuilder builder;


        public CreateWindow()
        {
            InitializeComponent();
            Info = new Info();
            Info.ConnectionString = "Server=localhost;Database=MediaManager;Integrated Security=True;";
            Info.YoutubeID = "";
            Info.Title = "";
            Info.ThumbnailUrl = "";
            Info.TagString = "";
            DataContext = this;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.TextBox_Tags, Info.ConnectionString);
            tagWindow.ShowDialog();
        }

        private bool CheckID()
        {
            if (builder is null)
                return false;
            string sql = $"SELECT COUNT(YoutubeID) FROM Video WHERE YoutubeID = '{this.TextBox_ID.Text}'";
            string result = DataBaseHandler.SQLToDataBase(sql, builder, reader => { 
                            reader.Read(); 
                            return reader.GetSqlInt32(0).ToString(); });
            if (result == "0")
                return true;
            return false;
        }

        private void CheckInfo(object sender, RoutedEventArgs e)
        {
            // check ConnectionString 
            builder = DataBaseHandler.DataBaseBuilder(Info.ConnectionString);
            if (builder is null)
            {
                this.TextBox_DB.Foreground = Brushes.Red;
                if (this.TextBox_DB.Text == "")
                    this.TextBox_DB.Text = "Invalid Connection String";
                return;
            }
            this.TextBox_DB.Foreground = Brushes.Green;

            // check YTID exists or not, len of YTID must be 11
            bool idOK;
            if (this.TextBox_ID.Text.Length != 11)
            {
                idOK = false;
                this.TextBox_ID.Text += "len of YTID must be 11";
            }
            else
                idOK = CheckID();
            this.TextBox_ID.Foreground = idOK ? Brushes.Green : Brushes.Red;

            // title can't be empty
            bool titleOK = this.TextBox_Title.Text != "";
            this.TextBox_Title.Foreground = titleOK ? Brushes.Green : Brushes.Red;
            if (!titleOK) this.TextBox_Title.Text = "Title can't be empty";

            if (idOK && titleOK)
            {
                
                this.Button_Create.IsEnabled = true;
                this.Button_Unlock.IsEnabled = true;
                this.Button_Check.IsEnabled = false;
                //this.Button_ChangeTag.IsEnabled = false;
                this.TextBox_DB.IsEnabled = false;
                this.TextBox_ID.IsEnabled = false;
                this.TextBox_Title.IsEnabled = false;
            }
        }
        private void UnlockInfo(object sender, RoutedEventArgs e)
        {
            this.Button_Create.IsEnabled = false;
            this.Button_Unlock.IsEnabled = false;
            this.Button_Check.IsEnabled = true;
            //this.Button_ChangeTag.IsEnabled = true;
            this.TextBox_DB.IsEnabled = true;
            this.TextBox_ID.IsEnabled = true;
            this.TextBox_Title.IsEnabled = true;

            this.TextBox_DB.Foreground = Brushes.Black;
            this.TextBox_ID.Foreground = Brushes.Black;
            this.TextBox_Title.Foreground = Brushes.Black;
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            MediaManager.SQL_CreateYTID(builder, Info);
            UnlockInfo(null, null);
        }
    }
}