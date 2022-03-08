using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    using System.IO;


    public partial class MyDataBase : IMyDataBase
    {
        public void Save(Video video)
        {
            /// <summary>
            /// this method also update both
            /// videoDictionary and Tags in
            /// MyDataBase by the new video.
            /// </summary>
            
            this.videoDictionary.TryAdd(video.VideoId, video);
            video.Save(this.folder + "\\Datas");
            UpdateTags(from tag in video.Tags
                       select tag);
        }
        private Video Load(string videoPath)
        {
            return Video.Load(videoPath);
        }
        public bool Delete(string id)
        {
            try
            {
                File.Delete($"{this.folder}\\Datas\\{id}.video");
                return true;
            }
            catch { return false; } 
        }
    }
}
