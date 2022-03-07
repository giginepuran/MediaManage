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
    using subWindow;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
        }
        private void CreateMode(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.ShowDialog();     
        }

        private void ReadMode(object sender, RoutedEventArgs e)
        {
            ReadWindow readWindow = new ReadWindow();
            readWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
