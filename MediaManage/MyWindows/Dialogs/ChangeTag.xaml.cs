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

namespace MediaManage.Dialogs
{
    using Classes;
    using SubWindow;
    using DataBaseHandler;
    /// <summary>
    /// Interaction logic for ChangeTag.xaml
    /// </summary>
    public partial class ChangeTag : Window
    {
        TextBox tagTextBox;
        public List<string> UnSelectedList { get; set; }
        public List<String> SelectedList { get; set; }
        string tagString;

        public ChangeTag(TextBox textbox_tags, string connectionString)
        {
            InitializeComponent();
            this.tagTextBox = textbox_tags;
            tagString = textbox_tags.Text;
            UnSelectedList = new List<string>();
            SelectedList = new List<string>();

            SqlConnectionStringBuilder builder = DataBaseHandler.DataBaseBuilder(connectionString);
            if (builder != null)
            {
                string sql = "SELECT TagName FROM VideoTag";
                MediaManager.MergeTag(builder, UnSelectedList);
            }
            DistributeTag();
            this.DataContext = this;
        }

        public ChangeTag(TextBox textbox_tags, string[] connStrs)
        {
            InitializeComponent();
            this.tagTextBox = textbox_tags;
            tagString = textbox_tags.Text;
            UnSelectedList = new List<string>();
            SelectedList = new List<string>();

            foreach (string connectionString in connStrs)
            {
                SqlConnectionStringBuilder builder = DataBaseHandler.DataBaseBuilder(connectionString);
                if (builder != null)
                {
                    string sql = "SELECT TagName FROM VideoTag";
                    MediaManager.MergeTag(builder, UnSelectedList);
                }
            }
            DistributeTag();
            this.DataContext = this;
        }

        private void ApplyTagChange(object sender, RoutedEventArgs e)
        {
            string tagString = string.Join(',', SelectedList);
            this.tagString = tagString;
            tagTextBox.Text = tagString;
            this.Close();
        }

        private void NewTag(object sender, RoutedEventArgs e)
        {
            string newTag = this.TextBox_NewTag.Text;
            if (!UnSelectedList.Contains(newTag) && !SelectedList.Contains(newTag))
            {
                UnSelectedList.Insert(0, newTag);
            }
            this.ListBox_UnSelect.Items.Refresh();
        }

        private void DistributeTag()
        {
            var tags = tagString.Split(',');
            foreach (string tag in tags)
            {
                if (tag == "") continue;
                if (UnSelectedList.Contains(tag))
                {
                    SelectedList.Add(tag);
                    UnSelectedList.Remove(tag);
                }
                else if (!SelectedList.Contains(tag))
                {// new tag separated with db before CUD to DB
                    SelectedList.Add(tag);
                }
            }
        }

        private void UnSelectTag(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Label lbl) return;
            if (lbl.Tag is not string tag) return;
            SelectedList.Remove(tag);
            UnSelectedList.Insert(0, tag);
            this.ListBox_UnSelect.Items.Refresh();
            this.ListBox_Select.Items.Refresh();
        }

        private void SelectTag(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Label lbl) return;
            if (lbl.Tag is not string tag) return;
            UnSelectedList.Remove(tag);
            SelectedList.Insert(0, tag);
            this.ListBox_UnSelect.Items.Refresh();
            this.ListBox_Select.Items.Refresh();
        }
    }
}
