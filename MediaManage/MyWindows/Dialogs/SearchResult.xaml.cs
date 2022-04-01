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

namespace MediaManage.Dialogs
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    
    using Classes;
    using DataBaseHandler;
    public partial class SearchResult : Window
    {
        public List<Info> Infos { get; set; }

        public SearchResult(string[] connStrs, string youtubeID, string title, string tagString)
        {
            Infos = new List<Info>();
            foreach (string connectionString in connStrs)
            {
                SqlConnectionStringBuilder builder = DataBaseHandler.DataBaseBuilder(connectionString);
                var result = MediaManager.SQL_SearchBy_ITT(builder, youtubeID, title, tagString);
                FoldResultOfITT(connectionString, result);
            }
            InitializeComponent();
            this.DataContext = this;
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is not Hyperlink hl) return;
            if (hl.DataContext is not Info info) return;
            Process.Start(new ProcessStartInfo($"https://youtu.be/{info.YoutubeID}") { UseShellExecute = true });
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow dgr) return;
            if (dgr.DataContext is not Info info) return;
            UpdateData ud = new UpdateData(info, this);
            ud.Show();
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
                Infos.Add(new Info
                {
                    ConnectionString = connectionString,
                    YoutubeID = result.YoutubeID,
                    Title = result.Title,
                    TagString = String.Join(',', result.Tags),
                    ThumbnailUrl = $"https://i.ytimg.com/vi/{result.YoutubeID}/hqdefault.jpg"
                });
            }
        }
    }
}