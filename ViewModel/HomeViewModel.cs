using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BeautifulGirl.Helper;
using ChatRooms.Control;
using ChatRooms.Helper;
using ChatRooms.Model;
using ChatRooms.Resources;
using ChatRooms.Utils;
using ChatRooms.View;
using Coding4Fun.Toolkit.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Phone.Net.NetworkInformation;
using SportTV.Helper;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace ChatRooms.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        readonly NavigationHelper _navigation = (NavigationHelper)SimpleIoc.Default.GetService(typeof(NavigationHelper));

        private bool _isLoading;                //Show loading
        private bool _isLoadingSticker;         //Show loading
        private bool _isSending;                //Show sending
        private bool _isSticker;                //Show sending
        private bool _isAbout;                  //Show Sticker
        private int _selectMenu;                //Value menu select
        private bool _isRoomChat;               //Is room chat
        private bool _isShowUser;               //Is show user
        private bool _isStickerItem;            //Is sticker item
        private bool _isPrivateRoom;            //Is Private Room
        private bool _isNotInternet;
        private int _startRecord;
        private string _textMsg;
        private double _opacityButton;
        private SolidColorBrush _bindingColor;

        private ObservableCollection<GroupSticker> _listGroup;
        private ObservableCollection<StickerItem> _listSticker;
        private ObservableCollection<StickerItem> _listItem;

        public const int PageSize = 31;

        public int StartRecord
        {
            get { return _startRecord; }
            set
            {
                _startRecord = value;
                RaisePropertyChanged("StartRecord");
            }
        }

        public bool IsLoadingSticker
        {
            get { return _isLoadingSticker; }

            set
            {
                _isLoadingSticker = value;
                RaisePropertyChanged("IsLoadingSticker");
            }
        }

        public bool IsSticker
        {
            get { return _isSticker; }

            set
            {
                _isSticker = value;
                RaisePropertyChanged("IsSticker");
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

        public bool IsAbout
        {
            get { return _isAbout; }

            set
            {
                _isAbout = value;
                RaisePropertyChanged("IsAbout");
            }
        }

        public int SelectMenu
        {
            get { return _selectMenu; }

            set
            {
                _selectMenu = value;
                RaisePropertyChanged("SelectMenu");
            }
        }

        public bool IsRoomChat
        {
            get { return _isRoomChat; }

            set
            {
                _isRoomChat = value;
                RaisePropertyChanged("IsRoomChat");
            }
        }

        public bool IsShowUser
        {
            get { return _isShowUser; }

            set
            {
                _isShowUser = value;
                RaisePropertyChanged("IsShowUser");
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

        public bool IsPrivateRoom
        {
            get { return _isPrivateRoom; }

            set
            {
                _isPrivateRoom = value;
                RaisePropertyChanged("IsPrivateRoom");
            }
        }

        public bool IsNotInternet
        {
            get { return _isNotInternet; }

            set
            {
                _isNotInternet = value;
                RaisePropertyChanged("IsNotInternet");
            }
        }

        public bool IsSending
        {
            get { return _isSending; }

            set
            {
                _isSending = value;
                RaisePropertyChanged("IsSending");
            }
        }

        public string TextMsg
        {
            get { return _textMsg; }

            set
            {
                _textMsg = value;
                RaisePropertyChanged("TextMsg");
            }
        }

        public double OpacityButton
        {
            get { return _opacityButton; }

            set
            {
                _opacityButton = value;
                RaisePropertyChanged("OpacityButton");
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

        public ObservableCollection<StickerItem> ListSticker
        {
            get { return _listSticker; }

            set
            {
                _listSticker = value;
                RaisePropertyChanged("ListSticker");
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

        public SolidColorBrush BindingColor
        {
            get { return _bindingColor; }

            set
            {
                _bindingColor = value;
                RaisePropertyChanged("BindingColor");
            }
        }

        public RelayCommand<string> SendMsgCommand { get; set; }
        public RelayCommand PrivateRoomCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand GetGroupStikerCommand { get; set; }
        public RelayCommand<string> GetStickerByGroupCommand { get; set; }
        public RelayCommand ContinuousLoadingCommand { get; set; }
        public RelayCommand GetGroupStiker2Command { get; set; }
        public RelayCommand<string> GetStickerByGroup2Command { get; set; }

        public HomeViewModel()
        {
            IsLoading = true;
            IsAbout = false;
            IsRoomChat = false;
            IsShowUser = false;
            IsStickerItem = false;
            IsPrivateRoom = false;
            IsNotInternet = false;
            IsSending = false;
            IsSticker = false;
            IsLoadingSticker = false;
            SelectMenu = 1;
            OpacityButton = 1;

            SendMsgCommand = new RelayCommand<string>(SendMsg);
            LogoutCommand = new RelayCommand(Logout);
            GetGroupStikerCommand = new RelayCommand(GetGroupStiker);
            GetStickerByGroupCommand = new RelayCommand<string>(GetStickerByGroup);
            ContinuousLoadingCommand = new RelayCommand(ContinuousLoading);
            GetGroupStiker2Command = new RelayCommand(GetGroupStiker2);
            GetStickerByGroup2Command = new RelayCommand<string>(GetStickerByGroup2);

            ListGroup = new ObservableCollection<GroupSticker>();
            ListSticker = new ObservableCollection<StickerItem>();

            TextMsg = string.Empty;
            BindingColor = (SolidColorBrush)Application.Current.Resources["ForegroundHyperlink"];
        }

        private void ContinuousLoading()
        {
            try
            {

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void GetStickerByGroup2(string groupCode)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    ListSticker = new ObservableCollection<StickerItem>();
                    ListItem = new ObservableCollection<StickerItem>();

                    //IsStickerItem = true;
                    IsLoadingSticker = true;
                    StartRecord = 0;

                    var ret = await DownloadService.ParseSticker(String.Format(Const.StickerItemPath, groupCode));

                    if (ret.Count > 0)
                    {
                        ret.Remove(ret[ret.Count - 1]);

                        ListItem = ret;

                        for (int i = 0; i < ret.Count; i++)
                        {
                            var dotChar = ret[i].url[ret[i].url.Length - 4].ToString();

                            if (dotChar != ".")
                            {
                                ListItem.Remove(ListItem[i]);
                            }
                        }

                        if (ListItem.Count > PageSize)
                        {
                            for (int i = 0; i < PageSize; i++)
                            {
                                ListSticker.Add(ListItem[i]);
                            }

                            StartRecord = StartRecord + PageSize;
                        }
                        else ListSticker = ListItem;

                        //ListSticker = ListItem;
                    }

                    IsLoadingSticker = false;
                }
                else
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Lost internet connection...", TextWrapping = TextWrapping.Wrap };
                    toast.Show();
                }
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void GetGroupStiker2()
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    IsStickerItem = false;

                    ListGroup = new ObservableCollection<GroupSticker>();
                    ListItem = new ObservableCollection<StickerItem>();

                    IsLoadingSticker = true;

                    var ret = await DownloadService.ParseGroup(Const.GroupStickerPath);

                    if (ret.Count > 0)
                    {
                        ret.Remove(ret[ret.Count - 1]);
                        ListGroup = ret;
                    }

                    var url = ListGroup[0].grurl;
                    GetStickerByGroup2(url);

                    IsLoadingSticker = false;
                }
                else
                {
                    IsNotInternet = true;
                }
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void GetStickerByGroup(string groupCode)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    ListSticker = new ObservableCollection<StickerItem>();
                    ListItem = new ObservableCollection<StickerItem>();

                    IsStickerItem = true;
                    IsLoading = true;
                    StartRecord = 0;

                    var ret = await DownloadService.ParseSticker(String.Format(Const.StickerItemPath, groupCode));

                    if (ret.Count > 0)
                    {
                        ret.Remove(ret[ret.Count - 1]);

                        ListItem = ret;

                        for(int i = 0; i < ret.Count; i++)
                        {
                            var dotChar = ret[i].url[ret[i].url.Length - 4].ToString();

                            if (dotChar != ".")
                            {
                                ListItem.Remove(ListItem[i]);
                            }
                        }

                        if (ListItem.Count > PageSize)
                        {
                            for (int i = 0; i < PageSize; i++)
                            {
                                ListSticker.Add(ListItem[i]);
                            }

                            StartRecord = StartRecord + PageSize;
                        }
                        else ListSticker = ListItem;

                        //ListSticker = ListItem;
                    }
                    
                    IsLoading = false;
                }
                else
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Lost internet connection...", TextWrapping = TextWrapping.Wrap };
                    toast.Show();
                }
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void GetGroupStiker()
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    IsStickerItem = false;

                    ListGroup = new ObservableCollection<GroupSticker>();
                    ListItem = new ObservableCollection<StickerItem>();

                    IsLoading = true;

                    var ret = await DownloadService.ParseGroup(Const.GroupStickerPath);

                    if (ret.Count > 0)
                    {
                        ret.Remove(ret[ret.Count - 1]);
                        ListGroup = ret;
                    }

                    IsLoading = false;

                    //if (ListGroup != null && ListGroup.Count > 0)
                    //{
                    //    //nothing
                    //}
                    //else
                    //{
                    //    ListGroup = new ObservableCollection<GroupSticker>();

                    //    IsLoading = true;

                    //    var ret = await DownloadService.ParseGroup(Const.GroupStickerPath);

                    //    if (ret.Count > 0)
                    //    {
                    //        ret.Remove(ret[ret.Count - 1]);
                    //        ListGroup = ret;
                    //    }

                    //    IsLoading = false;
                    //}
                }
                else
                {
                    //var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Lost internet connection..." };
                    //toast.Show();
                    IsNotInternet = true;
                }
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void SendMsg(string messenge)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    IsSending = true;
                    BindingColor = (SolidColorBrush)Application.Current.Resources["DisableColorBrush"];

                    var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                    var room = App.SelectedRoom;
                    if(IsPrivateRoom)
                        room = IsolatedStorageSettings.ApplicationSettings["PrivateRoom"].ToString();

                    var getUrl = String.Format(Const.SendMsgPath, nickname, room);

                    Debug.WriteLine(getUrl);

                    if (!string.IsNullOrEmpty(nickname) && !string.IsNullOrEmpty(room))
                        await DownloadService.PostAsync(getUrl, messenge);
                    else Debug.WriteLine("nickname or room null");

                    BindingColor = (SolidColorBrush)Application.Current.Resources["ForegroundHyperlink"];
                    IsSending = false;
                    TextMsg = string.Empty;
                }
                else
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Network Error...", TextWrapping = TextWrapping.Wrap };
                    toast.Show();
                }

                //Debug.WriteLine(reponse);
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private async void Logout()
        {
            try
            {
                IsLoading = true;

                var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                var base64 = Base64Encode(nickname);
                var getUrl = String.Format(Const.LogoutPath, base64);

                Debug.WriteLine(getUrl);

                await DownloadService.LoginServer(getUrl);

                IsolatedStorageSettings.ApplicationSettings.Remove("Logging");
                IsolatedStorageSettings.ApplicationSettings.Save();

                _navigation.Navigate<Login>();
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}