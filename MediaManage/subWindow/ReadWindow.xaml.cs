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
    using System.IO;
    using classes;
    using dialogs;
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        internal List<MyDataBase> dbList;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        public ReadWindow()
        {
            dbList = new List<MyDataBase>();
            CheckBoxBindings = new List<CheckBoxBinding>();
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string id = this.TextBox_ID.Text;
            string subTitle = this.TextBox_Title.Text;
            var containTags = (from bindings in CheckBoxBindings
                               where bindings.IsChecked == true
                               select new Tag(bindings.TagName)).ToList();

            List < (MyDataBase, Video) > results = new List<(MyDataBase, Video)>();
            foreach (MyDataBase db in dbList)
            {
                var result = db.SearchByID(id);
                result = db.SearchByTag(containTags, result);
                result = db.SearchByTitle(subTitle, result);

                foreach (Video video in result)
                    results.Add((db, video));
            }
            SearchResult sr = new SearchResult(results);
            sr.ShowDialog();
        }

        private void Load_DataBase(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            tb.Foreground = Brushes.Black;

            // initialize when databases are changed
            dbList = new List<MyDataBase>();
            CheckBoxBindings = new List<CheckBoxBinding>();

            var dbs = from path in tb.Text.Split(';')
                       where MyDataBase.IsDatabase(path)
                       select new MyDataBase(path);
            if (dbs.Count() == 0) return;

            List<Tag> tags = new List<Tag>();
            
            foreach (var db in dbs)
            {
                this.dbList.Add(db);
                var newTags = from tag in db.tags.Values
                              where !tags.Contains(tag)
                              select tag;
                               
                foreach (Tag tag in newTags)
                    tags.Add(tag);
            }
            
            foreach (Tag tag in tags)
            {
                CheckBoxBindings.Add(new CheckBoxBinding(tag.TagName, false));
            }
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.dbList, this);
            tagWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
