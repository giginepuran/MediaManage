using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public partial class MyDataBase : IMyDataBase
    {
        internal string folder { get; set; }
        internal Dictionary<string, Tag> tags { get; set; }
        internal Dictionary<string, Video> videoDictionary;

        public MyDataBase(string folder)
        {
            this.folder = folder;
            this.tags = new Dictionary<string, Tag>();
            this.videoDictionary = new Dictionary<string, Video>();
            LoadVideoDictionary();
            LoadTags();
        }
    }
}
