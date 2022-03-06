using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public interface IMyDataBase
    {
        void Save(Video video);
        IEnumerable<Video> SearchByTitle(string subTitle);
        IEnumerable<Video> SearchByTag(Tag tag);
        Video SearchByID(string id);
        List<Tag> GetTags();
    }
}
