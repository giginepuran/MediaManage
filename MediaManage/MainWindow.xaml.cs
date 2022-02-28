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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaManage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.IO;
    using classes;
    using dialogs;

    public partial class MainWindow : Window
    {
        internal MyDataBase db;

        public MainWindow()
        {            
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeTag(object sender, RoutedEventArgs e)
        {
            ChangeTag tagWindow = new ChangeTag(db);
            tagWindow.Show();
        }

        private void CheckID(object sender, RoutedEventArgs e)
        {
            if (db == null) return;
            Notification notiWindow = new Notification();
            if (db.Load(this.TextBox_ID.Text) == null)
                notiWindow.Information.Text = "this videoID is okay";
            else
                notiWindow.Information.Text = "this videoID is repeated";
            notiWindow.Show();
        }

        private void CheckAll(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_GetFolder(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (!Directory.Exists(tb.Text)) return;
            db = new MyDataBase(tb.Text);
        }

        private void Add2DB(object sender, RoutedEventArgs e)
        {

        }
    }
}
