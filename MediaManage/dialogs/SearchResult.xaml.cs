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

namespace MediaManage.dialogs
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    
    using classes;
    using System.Diagnostics;
    public partial class SearchResult : Window
    {
        public List<SearchResultBinding> ResultBindings { get; set; }
        public SearchResult(List<(MyDataBase, Video)> results)
        {
            ResultBindings = new List<SearchResultBinding>();
            InitializeComponent();
            var bindings =
                from result in results
                select new SearchResultBinding(result.Item1.folder, 
                                               result.Item2.VideoId,
                                               result.Item2.Title,
                                               result.Item2.Tags);
            foreach (var binding in bindings)
                ResultBindings.Add(binding);
            this.DataContext = this;
        }
        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is not Hyperlink hl) return;
            if (hl.DataContext is not SearchResultBinding srb) return;
            Process.Start(new ProcessStartInfo($"https://youtu.be/{srb.ID}") { UseShellExecute = true });
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow dgr) return;
            if (dgr.DataContext is not SearchResultBinding srb) return;
            MyDataBase db = new MyDataBase(srb.DB);
            UpdateData ud = new UpdateData(db, srb.ID);
            ud.ShowDialog();
        }
    }
}/*, link.NavigateUri.AbsoluteUri*/