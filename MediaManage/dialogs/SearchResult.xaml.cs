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
using System.Diagnostics;
using System.Data;

namespace MediaManage.dialogs
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    
    using classes;
    using DataBaseHandler;
    public partial class SearchResult : Window
    {
        public List<SearchResultBinding> ResultBindings { get; set; }
        SqlConnectionStringBuilder builder;
        string connectionString;

        public SearchResult(string connectionString, string youtubeID, string title, string tagString)
        {
            this.connectionString = connectionString;
            ResultBindings = new List<SearchResultBinding>();
            builder = DataBaseHandler.DataBaseBuilder(connectionString);
            var table1 = MediaManager.SQL_SearchByID(youtubeID, builder);
            var table2 = MediaManager.SQL_SearchBySubTitle(title, builder);
            var table3 = MediaManager.SQL_SearchByTags(tagString, builder);
            GetResults(table1, table2, table3);
            InitializeComponent();
            this.DataContext = this;
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is not Hyperlink hl) return;
            if (hl.DataContext is not SearchResultBinding srb) return;
            Process.Start(new ProcessStartInfo($"https://youtu.be/{srb.ID}") { UseShellExecute = true });
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow dgr) return;
            if (dgr.DataContext is not SearchResultBinding srb) return;
            MyDataBase db = new MyDataBase(srb.DB);
            UpdateData ud = new UpdateData(db, srb.ID);
            ud.ShowDialog();
        }

        private void GetResults(DataTable dt1, DataTable dt2, DataTable dt3)
        {
            var results = from table1 in dt1.AsEnumerable()
                          join table2 in dt2.AsEnumerable() on table1["YoutubeID"] equals table2["YoutubeID"]
                          join table3 in dt3.AsEnumerable() on table1["YoutubeID"] equals table3["YoutubeID"]
                          select new{ YoutubeID = table1["YoutubeID"].ToString(), 
                                      Title = table1["Title"].ToString() };
            
            foreach (var result in results)
            {
                string tagString = MediaManager.GetAllTagByID(result.YoutubeID, builder);
                ResultBindings.Add(new SearchResultBinding(connectionString, result.YoutubeID, result.Title, tagString));
            }
        }
    }
}/*, link.NavigateUri.AbsoluteUri*/