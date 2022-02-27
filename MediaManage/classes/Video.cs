using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MediaManage.classes
{
    public class Video
    {
        public string VideoId { get; private set; }
        public string Title { get; private set; }
        public string Thumbnail { get; private set; }
        public List<Tag> Tags;
        public Location Location;

        public Video(string videoId, string title, string thumbnail, 
                     string address, string addressType = "default", 
                     params string[] tags)
        {
            this.VideoId = videoId;
            this.Title = title;
            this.Thumbnail = thumbnail;
            this.Location = new Location(address, addressType);
            this.Tags = new List<Tag>();
            foreach (string tagName in tags)
                Tags.Add(new Tag(tagName));
        }

        private string GetTags()
        {             
            string tagInfo = "";
            for (int i = 0; i < Tags.Count-1; i++)
                tagInfo += $"\"{Tags[i].TagName}\",";
            tagInfo += $"\"{Tags.Last().TagName}\"";
            return tagInfo;
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
                string data = $"{{\"VideoId\":\"{VideoId}\"}}\n" +
                              $"{{\"Title\":\"{Title}\"}}\n" +
                              $"{{\"Thumbnail\":\"{Thumbnail}\"}}\n" +
                              $"{{\"Address\":\"{Location.Address}\"}}\n" +
                              $"{{\"AddressType\":\"{Location.AddressType}\"}}\n" +
                              $"{{\"Tags\":{GetTags()}}}";
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public static Video Load(string folder, string id)
        {
            string[] infos;
            // Open the stream and read it back.
            try
            {
                using (StreamReader sr = File.OpenText($"{folder}\\{id}.video"))
                {
                    infos = sr.ReadToEnd().Replace("{", "")
                                          .Replace("}", "")
                                          .Replace("\"", "")
                                          .Split("\n");
                }
                string videoId = "";
                string title = "";
                string thumbnail = "";
                string address = "";
                string addressType = "";
                string[] tags = { };

                foreach (string info in infos)
                {
                    var sep = info.Split(':');
                    if (sep.Length != 2)
                        return null;
                    switch (sep[0])
                    {
                        case "VideoId":
                            videoId = sep[1];
                            break;
                        case "Title":
                            title = sep[1];
                            break;
                        case "Thumbnail":
                            thumbnail = sep[1];
                            break;
                        case "Address":
                            address = sep[1];
                            break;
                        case "AddressType":
                            addressType = sep[1];
                            break;
                        case "Tags":
                            tags = sep[1].Split(',');
                            break;
                        default:
                            return null;
                    }
                }
                return new Video(videoId, title, thumbnail, address, addressType, tags);
            }
            catch { return null; }
        }
    }
}
