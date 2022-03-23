using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public class SearchResultBinding
    {
        public string DB { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }

        public SearchResultBinding(string db, string id, string title, string tagString)
        {
            this.DB = db;
            this.ID = id;
            this.Title = title;
            this.Tags = tagString;
        }
    }
}
