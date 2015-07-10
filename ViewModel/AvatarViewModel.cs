using System.Collections.ObjectModel;
using ChatRooms.Model;
using GalaSoft.MvvmLight;

namespace ChatRooms.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AvatarViewModel : ViewModelBase
    {
        private bool _isLoading;                //Show loading
        private bool _isStickerItem;            //Is sticker item
        private int _startRecord;
        private ObservableCollection<GroupSticker> _listGroup;
        private ObservableCollection<StickerItem> _listSticker;
        private ObservableCollection<StickerItem> _listItem;

        public const int PageSize = 15;

        public int StartRecord
        {
            get { return _startRecord; }
            set
            {
                _startRecord = value;
                RaisePropertyChanged("StartRecord");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }

            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public bool IsStickerItem
        {
            get { return _isStickerItem; }

            set
            {
                _isStickerItem = value;
                RaisePropertyChanged("IsStickerItem");
            }
        }

        public ObservableCollection<GroupSticker> ListGroup
        {
            get { return _listGroup; }

            set
            {
                _listGroup = value;
                RaisePropertyChanged("ListGroup");
            }
        }

        public ObservableCollection<StickerItem> ListItem
        {
            get { return _listItem; }

            set
            {
                _listItem = value;
                RaisePropertyChanged("ListItem");
            }
        }

        public ObservableCollection<StickerItem> ListSticker
        {
            get { return _listSticker; }

            set
            {
                _listSticker = value;
                RaisePropertyChanged("ListSticker");
            }
        }
        /// <summary>
        /// Initializes a new instance of the AvatarViewModel class.
        /// </summary>
        public AvatarViewModel()
        {
            IsLoading = false;
            IsStickerItem = false;

            ListGroup = new ObservableCollection<GroupSticker>();
            ListSticker = new ObservableCollection<StickerItem>();
        }
    }
}