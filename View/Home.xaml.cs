using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using BeautifulGirl.Framework;
using BeautifulGirl.Utils;
using ChatRooms.Control;
using ChatRooms.Helper;
using ChatRooms.Model;
using ChatRooms.Resources;
using ChatRooms.Utils;
using ChatRooms.ViewModel;
using Coding4Fun.Toolkit.Controls;
using GoogleAds;
using ImageTools;
using ImageTools.Controls;
using ImageTools.IO.Gif;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MyTube.Control;
using SportTV.Helper;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace ChatRooms.View
{
    public partial class Home : PhoneApplicationPage
    {
        #region Private member

        private readonly ProgressIndicator _progressSending = new ProgressIndicator { IsVisible = false, IsIndeterminate = false, Text = "Sending..." };

        private double _dragDistanceToOpen = 75.0;
        private double _dragDistanceToClose = 305.0;
        private double _dragDistanceNegative = -75.0;
        private FrameworkElement _feContainer;
        private int _selectedMenu;
        private int _countBack;

        private HomeViewModel _viewModel;
        
        #endregion

        #region Contrutor

        public Home()
        {
            InitializeComponent();

            _feContainer = this.Container as FrameworkElement;

            _viewModel = (HomeViewModel) DataContext;

            SystemTray.SetProgressIndicator(this, _progressSending);

            MenuNavigation.SelectCompleted += MenuNavigation_SelectCompleted;

            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();
        }

        #region On Navigate

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (_viewModel.IsSticker)
            {
                e.Cancel = true;

                ImgShowSticker.Source = new BitmapImage(new Uri("/Assets/flatemoticons.png", UriKind.RelativeOrAbsolute));

                _viewModel.IsSticker = false;
            }
            else if (_viewModel.IsRoomChat)
            {
                e.Cancel = true;

                if (DataBrower.CanGoBack)
                    DataBrower.GoBack();

                if (_viewModel.IsShowUser) _viewModel.IsShowUser = false;
                else _viewModel.IsRoomChat = false;
            }
            else if (_viewModel.IsAbout && _viewModel.IsStickerItem)
            {
                e.Cancel = true;

                _viewModel.IsStickerItem = false;

                if (ListStickers.ItemsSource.Count > 0)
                {
                    ListStickers.ItemsSource.Clear();

                    //SearchVisualTree(ListStickers);
                }
            }
            else
            {
                if (_countBack == 0)
                {
                    e.Cancel = true;
                    ExitGrid.Visibility = Visibility.Visible;
                    _countBack = _countBack + 1;
                }
                else
                {
                    try
                    {
                        IsolatedStorageSettings.ApplicationSettings["CountAds"] = Variable.CountAds;
                        IsolatedStorageSettings.ApplicationSettings.Save();

                        e.Cancel = true;

                        App.InterstitialAd.DismissingOverlay += InterstitialAd_DismissingOverlay;
                        App.InterstitialAd.ShowAd();
                    }
                    catch (Exception)
                    {
                        Application.Current.Terminate();
                    }
                }
            }
        }

        private void InterstitialAd_DismissingOverlay(object sender, AdEventArgs e)
        {
            App.Current.Terminate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    NavigationService.RemoveBackEntry();

                    _selectedMenu = 1;
                    _viewModel.IsRoomChat = false;
                    _viewModel.IsShowUser = false;
                    _viewModel.IsNotInternet = false;
                    _viewModel.BindingColor = (SolidColorBrush)Application.Current.Resources["ForegroundHyperlink"];

                    if (IsolatedStorageSettings.ApplicationSettings.Contains("Logging"))
                    {
                        var logging = IsolatedStorageSettings.ApplicationSettings["Logging"].ToString();

                        if (logging.Equals("0"))
                        {
                            var loginPath = IsolatedStorageSettings.ApplicationSettings["LoginPath"].ToString();
                            DataBrower.Source = new Uri(loginPath, UriKind.RelativeOrAbsolute);

                            IsolatedStorageSettings.ApplicationSettings["Logging"] = "1";
                            IsolatedStorageSettings.ApplicationSettings.Save();

                            Debug.WriteLine("First login");
                        }
                        else
                        {
                            var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                            DataBrower.Source = new Uri(String.Format(Const.HomePath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute);

                            Debug.WriteLine("Resume login");
                        }
                    }
                    else
                    {
                        _viewModel.IsRoomChat = false;
                        _viewModel.IsShowUser = false;
                        
                        NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.RelativeOrAbsolute));
                    }
                }

                AdsService.OnRequestInterstitialClick();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            base.OnNavigatedTo(e); 
        }

        #endregion

        private void MenuNavigation_SelectCompleted(int result)
        {
            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("NickName"))
                {
                    var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();

                    _selectedMenu = result;

                    CloseSettings();

                    _viewModel.IsAbout = false;
                    _viewModel.IsRoomChat = false;
                    _viewModel.IsShowUser = false;
                    _viewModel.IsPrivateRoom = false;
                    _viewModel.IsNotInternet = false;
                    _viewModel.IsSticker = false;
                    ImgShowSticker.Source = new BitmapImage(new Uri("/Assets/flatemoticons.png", UriKind.RelativeOrAbsolute));

                    switch (result)
                    {
                        case 6:
                            //Share task
                            ShareLinkTask slt = new ShareLinkTask();
                            slt.LinkUri = new Uri("http://farjichat.com", UriKind.RelativeOrAbsolute);
                            slt.Title = "Check out the perfect chat room app you'll fall in love with";
                            slt.Show();
                            break;
                        case 1:
                            //Home
                            TxtTitle.Text = "Home";
                            DataBrower.Navigate(new Uri(String.Format(Const.HomePath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute));
                            break;
                        case 2:
                            //User Online
                            TxtTitle.Text = "User Online";
                            DataBrower.Navigate(new Uri(String.Format(Const.OnlineUserPath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute));
                           
                            break;
                        case 3:
                            //Private Room
                            TxtTitle.Text = "Private Room";
                            ShowPrivateRoom();
                            break;
                        case 4:
                            //Room chat
                            TxtTitle.Text = "Room chat";

                            if (!string.IsNullOrEmpty(App.SelectedRoom))
                            {
                                DataBrower.Navigate(new Uri(String.Format(Const.ChatRoomsPath, nickname, string.Empty, string.Empty, App.SelectedRoom),
                                        UriKind.RelativeOrAbsolute));

                                _viewModel.IsRoomChat = true;
                            }
                            else
                            {
                                DataBrower.Navigate(new Uri(String.Format(Const.OnlineUserPath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute));

                                MenuNavigation.SetSelectedMenu(2);
                            }
                            break;
                        case 5:
                            //Sticker Path
                            _viewModel.IsAbout = true;
                            TxtTitle.Text = "Sticker";

                            _viewModel.GetGroupStikerCommand.Execute(null);
                            //DataBrower.Navigate(new Uri(Const.StickerPath, UriKind.RelativeOrAbsolute));
                            break;
                        case 7:
                            //About
                            TxtTitle.Text = "About";
                            DataBrower.Navigate(new Uri(Const.AboutPath, UriKind.RelativeOrAbsolute));
                            break;
                        case 8:
                            //Logout
                            _viewModel.IsLoading = true;
                            _viewModel.LogoutCommand.Execute(null);
                            break;
                        case 9:
                            //Blocked
                            TxtTitle.Text = "Blocked";
                            DataBrower.Navigate(new Uri(String.Format(Const.BlockedPath, nickname), UriKind.RelativeOrAbsolute));
                            break;

                        case 10:
                            //Blocked
                            TxtTitle.Text = "Meme Generator";
                            DataBrower.Navigate(new Uri(String.Format(Const.MemePath, nickname), UriKind.RelativeOrAbsolute));
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #region Show Private Room

        private void ShowPrivateRoom()
        {
            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("PrivateRoom") && !string.IsNullOrEmpty(IsolatedStorageSettings.ApplicationSettings["PrivateRoom"].ToString()))
                {
                    var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                    var room = IsolatedStorageSettings.ApplicationSettings["PrivateRoom"].ToString();
                    
                    DataBrower.Navigate(new Uri(String.Format(Const.PrivateRoomsPath, nickname, room),
                            UriKind.RelativeOrAbsolute));

                    _viewModel.IsRoomChat = true;
                    _viewModel.IsPrivateRoom = true;
                }
                else
                {
                    var control = new SignControl();
                    PopupManager.Instance.Show(control);

                    control.SignCompleted += control_SignCompleted;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void control_SignCompleted(SignControlResult result)
        {
            try
            {
                var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();

                if (result.Action == 1)
                {
                    IsolatedStorageSettings.ApplicationSettings["PrivateRoom"] = result.Content;
                    IsolatedStorageSettings.ApplicationSettings.Save();

                    _viewModel.IsRoomChat = true;

                    DataBrower.Navigate(new Uri(String.Format(Const.PrivateRoomsPath, nickname, result.Content),
                            UriKind.RelativeOrAbsolute));

                    _viewModel.IsPrivateRoom = true;
                }
                else
                {
                    DataBrower.Navigate(new Uri(String.Format(Const.OnlineUserPath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute));

                    MenuNavigation.SetSelectedMenu(2);
                    _selectedMenu = 2;
                    TxtTitle.Text = "User Online";
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        #region DataBrower Navigate

        private void DataBrower_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _viewModel.IsLoading = false;
            DataBrower.Opacity = 1;
        }

        private void InterstitialAd_DismissingOverlay2(object sender, AdEventArgs e)
        {
            AdsService.OnRequestInterstitialClick();
        }

        //Show loading when navigating
        private void DataBrower_Navigating(object sender, NavigatingEventArgs e)
        {
            try
            {
                var uri = e.Uri.AbsoluteUri;
                var path = e.Uri.AbsolutePath;

                if (uri.StartsWith("to"))
                {
                    if (_selectedMenu != 7)
                    {
                        var sendTo = e.Uri.Authority;

                        TxtMessage.Text = sendTo.ToUpper() + ", ";
                        TxtMessage.Focus();
                        TxtMessage.Select(TxtMessage.Text.Length, 0);

                        e.Cancel = true;
                        return;
                    }
                }

                if (uri.StartsWith("memecopy"))
                {
                    if (_selectedMenu != 7)
                    {
                        var code = "_" + e.Uri.OriginalString.Replace("memecopy://", "").Replace("/", "") + "_";

                        Clipboard.SetText(code);

                        var toast = new ToastPrompt { Message = code + " copied" };
                        toast.Show();

                        e.Cancel = true;
                        return;
                    }
                }

                if (uri.StartsWith("block"))
                {
                    if (_selectedMenu != 7)
                    {
                        var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                        var blocked = e.Uri.OriginalString.Replace("block://", "").Replace("/", "");
                        var title = blocked.ToUpper() + " Selected";
                        var msgContent = "Choose action for " + blocked.ToUpper();

                        var loading = new LoadingControl();

                        DialogMessage dialog = new DialogMessage(title, msgContent, 2, "Block", "Cancel", "Send PM");
                        dialog.Dismissed += async delegate(int args)
                        {
                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                if (args.Equals(0))
                                {
                                    PopupManager.Instance.Show(loading);

                                    //Build uri request
                                    var uriRequest = String.Format(Const.BlockPath, blocked, nickname);
                                    Debug.WriteLine(uriRequest);
                                    var result = await DownloadService.LoginServer(uriRequest);

                                    PopupManager.Instance.Hide();

                                    //Show toast result
                                    var toast = new ToastPrompt();
                                    toast.Message = result;
                                    toast.Show();
                                }
                                else if (args.Equals(1))
                                {
                                    title = "Private Message to " + blocked.ToUpper();
                                    var sendPmControl = new SendPmControl(title);
                                    sendPmControl.Show();
                                    sendPmControl.Dismissed += async delegate(int i, string msg)
                                    {
                                        if (i.Equals(1))
                                        {
                                            _progressSending.IsVisible = true;

                                            var postUrl = String.Format(Const.ChatPmPath, nickname, blocked, App.SelectedRoom);
                                            await DownloadService.SendPm(postUrl, msg.ToLower());

                                            _progressSending.IsVisible = false;
                                        }
                                    };
                                }
                            }
                            else
                            {
                                var toast = new ToastPrompt
                                {
                                    Title = AppResources.ApplicationTitle,
                                    Message = "Network Error...",
                                    TextWrapping = TextWrapping.Wrap
                                };
                                toast.Show();
                            }
                        };
                        dialog.Show();

                        e.Cancel = true;
                        return;
                    }
                }

                if (uri.StartsWith("unblock"))
                {
                    if (_selectedMenu != 7)
                    {
                        var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                        var blocked = e.Uri.Authority;
                        var title = "Unblock " + blocked.ToUpper();
                        var msgContent = "Do you really want to unblock " + blocked.ToUpper();

                        var dialog = new DialogMessage(title, msgContent, 1, "No", "Yes");
                        dialog.Dismissed += async delegate(int args)
                        {
                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                if (args.Equals(0))
                                {
                                    //Nothing
                                }
                                else if (args.Equals(1))
                                {
                                    var loading = new LoadingControl("Unblocking");
                                    PopupManager.Instance.Show(loading);

                                    //Build uri request
                                    var uriRequest = String.Format(Const.UnblockPath, blocked, nickname);
                                    var result = await DownloadService.LoginServer(uriRequest);

                                    PopupManager.Instance.Hide();

                                    //Show toast result
                                    var toast = new ToastPrompt();
                                    toast.Message = result;
                                    toast.Show();

                                    DataBrower.Navigate(new Uri(DataBrower.Source.AbsoluteUri, UriKind.RelativeOrAbsolute));
                                }
                            }
                            else
                            {
                                var toast = new ToastPrompt
                                {
                                    Title = AppResources.ApplicationTitle,
                                    Message = "Network Error...",
                                    TextWrapping = TextWrapping.Wrap
                                };
                                toast.Show();
                            }
                        };
                        dialog.Show();

                        e.Cancel = true;
                        return;
                    }
                }

                DataBrower.Opacity = 0;

                try
                {
                    if (Variable.CountAds % 10 == 0 && Variable.CountAds != 0)
                    {
                        App.InterstitialAd.DismissingOverlay += InterstitialAd_DismissingOverlay2;
                        App.InterstitialAd.ShowAd();
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("Not load ads");
                }

                Variable.CountAds = Variable.CountAds + 1;

                Debug.WriteLine("Room Chat:" + _viewModel.IsRoomChat);

                if (path.Contains("index.php"))
                {
                    _viewModel.IsRoomChat = false;
                    _viewModel.IsShowUser = false;

                    IsolatedStorageSettings.ApplicationSettings.Remove("Logging");
                    IsolatedStorageSettings.ApplicationSettings.Save();

                    NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.RelativeOrAbsolute));
                }

                if (e.Uri.OriginalString.Contains("chat.php"))
                {
                    var redirectUrl = e.Uri.OriginalString.Replace("chat.php", "chatajax.php");
                    Debug.WriteLine(redirectUrl);

                    DataBrower.Navigate(new Uri(redirectUrl, UriKind.RelativeOrAbsolute));
                }

                _viewModel.IsLoading = true;
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

        private void DialogOnDismissed(int args)
        {
            try
            {
                if (args.Equals(0))
                {
                    
                }
                else if (args.Equals(1))
                {
                    
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        //When navigated, check session end, go to room chat
        private void DataBrower_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                var title = (string)DataBrower.InvokeScript("eval", "document.URL.toString()");
                var content = DataBrower.SaveToString();

                //if (_selectedMenu.Equals(10))
                //{
                //    //Debug.WriteLine(content);

                //    if (content.Contains("memecopy://"))
                //    {
                //        Debug.WriteLine("Meme replace");

                //        //content = content.Replace("memecopy://", "meme://");

                //        //DataBrower.NavigateToString(content);
                //    }
                //}

                Debug.WriteLine(title);

                //Loaded room
                if (_selectedMenu == 2 || _selectedMenu == 1)
                {
                    if (title.Split('&').Length > 3)
                    {
                        var roomName = title.Split('&')[3];

                        if (!string.IsNullOrEmpty(roomName))
                        {
                            if (roomName.StartsWith("room="))
                            {
                                _viewModel.IsRoomChat = true;

                                roomName = roomName.Replace("room=", "");
                                App.SelectedRoom = roomName;
                            }
                        }

                        Debug.WriteLine(title);
                    }
                }

                //Loaded sticker
                if (_selectedMenu == 5){
                    if (title.Contains("groups="))
                    {
                        content = content.Replace("copy://.hwwitch.", "#");

                        DataBrower.NavigateToString(content); 
                    }
                }

                //var reEnter = (string)DataBrower.InvokeScript("eval", "document.getElementsByClassName('ui-link').innerHTML");

                //Check Session
                if (content.Contains("Session problem"))
                {
                    _viewModel.IsRoomChat = false;

                    var msgBox = new CustomMessageBox()
                    {
                        Caption = AppResources.ApplicationTitle,
                        Message = AppResources.SessionTimeout,
                        RightButtonContent = "OK",
                        IsFullScreen = false,
                        VerticalAlignment = VerticalAlignment.Top,
                        Background = new SolidColorBrush(Colors.Black)
                    };

                    msgBox.Dismissed += (s, args) =>
                    {
                        _viewModel.IsRoomChat = true;
                        _viewModel.IsShowUser = false;

                        IsolatedStorageSettings.ApplicationSettings.Remove("Logging");
                        IsolatedStorageSettings.ApplicationSettings.Save();

                        NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.RelativeOrAbsolute));
                    };

                    msgBox.Show();
                }

                _viewModel.IsLoading = false;
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

        #endregion

        #endregion

        #region Send Msg

        private void SendMsgClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_viewModel.IsSending)
                {
                    var content = TxtMessage.Text;

                    if (!string.IsNullOrEmpty(content))
                    {
                        //_viewModel.SendMsgCommand.Execute(content);

                        //DataBrower.Navigate(new Uri(DataBrower.Source.AbsoluteUri, UriKind.RelativeOrAbsolute));

                        //TxtMessage.Text = string.Empty;

                        SendMsg(content);
                    }
                    else TxtMessage.Focus();
                }
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
                    _progressSending.IsVisible = true;
                    _viewModel.IsSending = true;
                    _viewModel.BindingColor = (SolidColorBrush)Application.Current.Resources["DisableColorBrush"];

                    var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                    var room = App.SelectedRoom;
                    if (_viewModel.IsPrivateRoom)
                        room = IsolatedStorageSettings.ApplicationSettings["PrivateRoom"].ToString();

                    var getUrl = String.Format(Const.SendMsgPath, nickname, room);

                    Debug.WriteLine(getUrl);

                    if (!string.IsNullOrEmpty(nickname) && !string.IsNullOrEmpty(room))
                        await DownloadService.PostAsync(getUrl, messenge);
                    else Debug.WriteLine("nickname or room null");

                    _viewModel.BindingColor = (SolidColorBrush)Application.Current.Resources["ForegroundHyperlink"];
                    _viewModel.IsSending = false;
                    _viewModel.TextMsg = string.Empty;

                    _progressSending.IsVisible = false;

                    //DataBrower.InvokeScript("eval", "newLoad");
                    //DataBrower.InvokeScript("newLoad()");
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
                _progressSending.IsVisible = true;
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = networkExceptionEx.MessageEx, TextWrapping = TextWrapping.Wrap };
                toast.Show(); 
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ShowStickerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_viewModel.IsSticker)
                {
                    ImgShowSticker.Source =
                        new BitmapImage(new Uri("/Assets/flatemoticons_press.png", UriKind.RelativeOrAbsolute));

                    _viewModel.IsSticker = true;

                    if(_viewModel.ListGroup.Count <= 0)
                        _viewModel.GetGroupStiker2Command.Execute(null);
                }
                else
                {
                    ImgShowSticker.Source = new BitmapImage(new Uri("/Assets/flatemoticons.png", UriKind.RelativeOrAbsolute));
                    _viewModel.IsSticker = false;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void GroupStickerTap(object sender, GestureEventArgs e)
        {
            try
            {
                var stackPanel = sender as StackPanel;

                if (stackPanel != null)
                {
                    SearchVisualTree(LstGroupStickers);
                    stackPanel.Background = (SolidColorBrush)Application.Current.Resources["ForegroundHyperlink"];

                    var url = stackPanel.Tag.ToString();
                    _viewModel.GetStickerByGroup2Command.Execute(url);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void SearchVisualTree(DependencyObject targetElement)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is StackPanel)
                {
                    var targetItem = (StackPanel)child;

                    if(targetItem.Name.Equals("GroupStickerPanel"))
                        targetItem.Background = new SolidColorBrush(Colors.Transparent);
                    else
                    {
                        SearchVisualTree(targetItem);
                    }
                }
                else
                {
                    SearchVisualTree(child);
                }
            }
        }

        private void StickerItemTap(object sender, GestureEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    var code = image.Tag.ToString();

                    _viewModel.TextMsg = _viewModel.TextMsg + " " + code;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        #region Setting Panel

        private bool _isSettingsOpen = false;

        private void ImgButtonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isSettingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }

        private void CloseSettings()
        {
            var trans = _feContainer.GetHorizontalOffset().Transform;
            trans.Animate(trans.X, 0, TranslateTransform.XProperty, 300, 0, new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            });

            _isSettingsOpen = false;
        }

        private void OpenSettings()
        {
            var trans = _feContainer.GetHorizontalOffset().Transform;
            trans.Animate(trans.X, 380, TranslateTransform.XProperty, 300, 0, new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            });

            _isSettingsOpen = true;
        }

        private void GestureListener_OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            try
            {
                if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
                {
                    double offset = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                    if (offset > _dragDistanceToOpen)
                        OpenSettings();
                    else
                        _feContainer.SetHorizontalOffset(offset);
                }

                if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
                {
                    double offsetContainer = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                    if (offsetContainer < _dragDistanceToClose)
                        CloseSettings();
                    else
                        _feContainer.SetHorizontalOffset(offsetContainer);
                }

                //if (e.Direction == System.Windows.Controls.Orientation.Vertical && e.VerticalChange > 0 && _viewModel.IsRoomChat && !_isSettingsOpen)
                //{
                //    double offSet = e.VerticalChange;

                //    double offsetContainer = _feContainer.GetVerticalOffset().Value + e.VerticalChange;

                //    Debug.WriteLine(offsetContainer);
                //}
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void GestureListener_OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            try
            {
                if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
                {
                    if (e.HorizontalChange < _dragDistanceToOpen)
                        this.ResetLayoutRoot();
                    else
                        this.OpenSettings();
                }

                if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
                {
                    if (e.HorizontalChange > _dragDistanceNegative)
                        ResetLayoutRoot();
                    else
                        CloseSettings();
                }

                //if (e.Direction == System.Windows.Controls.Orientation.Vertical && e.VerticalChange > 0 && _viewModel.IsRoomChat)
                //{
                //    double offSet = e.VerticalChange;

                //    Debug.WriteLine("Down " + offSet);

                //    if (offSet > 200)
                //    {
                //        DataBrower.Navigate(new Uri(DataBrower.Source.AbsoluteUri, UriKind.RelativeOrAbsolute));
                //    }
                //}

                //PullToRefreshBar.Value = 0;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
        private void GestureListener_OnDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            Debug.WriteLine("Start ");
        }

        private void SettingsStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ResetLayoutRoot();
        }

        private void ResetLayoutRoot()
        {
            if (!_isSettingsOpen)
                _feContainer.SetHorizontalOffset(0.0);
            else
                _feContainer.SetHorizontalOffset(380.0);
        } 

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Vertical && e.VerticalVelocity > 0 && _viewModel.IsRoomChat)
            {
                double offSet = e.VerticalVelocity;

                Debug.WriteLine("Down " + offSet);
            }
        }

        #endregion

        #region Button Click

        private void ExitTap(object sender, GestureEventArgs gestureEventArgs)
        {
            ExitGrid.Visibility = Visibility.Collapsed;
            _countBack = 0;
        }

        //Click mail us
        private void MailToClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask mailTask = new EmailComposeTask();
                mailTask.To = "farjilok@gmail.com";
                mailTask.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ImgButtonRefreshOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBrower.Navigate(new Uri(DataBrower.Source.AbsoluteUri, UriKind.RelativeOrAbsolute));
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ImgButtonUserOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.IsShowUser = true;

                var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                var roomName = App.SelectedRoom;

                if(_viewModel.IsPrivateRoom)
                    roomName = IsolatedStorageSettings.ApplicationSettings["PrivateRoom"].ToString();

                DataBrower.Navigate(new Uri(String.Format(Const.UserInRoomPath, nickname, roomName), UriKind.RelativeOrAbsolute));
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ImgQuitRoomOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("PrivateRoom");
                IsolatedStorageSettings.ApplicationSettings.Save();

                _viewModel.IsLoading = true;
                _viewModel.IsPrivateRoom = false;

                var nickname = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                DataBrower.Navigate(new Uri(String.Format(Const.OnlineUserPath, nickname, string.Empty, string.Empty), UriKind.RelativeOrAbsolute));
                MenuNavigation.SetSelectedMenu(2);
                _selectedMenu = 2;
                TxtTitle.Text = "User Online";
                
                _viewModel.IsRoomChat = false;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        private void DataBrower_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            try
            {
                Debug.WriteLine("DataBrower_NavigationFailed");

                _viewModel.IsNotInternet = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #region Load Image Sticker

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            try
            {
                var grid = sender as Grid;

                if (grid != null)
                {
                    if (ListStickers.ItemsSource.Count > 0)
                    {
                        ListStickers.ItemsSource.Clear();
                    }

                    var code = grid.Tag.ToString();

                    if (!string.IsNullOrEmpty(code))
                    {
                        _viewModel.GetStickerByGroupCommand.Execute(code);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void Image_OnLoadingCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("Loading Gif Completed");

            try
            {
                var image = sender as AnimatedImage;

                if (image != null)
                {
                    var grid = image.Parent as Grid;

                    if (grid != null)
                    {
                        var defaultImg = grid.FindName("DefaultImage") as Image;

                        if (defaultImg != null)
                        {
                            defaultImg.Visibility = Visibility.Collapsed;
                            //image.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void Image_OnLoadingFailed(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("Load Gif Failed");

            try
            {
                var image = sender as AnimatedImage;

                if (image != null)
                {
                    var grid = image.Parent as Grid;

                    if (grid != null)
                    {
                        var defaultImg = grid.FindName("DefaultImage") as Image;

                        if (defaultImg != null)
                        {
                            image.Visibility = Visibility.Collapsed;

                            defaultImg.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void StickerTap(object sender, GestureEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    var code = image.Tag.ToString();

                    Clipboard.SetText(code);

                    var toast = new ToastPrompt { Message = code + " copied" };
                    toast.Show();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void StickerGifTap(object sender, GestureEventArgs e)
        {
            try
            {
                var image = sender as AnimatedImage;

                if (image != null)
                {
                    var code = image.Tag.ToString();

                    Clipboard.SetText(code);

                    var toast = new ToastPrompt { Message = code + " copied" };
                    toast.Show();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ImgUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    image.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void FlagTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml?page=flag", UriKind.RelativeOrAbsolute));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask mailTask = new EmailComposeTask();
                mailTask.To = "farjilok@gmail.com";
                mailTask.Show(); 
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #region Lost focus image

        private void Image_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Lost Focus AnimatedImage");

                var image = sender as AnimatedImage;

                if (image != null)
                {
                    image.Source = null;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Lost Focus Image");

                var image = sender as Image;

                if (image != null)
                {
                    BitmapImage bitmapImage = image.Source as BitmapImage;
                    bitmapImage.UriSource = null;

                    image.Source = null;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        private void ListStickers_OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            try
            {
                const int offset = 1;
                var listDoc = e.Container.Content as StickerItem;
                var index = _viewModel.ListSticker.IndexOf(listDoc);

                if (!_viewModel.IsLoading && _viewModel.ListSticker.Count - index <= offset && _viewModel.StartRecord >= 1)
                {
                    var maxSize = _viewModel.StartRecord + 31;

                    if (_viewModel.ListItem.Count > maxSize)
                    {
                        for (int i = _viewModel.StartRecord; i < maxSize; i++)
                        {
                            _viewModel.ListSticker.Add(_viewModel.ListItem[i]);
                        }

                        _viewModel.StartRecord = _viewModel.StartRecord + 31;

                        Debug.WriteLine("1: " + _viewModel.StartRecord);
                    }
                    else if (_viewModel.ListItem.Count < maxSize && _viewModel.ListItem.Count > _viewModel.StartRecord)
                    {
                        for (int i = _viewModel.StartRecord; i < _viewModel.ListItem.Count; i++)
                        {
                            _viewModel.ListSticker.Add(_viewModel.ListItem[i]);

                            _viewModel.StartRecord = _viewModel.ListItem.Count;
                        }

                        Debug.WriteLine("2: " + _viewModel.StartRecord);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void JpgLoadFail(object sender, ExceptionRoutedEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    image.Source = new BitmapImage(new Uri("/Assets/ic_menu_emoticons.png", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void StaticImage_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    var grid = image.Parent as Grid;

                    if (grid != null)
                    {
                        var defaultImg = grid.FindName("DefaultImage") as Image;

                        if (defaultImg != null)
                        {
                            defaultImg.Visibility = Visibility.Collapsed;
                            //image.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void AnimeImage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("AnimeImage_OnUnloaded");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion
    }
}