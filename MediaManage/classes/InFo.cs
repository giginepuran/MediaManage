using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MediaManage.Classes
{
    public class Info : INotifyPropertyChanged
    {
        private string _connectionString = "";
        private string _thumbnailUrl = "";
        private string _title = "";
        private string _youtubeID = "";
        private string _tagString = "";
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                RaisePropertyChanged("ConnectionString");
            }
        }

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
            set
            {
                _thumbnailUrl = value;
                RaisePropertyChanged("ThumbnailUrl");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string YoutubeID
        {
            get { return _youtubeID; }
            set
            {
                _youtubeID = value;
                RaisePropertyChanged("YoutubeID");
                this.ThumbnailUrl = $"https://i.ytimg.com/vi/{_youtubeID}/hqdefault.jpg";
            }
        }

        public string TagString 
        {   get { return _tagString; }
            set
            {
                _tagString = value;
                RaisePropertyChanged("TagString");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
