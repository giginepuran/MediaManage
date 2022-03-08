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
        // property for binding with XAML (readonly)
        public string DBPath { get; set; }
        public string IdDealWith { get; set; }
        public string ThumbnailUrl { get; set; }
        // property for binding with XAML (not readonly)
        public string VideoTitle { get; set; }
        public string Location { get; set; }
        public string TagString { get; set; }
        //
        private MyDataBase DB;
        public Video originalVideo { get; set; }
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        public UpdateData(MyDataBase db, string id)
        {
            InitializeComponent();
            DB = db;
            originalVideo = db.videoDictionary[id];

            DBPath = db.folder;
            IdDealWith = id;
            ThumbnailUrl = $"https://i.ytimg.com/vi/{id}/hqdefault.jpg";
            VideoTitle = originalVideo.Title;
            Location = originalVideo.Location;
            TagString = originalVideo.GetTagString();
            CheckBoxBindings = (from tag in db.GetTags()
                                select new CheckBoxBinding(tag.TagName, TagString.Contains(tag.TagName)))
                               .ToList();

            DataContext = this;
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.DB, this);
            tagWindow.ShowDialog();
            CheckDiff(this.TextBox_Tags, null);
        }

        private void ApplyUpdate(object sender, RoutedEventArgs e)
        {
            Video newVideo = new Video(IdDealWith, VideoTitle, IdDealWith, Location, TagString.Split(','));
            DB.Save(newVideo);
            return;
        }

        /// <summary>
        /// This method used to check 
        /// whether the video info in TextBox
        /// is different to original Video object.
        /// </summary>
        private void CheckDiff(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            bool isSame;
            switch (tb.Name)
            {
                case "TextBox_Title":
                    isSame = originalVideo.Title == tb.Text;
                    break;
                case "TextBox_Location":
                    isSame = originalVideo.Location == tb.Text;
                    break;
                case "TextBox_Tags":
                    isSame = CheckTagExactlySame(originalVideo.GetTagString(), tb.Text);
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
            bool isSame;
            string[] tags1 = tagString1.Split(','), tags2 = tagString2.Split(',');
            var intersect = tags1.Intersect(tags2);
            return intersect.Count() == tags1.Count() && intersect.Count() == tags2.Count();
        }

        private void ResetInfo(object sender, RoutedEventArgs e)
        {
            VideoTitle = originalVideo.Title;
            Location = originalVideo.Location;
            TagString = originalVideo.GetTagString();
            CheckBoxBindings = (from tag in DB.GetTags()
                                select new CheckBoxBinding(tag.TagName, TagString.Contains(tag.TagName)))
                               .ToList();
        }

        private void DeleteVideo(object sender, RoutedEventArgs e)
        {
            DB.Delete(IdDealWith);
            this.Close();
        }
    }
}
