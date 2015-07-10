using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ChatRooms.Helper;
using ChatRooms.Model;
using ChatRooms.Resources;
using ChatRooms.Utils;
using ChatRooms.ViewModel;
using Coding4Fun.Toolkit.Controls;
using ImageTools.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using SportTV.Helper;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChatRooms.Control
{
    public delegate void SelectAvatarCompletedHandler(string result);

    public partial class AvatarPicker : UserControl
    {
        public event SelectAvatarCompletedHandler SelectCompleted;

        private AvatarViewModel _viewModel;
        public bool IsHidden;

        public AvatarPicker()
        {
            InitializeComponent();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;

            _viewModel = (AvatarViewModel)DataContext;

            GetGroupStiker();
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            IsHidden = false;

            AnimationHide.Begin();
            AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();
        }

        private void StickerTap(object sender, GestureEventArgs e)
        {
            try
            {
                var image = sender as Image;

                if (image != null)
                {
                    var code = image.Tag.ToString();

                    IsHidden = false;

                    AnimationHide.Begin();
                    AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                    SelectCompleted(code);
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

                    IsHidden = false;

                    AnimationHide.Begin();
                    AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                    SelectCompleted(code);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void GroupAvatarOnTap(object sender, GestureEventArgs e)
        {
            try
            {
                var grid = sender as Grid;

                if (grid != null)
                {
                    if (ListStickerSelector.ItemsSource.Count > 0)
                    {
                        ListStickerSelector.ItemsSource.Clear();
                    }

                    var code = grid.Tag.ToString();

                    if (!string.IsNullOrEmpty(code))
                    {
                        GetStickerByGroup(code);
                    }
                }
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
                    _viewModel.ListSticker = new ObservableCollection<StickerItem>();

                    ListStickerSelector.ItemsSource.Clear();

                    _viewModel.IsStickerItem = true;
                    _viewModel.IsLoading = true;

                    var ret = await DownloadService.ParseSticker(String.Format(Const.StickerItemPath, groupCode));

                    if (ret.Count > 0)
                    {
                        ret.Remove(ret[ret.Count - 1]);

                        _viewModel.ListItem = ret;

                        for (int i = 0; i < ret.Count; i++)
                        {
                            var dotChar = ret[i].url[ret[i].url.Length - 4].ToString();

                            if (dotChar != ".")
                            {
                                _viewModel.ListItem.Remove(_viewModel.ListItem[i]);
                            }
                        }

                        if (_viewModel.ListItem.Count > PageSize)
                        {
                            for (int i = 0; i < PageSize; i++)
                            {
                                _viewModel.ListSticker.Add(_viewModel.ListItem[i]);
                            }

                            _viewModel.StartRecord = _viewModel.StartRecord + PageSize;
                        }
                        else _viewModel.ListSticker = _viewModel.ListItem;
                    }

                    _viewModel.IsLoading = false;
                }
                else
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Lost internet connection...", TextWrapping = TextWrapping.Wrap};
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

        private int PageSize = 15;

        private async void GetGroupStiker()
        {
            try
            {
                IsHidden = true;

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    _viewModel.IsStickerItem = false;
                    _viewModel.ListGroup = new ObservableCollection<GroupSticker>();
                    _viewModel.ListItem = new ObservableCollection<StickerItem>();

                    if (_viewModel.ListGroup != null && _viewModel.ListGroup.Count > 0)
                    {
                        //nothing
                    }
                    else
                    {
                        _viewModel.IsLoading = true;

                        var ret = await DownloadService.ParseGroup(Const.GroupStickerPath);

                        if (ret.Count > 0)
                        {
                            ret.Remove(ret[ret.Count - 1]);
                            _viewModel.ListGroup = ret;
                        }

                        _viewModel.IsLoading = false;
                    }
                }
                else
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = "Network error...", TextWrapping = TextWrapping.Wrap };
                    toast.Show();
                    //IsNotInternet = true;
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.IsStickerItem = false;
        }

        private void Image_OnLoadingFailed(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("Failed");

            try
            {
                var image = sender as AnimatedImage;

                if (image != null)
                {
                    //image.Visibility = Visibility.Collapsed;
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
            try
            {
                var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (phoneApplicationFrame != null)
                    phoneApplicationFrame.Navigate(new Uri("/View/About.xaml?page=flag", UriKind.RelativeOrAbsolute));
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void MailUsOnClick(object sender, RoutedEventArgs e)
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
                var image = sender as Image;

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
                    var maxSize = _viewModel.StartRecord + 15;

                    if (_viewModel.ListItem.Count > maxSize)
                    {
                        for (int i = _viewModel.StartRecord; i < maxSize; i++)
                        {
                            _viewModel.ListSticker.Add(_viewModel.ListItem[i]);
                        }

                        _viewModel.StartRecord = _viewModel.StartRecord + 15;

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
    }
}
