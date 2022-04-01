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

namespace MediaManage.SubWindow
{
    using System.IO;
    using Classes;
    using Dialogs;
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow()
        {
            InitializeComponent();
            /* Default connectionStrings: 
             * Server=localhost;Database=MediaManager;Integrated Security=True;,Server=localhost;Database=MM2;Integrated Security=True;
             */
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchResult sr = new SearchResult(TextBox_ConnectionString.Text.Split(','), 
                this.TextBox_ID.Text, this.TextBox_Title.Text, this.TextBox_Tags.Text);
            sr.ShowDialog();
        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(this.TextBox_Tags, TextBox_ConnectionString.Text.Split(','));
            tagWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
