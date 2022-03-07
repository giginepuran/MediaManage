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
        /// <summary>
        /// SearchByID only search exact id from dictionary,
        /// if id is empty return all video
        /// </summary>
        /// <param name="id">
        /// id = exact id string or "" empty string for all video in this db
        /// </param>
        public IEnumerable<Video> SearchByID(string id)
        {
            // id is empty string return all video
            if (id == "")
                return this.videoDictionary.Values.AsEnumerable();
            
            // something wrong return empty enum
            var result = Enumerable.Empty<Video>();
            if (id == null) return result;
            
            // search exact id string by dictionary
            Video video;
            if (!this.videoDictionary.TryGetValue(id, out video))
                return result; // empty enum

            // enum with only one element
            return (new List<Video> { video }).AsEnumerable();
        }

        // SearchByTitle overloads
        public IEnumerable<Video> SearchByTitle(string subTitle)
        {
            var result = from video in this.videoDictionary.Values
                         where video.Title.ToLower().Contains(subTitle.ToLower())
                         select video;
            return result;
        }
        public IEnumerable<Video> SearchByTitle(string subTitle, IEnumerable<Video> source)
        {
            var result = source.Where(video => video.Title.ToLower().Contains(subTitle.ToLower()));
            return result;
        }

        // SearchByTag overloads
        public IEnumerable<Video> SearchByTag(Tag tag)
        {
            var result = from video in this.videoDictionary.Values
                         where video.Tags.Contains(tag)
                         select video;
            return result;
        }
        public IEnumerable<Video> SearchByTag(Tag tag, IEnumerable<Video> source)
        {
            var result = source.Where(video => video.Tags.Contains(tag));
            return result;
        }
        public IEnumerable<Video> SearchByTag(List<Tag> tags)
        {
            var result =
                        from video in this.videoDictionary.Values
                        where video.Tags.Intersect(tags).Count() == tags.Count()
                        select video;
            return result;
        }
        public IEnumerable<Video> SearchByTag(List<Tag> tags, IEnumerable<Video> source)
        {
            var result = source.Where(
                         video => video.Tags.Intersect(tags).Count() == tags.Count());
            return result;
        }
    }
}
