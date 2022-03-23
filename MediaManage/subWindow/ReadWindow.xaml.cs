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
using System.Data.SqlClient;

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
        string connectionString;
        public ReadWindow()
        {
            InitializeComponent();
            //connectionString = "Server=localhost;Database=MediaManager;Integrated Security=True;";
            connectionString = this.TextBox_ConnectionString.Text;
        }

        

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchResult sr = new SearchResult(connectionString, 
                this.TextBox_ID.Text, this.TextBox_Title.Text, this.TextBox_Tags.Text);
            sr.ShowDialog();
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.TextBox_Tags, connectionString);
            tagWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
