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
using System.Diagnostics;

namespace MediaManage.dialogs
{
    /// <summary>
    /// Interaction logic for UpdateData.xaml
    /// </summary>

    using classes;
    public partial class UpdateData : Window
    { 
        public Info Info { get; set; }
        private SearchResultBinding oriInfo;
        public UpdateData(SearchResultBinding info)
        {
            InitializeComponent();
            oriInfo = info;
            Info = new Info();
            Info.ConnectionString = info.DB;
            Info.YoutubeID = info.ID;
            Info.Title = info.Title;
            Info.ThumbnailUrl = $"https://i.ytimg.com/vi/{info.ID}/hqdefault.jpg";
            Info.TagString = info.Tags;
            DataContext = this;
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.TextBox_Tags, Info.ConnectionString);
            tagWindow.ShowDialog();
            CheckDiff(this.TextBox_Tags, null);
        }

        private void ApplyUpdate(object sender, RoutedEventArgs e)
        {
            return;
        }
        // compare to original info
        private void CheckDiff(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            bool isSame;
            switch (tb.Name)
            {
                case "TextBox_ID":
                    isSame = oriInfo.ID == tb.Text;
                    break;
                case "TextBox_Title":
                    isSame = oriInfo.Title == tb.Text;
                    break;
                case "TextBox_Tags":
                    isSame = CheckTagExactlySame(oriInfo.Tags, tb.Text);
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
            Info.ConnectionString = oriInfo.DB;
            Info.YoutubeID = oriInfo.ID;
            Info.Title = oriInfo.Title;
            Info.ThumbnailUrl = $"https://i.ytimg.com/vi/{oriInfo.ID}/hqdefault.jpg";
            Info.TagString = oriInfo.Tags;
        }

        private void DeleteVideo(object sender, RoutedEventArgs e)
        {
            
        }

        private void Update(object sender, DataTransferEventArgs e)
        {
            
        }
    }
}
