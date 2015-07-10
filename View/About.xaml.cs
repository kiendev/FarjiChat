using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using ChatRooms.Helper;
using ChatRooms.Resources;
using ChatRooms.Utils;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using SportTV.Helper;

namespace ChatRooms.View
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoadingIndicator.Visibility = Visibility.Visible;

            if (NavigationContext.QueryString.ContainsKey("page"))
            {
                var page = NavigationContext.QueryString["page"];

                if (page.Contains("about"))
                {
                    AboutBrowser.Source = new Uri(Const.AboutPath, UriKind.RelativeOrAbsolute);
                }
                else AboutBrowser.Source = new Uri(Const.FlagPath, UriKind.RelativeOrAbsolute);
            }
        }

        private void DataBrower_LoadCompleted(object sender, NavigationEventArgs e)
        {
            LoadingIndicator.Visibility = Visibility.Collapsed;
            AboutBrowser.Opacity = 1;
        }

        private void DataBrower_Navigating(object sender, NavigatingEventArgs e)
        {
            try
            {
                var path = e.Uri.AbsolutePath;

                if (path.Equals("/iapp/avatar.php"))
                {
                    NavigationService.GoBack();
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
    }
}