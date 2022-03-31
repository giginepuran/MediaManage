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

namespace MediaManage.dialogs
{
    /// <summary>
    /// Interaction logic for UpdateData.xaml
    /// </summary>

    using classes;
    using DataBaseHandler;
    public partial class UpdateData : Window
    { 
        public Info Info { get; set; }
        private Info oriInfo;
        private Window parent;
        public UpdateData(Info info, Window parent)
        {
            InitializeComponent();
            this.parent = parent;
            oriInfo = info;
            Info = new Info();
            Info.ConnectionString = info.ConnectionString;
            Info.YoutubeID = info.YoutubeID;
            Info.Title = info.Title;
            Info.ThumbnailUrl = info.ThumbnailUrl;
            Info.TagString = info.TagString;
            DataContext = this;
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.TextBox_Tags, Info.ConnectionString);
            tagWindow.ShowDialog();
            CheckDiff(this.TextBox_Tags, null);
        }
        // compare to original info
        private void CheckDiff(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            bool isSame;
            switch (tb.Name)
            {
                case "TextBox_ID":
                    isSame = oriInfo.YoutubeID == tb.Text;
                    break;
                case "TextBox_Title":
                    isSame = oriInfo.Title == tb.Text;
                    break;
                case "TextBox_Tags":
                    isSame = CheckTagExactlySame(oriInfo.TagString, tb.Text);
                    break;
                default:
                    Debug.Write("This method should be used by TextBox_Title/Location/Tags only.\n" +
                                "It do not be running to here\n");
                    Debug.Assert(false);
                    return;
            }
            if (!isSame) tb.Foreground = Brushes.Orange;
            else tb.Foreground = Brushes.Black;
        }

        private bool CheckTagExactlySame(string tagString1, string tagString2)
        {
            string[] tags1 = tagString1.Split(','), tags2 = tagString2.Split(',');
            var intersect = tags1.Intersect(tags2);
            return intersect.Count() == tags1.Count() && intersect.Count() == tags2.Count();
        }

        private void ResetInfo(object sender, RoutedEventArgs e)
        {
            Info.ConnectionString = oriInfo.ConnectionString;
            Info.YoutubeID = oriInfo.YoutubeID;
            Info.Title = oriInfo.Title;
            Info.ThumbnailUrl = oriInfo.ThumbnailUrl;
            Info.TagString = oriInfo.TagString;
        }

        private void DeleteVideo(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Info.ConnectionString);
            MediaManager.SQL_DeleteYTID(builder, oriInfo.YoutubeID);
            if (this.parent is SearchResult sr)
            {
                var testing = sr.Infos.Remove(oriInfo);
                sr.InfosGrid.Items.Refresh();
            }
            this.Close();
        }

        private void ApplyUpdate(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Info.ConnectionString);
            MediaManager.SQL_UpdateInfo(builder, oriInfo, Info);
            if (this.parent is SearchResult sr)
            {
                oriInfo.YoutubeID = Info.YoutubeID;
                oriInfo.Title = Info.Title;
                oriInfo.ThumbnailUrl = Info.ThumbnailUrl;
                oriInfo.TagString = Info.TagString;
                sr.InfosGrid.Items.Refresh();
            }
            this.TextBox_ID.Foreground = Brushes.Black;
            this.TextBox_Title.Foreground = Brushes.Black;
            this.TextBox_Tags.Foreground = Brushes.Black;
        }
    }
}
