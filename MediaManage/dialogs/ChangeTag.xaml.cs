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
    /// <summary>
    /// Interaction logic for ChangeTag.xaml
    /// </summary>
    public partial class ChangeTag : Window
    {
        // waiting to remove
        TextBox tagTextBox;
        public List<CheckBoxBinding> CheckBoxBindings { get; set; }
        SqlConnectionStringBuilder builder;
        string tagString;
        public ChangeTag(MyDataBase db, CreateWindow cw)
        {
            // not yet
        }

        public ChangeTag(TextBox textbox_tags, string connectionString)
        {
            InitializeComponent();
            this.CheckBoxBindings = new List<CheckBoxBinding>();
            bool dbConnected;
            // read all tag in DB
            try
            {
                builder = new SqlConnectionStringBuilder(connectionString);
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT TagName FROM VideoTag";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.CheckBoxBindings.Add(new CheckBoxBinding(reader.GetSqlString(0).ToString(), false));
                                Debug.WriteLine("TagName: {0}", reader.GetSqlString(0));
                            }
                        }
                    }
                }
                
                dbConnected = true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                dbConnected = false;
            }


            this.DataContext = this;
            this.tagString = textbox_tags.Text;
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
