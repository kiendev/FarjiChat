using System;
using System.Collections.ObjectModel;

namespace ChatRooms.Model
{
    public class DataItem
    {
        public DataItem(string title)
        {
            Title = title;
        }

        public string Title
        {
            get;
            private set;
        }
    }

    public class GroupSticker
    {
        public string grname { get; set; }
        public string grurl { get; set; }
        public string grimg { get; set; }
    }

    public class GroupStickerUri
    {
        public string grname { get; set; }
        public string grurl { get; set; }
        public Uri grimg { get; set; }
    }

    public class StickerItem
    {
        public string code { get; set; }
        public string url { get; set; }
    }
}
