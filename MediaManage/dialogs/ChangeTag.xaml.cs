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
using System.Data.SqlClient;
using System.Diagnostics;

namespace MediaManage.dialogs
{
    using classes;
    using subWindow;
    using DataBaseHandler;
    /// <summary>
    /// Interaction logic for ChangeTag.xaml
    /// </summary>
    public partial class ChangeTag : Window
    {
        // waiting to remove
        TextBox tagTextBox;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        SqlConnectionStringBuilder? builder;
        string connectionString;
        string tagString;

        public ChangeTag(TextBox textbox_tags, string connectionString)
        {
            InitializeComponent();
            this.tagTextBox = textbox_tags;
            this.connectionString = connectionString;
            tagString = textbox_tags.Text;
            CheckBoxBindings = new List<CheckBoxBinding>();
            builder = DataBaseHandler.DataBaseBuilder(connectionString);
            if (builder != null)
            {
                string sql = "SELECT TagName FROM VideoTag";
                DataBaseHandler.SQLToDataBase(sql, builder, CheckBoxBindings, this.ReadAllTag);
            }
            this.DataContext = this;
        }

        private void ApplyTagChange(object sender, RoutedEventArgs e)
        {
            var checkedTags =
                from checkBoxBinding in CheckBoxBindings
                where checkBoxBinding.IsChecked == true
                select checkBoxBinding.TagName;
            string tagString = string.Join(',', checkedTags.ToArray());
            this.tagString = tagString;
            tagTextBox.Text = tagString;
        }

        private void Add_newTag(object sender, RoutedEventArgs e)
        {
        }

        private void DeleteTag(object sender, RoutedEventArgs e)
        {
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

        private void ReadAllTag(SqlDataReader reader, List<CheckBoxBinding> CheckBoxBindings)
        {
            var tags = this.tagString.Split(',');
            while (reader.Read())
            {
                string tagName = reader.GetSqlString(0).ToString();
                CheckBoxBindings.Add(new CheckBoxBinding(tagName, tags.Contains(tagName)));
            }
        }
    }
}
