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
using System.Collections.ObjectModel;

namespace MediaManage.dialogs
{
    using classes;
    using subWindow;
    /// <summary>
    /// Interaction logic for ChangeTag.xaml
    /// </summary>
    public partial class ChangeTag : Window
    {
        TextBox tagTextBox;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        public ChangeTag(MyDataBase db, CreateWindow cw)
        {
            InitializeComponent();
            if (db == null)
            {
                CheckBoxBindings = new List<CheckBoxBinding>(){ 
                                           new CheckBoxBinding("You", false),
                                           new CheckBoxBinding("haven't", false),
                                           new CheckBoxBinding("select", false),
                                           new CheckBoxBinding("a", false),
                                           new CheckBoxBinding("database", false)};
                cw.TextBox_Tags.Text = "";
                cw.CheckBoxBindings = this.CheckBoxBindings;
            }
            else
            {
                this.CheckBoxBindings = cw.CheckBoxBindings;
            }
            this.DataContext = this;
            this.tagTextBox = cw.TextBox_Tags;
        }

        public ChangeTag(List<MyDataBase> dbs, ReadWindow rw)
        {
            InitializeComponent();
            if (dbs.Count == 0)
            {
                CheckBoxBindings = new List<CheckBoxBinding>(){
                                           new CheckBoxBinding("You", false),
                                           new CheckBoxBinding("haven't", false),
                                           new CheckBoxBinding("select", false),
                                           new CheckBoxBinding("a", false),
                                           new CheckBoxBinding("database", false)};
                rw.TextBox_Tags.Text = "";
                rw.CheckBoxBindings = this.CheckBoxBindings;
            }
            else
            {
                this.CheckBoxBindings = rw.CheckBoxBindings;
            }
            this.DataContext = this;
            this.tagTextBox = rw.TextBox_Tags;
        }

        public ChangeTag(MyDataBase db, UpdateData ud)
        {
            InitializeComponent();
            if (db == null)
            {
                CheckBoxBindings = new List<CheckBoxBinding>(){
                                           new CheckBoxBinding("You", false),
                                           new CheckBoxBinding("haven't", false),
                                           new CheckBoxBinding("select", false),
                                           new CheckBoxBinding("a", false),
                                           new CheckBoxBinding("database", false)};
                ud.TagString = "";
                ud.CheckBoxBindings = this.CheckBoxBindings;
            }
            else
            {
                this.CheckBoxBindings = ud.CheckBoxBindings;
            }
            this.DataContext = this;
            this.tagTextBox = ud.TextBox_Tags;
        }

        private void ApplyTagChange(object sender, RoutedEventArgs e)
        {
            var trueTags =
                from binding in CheckBoxBindings
                where binding.IsChecked == true
                select $"{binding.TagName}";
            string tagString = string.Join(',', trueTags.ToArray());
            this.tagTextBox.Text = tagString;
            this.Close();
        }

        private void Add_newTag(object sender, RoutedEventArgs e)
        {
            string newTag = this.TextBox_NewTag.Text;
            foreach (CheckBoxBinding binding in CheckBoxBindings)
                if (binding.TagName == newTag)
                    return;
            CheckBoxBindings.Add(new CheckBoxBinding(newTag, false));
            UpdateListBox();
        }

        private void DeleteTag(object sender, RoutedEventArgs e)
        {
            if (sender is not Button bn) return;
            if (bn.DataContext is not CheckBoxBinding source) return;
            CheckBoxBindings.Remove(source);
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            this.ListBox.Items.Refresh();
        }

        private void Check_Tag(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb) return;
            if (cb.DataContext is not CheckBoxBinding source) return;
            source.IsChecked = true;
        }

        private void Uncheck_Tag(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb) return;
            if (cb.DataContext is not CheckBoxBinding source) return;
            source.IsChecked = false;
        }
    }
}
