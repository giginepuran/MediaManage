using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MediaManage.classes
{
    using System.IO;
    public partial class MyDataBase : IMyDataBase
    {
        private void LoadVideoDictionary()
        {
            /// <summary>
            /// this Method will update the existed
            /// videoDictionart in MyDataBase
            /// dependent on .../database/Datas/*.video.
            /// Also, it will update tags in MyDataBase simultaneously.
            /// </summary>

            var videos = from videoFile in Directory.EnumerateFiles($"{this.folder}\\Datas", "*.video")
                         select Load(videoFile);
            var newVideos = from video in videos
                            where !videoDictionary.ContainsKey(video.VideoId)
                            select video;
            foreach (Video video in newVideos)
            {
                this.videoDictionary.Add(video.VideoId, video);
                LoadTags();
            }        
        }

        private void LoadTags()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(this.folder + "\\Tags");
            var newTags = from file in dirInfo.GetFileSystemInfos()
                          select new Tag(file.Name.Replace(".tag",""));
            newTags = from tag in newTags
                      where !this.tags.ContainsKey(tag.TagName)
                      select tag;
            foreach (Tag tag in newTags)
                this.tags.Add(tag.TagName, tag);
        }

        private void UpdateTags(IEnumerable<Tag> tags)
        {
            /// <summary>
            /// Notice: this method used when
            /// adding new video to db only.
            /// this Method will update both
            /// Tags in MyDataBase and
            /// files in .../database/Tags/*.tag
            /// </summary>

            var newTags = from tag in tags
                          where !this.tags.ContainsKey(tag.TagName)
                          select tag;
            foreach (Tag tag in newTags)
            {
                this.tags.Add(tag.TagName, tag);
                using (FileStream fs = File.Create($"{this.folder}\\Tags\\{tag.TagName}.tag")) ;
            }
        }
        public List<Tag> GetTags()
        {
            var tags = from pair in this.tags
                       select pair.Value;
            return tags.ToList();
        }

        public static bool IsDatabase(string path)
        {
            if (!Directory.Exists(path))
                return false;
            if (!Directory.Exists(path + "\\Tags"))
                return false;
            if (!Directory.Exists(path + "\\Datas"))
                return false;
            return true;
        }
    }
}
