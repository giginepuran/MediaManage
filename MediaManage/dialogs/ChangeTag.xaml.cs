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
        public List<Tag> TagLabels { get; set; }
        public ChangeTag(MyDataBase db)
        {
            InitializeComponent();
            if (db == null)
                TagLabels = new List<Tag> { new Tag("Tag A"), new Tag("Tag B"), new Tag("Tag C") };
            else
                TagLabels = db.Tags;
            this.DataContext = this;
        }

        private void ApplyTagChange(object sender, RoutedEventArgs e)
        {

        }

        private void Add_newTag(object sender, RoutedEventArgs e)
        {
            Tag newTag = new Tag(this.TextBox_NewTag.Text);
            if (TagLabels.Contains(newTag))
                return;
            TagLabels.Add(newTag);
            this.ListBox.Items.Refresh();
        }
    }
}
