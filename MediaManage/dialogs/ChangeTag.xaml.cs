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
    /// <summary>
    /// Interaction logic for ChangeTag.xaml
    /// </summary>
    public partial class ChangeTag : Window
    {
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        public ChangeTag(MyDataBase db, string txtBoxtxt)
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
                ((MainWindow)Application.Current.MainWindow).TextBOX_Tags.Text = "";
            }
            else
            {
                List<Tag> tags = db.GetTags();
                var checkRepeat =
                    from tag in tags
                    select new CheckBoxBinding(tag.TagName, txtBoxtxt.Split(',').Contains(tag.TagName));

                CheckBoxBindings = checkRepeat.ToList();
            }
            this.DataContext = this;
        }

        private void ApplyTagChange(object sender, RoutedEventArgs e)
        {
            var trueTags =
                from binding in CheckBoxBindings
                where binding.IsChecked == true
                select $"{binding.TagName}";
            string tagString = string.Join(',', trueTags.ToArray());
            ((MainWindow)Application.Current.MainWindow).TextBOX_Tags.Text = tagString;
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
