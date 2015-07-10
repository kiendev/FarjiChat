using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRooms.Model;
using GalaSoft.MvvmLight;

namespace ChatRooms.ViewModel
{
    public class StickerViewModel : ViewModelBase
    {
        private ObservableCollection<GroupSticker> _listGroup;
        private ObservableCollection<StickerItem> _listSticker;
    }
}
