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

namespace MediaManage.subWindow
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    using System.IO;
    using classes;
    using dialogs;

    public partial class CreateWindow : Window
    {
        internal MyDataBase db;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }

        public CreateWindow()
        {
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(db, this);
            tagWindow.ShowDialog();
        }

        private bool CheckID()
        {
            if (db == null) return false;
            if (db.Load(this.TextBox_ID.Text) == null)
                return true;
            return false;
        }

        private void CheckAll(object sender, RoutedEventArgs e)
        {
            bool id_ok = CheckID();
            bool title_ok = this.TextBox_Title.Text != "";
            bool thumbnail_ok = this.TextBox_Thumbnail.Text == this.TextBox_ID.Text ||
                                File.Exists(this.TextBox_Thumbnail.Text);
            bool location_ok = this.TextBox_Location.Text == this.TextBox_ID.Text ||
                               File.Exists(this.TextBox_Thumbnail.Text);
            bool db_ok = db != null;

            this.TextBox_ID.Foreground = id_ok ? Brushes.Green : Brushes.Red;
            this.TextBox_Title.Foreground = title_ok ? Brushes.Green : Brushes.Red;
            this.TextBox_Thumbnail.Foreground = thumbnail_ok ? Brushes.Green : Brushes.Red;
            this.TextBox_Location.Foreground = location_ok ? Brushes.Green : Brushes.Red;
            this.TextBox_Database.Foreground = db_ok ? Brushes.Green : Brushes.Red;

            if (id_ok && title_ok && thumbnail_ok && db_ok && location_ok)
            {
                this.TextBox_ID.IsReadOnly = true;
                this.TextBox_Title.IsReadOnly = true;
                this.TextBox_Thumbnail.IsReadOnly = true;
                this.TextBox_Location.IsReadOnly = true;
                this.TextBox_Database.IsReadOnly = true;
                this.Button_ChangeTag.IsEnabled = false;
                this.Button_Reset.IsEnabled = true;
                this.Button_Add.IsEnabled = true;
            }
        }

        private void Load_DataBase(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (!Directory.Exists(tb.Text)) return;
            tb.Foreground = Brushes.Black;

            db = new MyDataBase(tb.Text);
            string tagString = this.TextBox_Tags.Text;
            List<Tag> tags = db.GetTags();
            var checkRepeat =
                from tag in tags
                select new CheckBoxBinding(tag.TagName, tagString.Split(',').Contains(tag.TagName));

            CheckBoxBindings = checkRepeat.ToList();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Default_All();
        }

        private void Add2DB(object sender, RoutedEventArgs e)
        {
            string videoID = this.TextBox_ID.Text;
            string title = this.TextBox_Title.Text;
            string thumbnail = this.TextBox_Thumbnail.Text;
            string location = this.TextBox_Location.Text;
            string tags = this.TextBox_Tags.Text;
            Video newVideo = new Video(videoID, title, thumbnail, location, tags.Split(','));
            db.Save(newVideo);

            Default_All();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            tb.Foreground = Brushes.Black;
        }

        private void Default_All()
        {
            this.TextBox_ID.Foreground = Brushes.Black;
            this.TextBox_Title.Foreground = Brushes.Black;
            this.TextBox_Thumbnail.Foreground = Brushes.Black;
            this.TextBox_Location.Foreground = Brushes.Black;
            this.TextBox_Database.Foreground = Brushes.Black;

            this.TextBox_ID.IsReadOnly = false;
            this.TextBox_Title.IsReadOnly = false;
            this.TextBox_Thumbnail.IsReadOnly = false;
            this.TextBox_Location.IsReadOnly = false;
            this.TextBox_Database.IsReadOnly = false;
            this.Button_ChangeTag.IsEnabled = true;
            this.Button_Reset.IsEnabled = false;
            this.Button_Add.IsEnabled = false;
        }

        private void TextBox_ID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (tb.Text.Length==11)
            {
                this.TextBox_Thumbnail.Text = tb.Text;
                this.TextBox_Location.Text = tb.Text;
            }
            TextBox_TextChanged(tb, e);
        }
    }
}