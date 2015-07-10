using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ChatRooms.ViewModel;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChatRooms.Control
{
    public delegate void SelectMenuCompletedHandler(int result);

    public partial class Slideview
    {
        public event SelectMenuCompletedHandler SelectCompleted;
        private HomeViewModel _viewModel;

        public Slideview()
        {
            InitializeComponent();

            _viewModel = (HomeViewModel) DataContext;
        }

        private void Slideview_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ListMenu_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = e.AddedItems[0] as Categories;

                if (item != null)
                {
                    var redirectUrl = "/View/ListAnime.xaml?isFav=0&url=" + item.TargetUrl + "&title=" + item.FullTitle;

                    var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                    if (phoneApplicationFrame != null)
                        phoneApplicationFrame.Navigate(new Uri(redirectUrl, UriKind.Relative));
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void TapHome(object sender, GestureEventArgs e)
        {
            var redirectUrl = "/View/Home.xaml";

            var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneApplicationFrame != null)
                phoneApplicationFrame.Navigate(new Uri(redirectUrl, UriKind.Relative));
        }

        private void TapFavourite(object sender, GestureEventArgs e)
        {
            var redirectUrl = "/View/ListAnime.xaml?isFav=1";

            var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneApplicationFrame != null)
                phoneApplicationFrame.Navigate(new Uri(redirectUrl, UriKind.Relative));
        }

        public void SetSelectedMenu(int menu)
        {
            _viewModel.SelectMenu = menu;
        }

        private void ShareTask(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 6;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void AboutTask(object sender, GestureEventArgs e)
        {
            try
            {
                //var redirectUrl = "/View/About.xaml";

                //var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                //if (phoneApplicationFrame != null)
                //    phoneApplicationFrame.Navigate(new Uri(redirectUrl, UriKind.Relative));

                _viewModel.SelectMenu = 7;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void LogoutTap(object sender, GestureEventArgs e)
        {
            try
            {
                //_viewModel.IsRoomChat = false;
                //_viewModel.IsShowUser = false;

                //_viewModel.LogoutCommand.Execute(null);

                _viewModel.SelectMenu = 8;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }

                //var redirectUrl = "/View/Login.xaml";

                //var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                //if (phoneApplicationFrame != null)
                //    phoneApplicationFrame.Navigate(new Uri(redirectUrl, UriKind.Relative));
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
                _viewModel.SelectMenu = 5;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void HomeTask(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 1;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void OnlineTask(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 2;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void PrivateRoomTask(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 3;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ChatRoomTask(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 4;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void BlockedTap(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 9;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void MemeTap(object sender, GestureEventArgs e)
        {
            try
            {
                _viewModel.SelectMenu = 10;

                if (SelectCompleted != null)
                {
                    SelectCompleted(_viewModel.SelectMenu);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }

    public class Categories
    {
        public int CategoryId { get; set; }
        public string FullTitle { get; set; }
        public string TargetUrl { get; set; }
    }
}
