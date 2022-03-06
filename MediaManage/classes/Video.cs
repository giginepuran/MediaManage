using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MediaManage.classes
{
    using System.Diagnostics;
    public class Video
    {
        public string VideoId { get; private set; }
        public string Title { get; private set; }
        public string Thumbnail { get; private set; }
        public string Location { get; private set; }
        public List<Tag> Tags;
        

        public Video(string videoId, string title, string thumbnail, 
                     string location, params string[] tags)
        {
            this.VideoId = videoId;
            this.Title = title;
            this.Thumbnail = thumbnail;
            this.Location = location;
            this.Tags = new List<Tag>();
            foreach (string tagName in tags)
            {
                if (tagName == "") continue;
                Tags.Add(new Tag(tagName));
            }
                
        }

        public bool ValueEquals(object obj)
        {
            var other = obj as Video;

            if (other == null)
                return false;

            if (other.VideoId != this.VideoId || 
                other.Thumbnail != this.Thumbnail ||
                other.Title != this.Title ||
                !other.Location.Equals(this.Location) ||
                other.Tags.Count != this.Tags.Count)
                return false;

            foreach (Tag tag in other.Tags)
            {
                if (!this.Tags.Contains(tag))
                    return false;
            }

            return true;
        }


        public void Save(string folder)
        {
            using (FileStream fs = File.Create($"{folder}\\{VideoId}.video"))
            {
                var tagNames = from tag in Tags
                               select tag.TagName;
                string tagInfo = "";
                foreach (string tagName in tagNames)
                    tagInfo += $"\"{tagName}\",";
                if (tagInfo == "")
                    tagInfo = "\"\"";
                else
                    tagInfo = tagInfo.Remove(tagInfo.Length - 1);
                
                string data = $"{{\"VideoId\":\"{VideoId}\"}}\n" +
                              $"{{\"Title\":\"{Title}\"}}\n" +
                              $"{{\"Thumbnail\":\"{Thumbnail}\"}}\n" +
                              $"{{\"Location\":\"{Location}\"}}\n" +
                              $"{{\"Tags\":{tagInfo}}}";
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                // Add video information.
                fs.Write(info, 0, info.Length);
            }
        }

        public static Video Load(string folder, string id)
        {
            string filePath = $"{folder}\\{id}.video";
            return Video.Load(filePath);
        }
        public static Video Load(string filePath)
        {
            //try
            //{
                string[] infos;
                using (StreamReader sr = File.OpenText(filePath))
                {
                    Debug.WriteLine(filePath);
                    infos = sr.ReadToEnd().Split("\n");
                }
                string videoId = "";
                string title = "";
                string thumbnail = "";
                string location = "";
                string[] tags = { };

                foreach (string info in infos)
                {
                    var sep = info.Split("\":\"");
                    if (sep.Length != 2)
                        return null;
                    switch (sep[0])
                    {
                        case "{\"VideoId":
                            videoId = sep[1].Remove(sep[1].Length-2, 2);
                            break;
                        case "{\"Title":
                            title = sep[1].Remove(sep[1].Length - 2, 2);
                            break;
                        case "{\"Thumbnail":
                            thumbnail = sep[1].Remove(sep[1].Length - 2, 2);
                            break;
                        case "{\"Location":
                            location = sep[1].Remove(sep[1].Length - 2, 2);
                            break;
                        case "{\"Tags":
                            sep[1] = sep[1].Replace("\"", "").Replace("}","");
                            tags = sep[1].Split(',');
                            break;
                        default:
                            return null;
                    }
                }
                return new Video(videoId, title, thumbnail, location, tags);
            //}
            //catch { return null; }
        }
    }
}
