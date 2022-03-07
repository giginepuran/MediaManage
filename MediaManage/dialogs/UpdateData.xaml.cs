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

namespace MediaManage.dialogs
{
    /// <summary>
    /// Interaction logic for UpdateData.xaml
    /// </summary>

    using classes;
    public partial class UpdateData : Window
    {
        // property for binding
        public string DBPath { get; set; }
        public string IdDealWith { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoTitle { get; set; }
        public string Location { get; set; }
        public string TagString { get; set; }
        //
        private MyDataBase DB;
        private Video video;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        public UpdateData(MyDataBase db, string id)
        {
            InitializeComponent();
            DB = db;
            video = db.videoDictionary[id];

            DBPath = db.folder;
            IdDealWith = id;
            ThumbnailUrl = $"https://i.ytimg.com/vi/{id}/hqdefault.jpg";
            VideoTitle = video.Title;
            Location = video.Location;
            TagString = String.Join(",",(from tag in video.Tags
                                         select tag.TagName).ToArray());
            CheckBoxBindings = (from tag in db.GetTags()
                                select new CheckBoxBinding(tag.TagName, TagString.Contains(tag.TagName)))
                               .ToList();

            DataContext = this;
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.DB, this);
            tagWindow.ShowDialog();
        }

        private void ApplyUpdate(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
