using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    using System.IO;

    public interface IMyDataBase
    {
        void Save(Video video);
        Video Load(string folder);
    }
    public class MyDataBase:IMyDataBase
    {
        public string Folder { get; set; }
        public List<Tag> Tags { get; set; }
        public MyDataBase(string folder)
        {
            this.Folder = folder;
            var files = from tagFile in Directory.EnumerateFiles($"{folder}\\Tags", "*.tag")
                        select new Tag(Path.GetFileName(tagFile).Split('.')[0]);
            Tags = files.ToList();
        }
        public void Save(Video video)
        {
            video.Save(Folder);
            foreach (Tag tag in video.Tags)
            {
                if (!this.Tags.Contains(tag))
                {
                    this.Tags.Add(tag);
                    using (FileStream fs = File.Create($"{Folder}\\Tags\\{tag.TagName}.tag")) ;
                }
            }
        }
        public Video Load(string videoId)
        {
            return Video.Load(Folder, videoId);
        }
    }
}
