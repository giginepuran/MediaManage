using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    using System.IO;
    public partial class MyDataBase:IMyDataBase
    {
        
        public IEnumerable<Video> SearchByTitle(string subTitle)
        {
            var result = from video in this.videoDictionary.Values
                         where video.Title.Contains(subTitle)
                         select video;
            return result;
        }
        public IEnumerable<Video> SearchByTag(Tag tag)
        {
            var result = from video in this.videoDictionary.Values
                         where video.Tags.Contains(tag)
                         select video;
            return result;
        }
        public Video SearchByID(string id)
        {
            Video video;
            this.videoDictionary.TryGetValue(id, out video);
            return video;
        }
    }
}
