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
            var result = MediaManager.SQL_SearchBy_ITT(builder, youtubeID, title, tagString);
            FoldResultOfITT(connectionString, result);
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

        private void FoldResultOfITT(string connectionString, DataTable dt)
        {
            var groupByYoutubeID = from dataRow in dt.AsEnumerable()
                                   group dataRow by dataRow["YoutubeID"] into mediaInfo
                                   orderby mediaInfo.Key // Key is ytID
                                   select new
                                   {
                                       YoutubeID = mediaInfo.Key.ToString(),
                                       Title = mediaInfo.ElementAt(0)["Title"].ToString(),
                                       Tags = from dataRow in mediaInfo.AsEnumerable()
                                              select dataRow["TagName"]
                                   };
            
            foreach (var result in groupByYoutubeID)
            {
                ResultBindings.Add(new SearchResultBinding(connectionString, 
                                                           result.YoutubeID, 
                                                           result.Title, 
                                                           String.Join(',', result.Tags) ));
            }
        }
    }
}/*, link.NavigateUri.AbsoluteUri*/