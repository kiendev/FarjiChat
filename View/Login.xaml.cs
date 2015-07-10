using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;
using ChatRooms.Control;
using ChatRooms.Resources;
using ChatRooms.ViewModel;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using SportTV.Helper;

namespace ChatRooms.View
{
    public partial class Login : PhoneApplicationPage
    {
        private MainViewModel _viewModel;
        private string _gender;
        private AvatarPicker _avatarControl;

        // Constructor
        public Login()
        {
            InitializeComponent();

            _viewModel = (MainViewModel)DataContext;

            _gender = "";
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.SelectedRoom = string.Empty;

            if (IsolatedStorageSettings.ApplicationSettings.Contains("NickName"))
            {
                TxtNickname.Text = IsolatedStorageSettings.ApplicationSettings["NickName"].ToString();
                TxtAge.Text = IsolatedStorageSettings.ApplicationSettings["Age"].ToString();
                TxtAvatarCode.Text = IsolatedStorageSettings.ApplicationSettings["AvatarCode"].ToString();

                _gender = IsolatedStorageSettings.ApplicationSettings["Gender"].ToString();
                var nickColor = IsolatedStorageSettings.ApplicationSettings["NickColor"] is Color ? (Color)IsolatedStorageSettings.ApplicationSettings["NickColor"] : new Color();

                _viewModel.NickColor = new SolidColorBrush(nickColor);

                if (_gender.Equals("m"))
                {
                    BtnMale.Background = (SolidColorBrush)Application.Current.Resources["ButtonBackground"];
                    BtnFemale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                }
                else if (_gender.Equals("f"))
                {
                    BtnMale.Background = (SolidColorBrush) Application.Current.Resources["TransparentBackground"];
                    BtnFemale.Background = (SolidColorBrush) Application.Current.Resources["ButtonBackground"];
                }
                else
                {
                    BtnMale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                    BtnFemale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            var isHidden = false;

            try
            {
                if (_avatarControl != null)
                isHidden = _avatarControl.IsHidden;
            }
            catch (Exception)
            {
                Application.Current.Terminate();
            }

            if (isHidden)
            {
                e.Cancel = true;

                PopupManager.Instance.Hide();
            }
            else 
                Application.Current.Terminate();
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckLogin())
                {
                    var param = new Dictionary<string, string>();
                    param["nickName"] = TxtNickname.Text.Trim();
                    param["age"] = "";
                    param["avatarCode"] = TxtAvatarCode.Text.Replace("#", "").Trim();
                    param["gender"] = _gender;

                    if (cbAgree.IsChecked != null && (bool)cbAgree.IsChecked)
                    {
                        _viewModel.LoginCommand.Execute(param);
                    }
                    else
                    {
                        var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = AppResources.AcceptPolicy, TextWrapping = TextWrapping.Wrap };
                        toast.Show();
                    }
                }

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private bool CheckLogin()
        {
            try
            {
                var nickname = TxtNickname.Text.Trim();

                if (string.IsNullOrEmpty(nickname))
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = AppResources.NicknameNotEmpty, TextWrapping = TextWrapping.Wrap };
                    toast.Show();

                    //MessageBox.Show(AppResources.NicknameNotEmpty);

                    return false;
                }

                if (nickname.Length < 3 || nickname.Length > 10 || nickname.Contains(" "))
                {
                    var toast = new ToastPrompt { Title = AppResources.ApplicationTitle, Message = AppResources.NicknameNotSpaceAndSpecial, TextWrapping = TextWrapping.Wrap };
                    toast.Show();

                    //MessageBox.Show(AppResources.NicknameNotSpaceAndSpecial);

                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return false;
            }
        }

        private void BtnMale_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_gender.Equals("m"))
                {
                    _gender = "";

                    BtnMale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                    BtnFemale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                }
                else
                {
                    _gender = "m";

                    BtnMale.Background = (SolidColorBrush)Application.Current.Resources["ButtonBackground"];
                    BtnFemale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void BtnFemale_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_gender.Equals("f"))
                {
                    _gender = "";

                    BtnMale.Background = (SolidColorBrush) Application.Current.Resources["TransparentBackground"];
                    BtnFemale.Background = (SolidColorBrush) Application.Current.Resources["TransparentBackground"];
                }
                else
                {
                    _gender = "f";

                    BtnMale.Background = (SolidColorBrush)Application.Current.Resources["TransparentBackground"];
                    BtnFemale.Background = (SolidColorBrush)Application.Current.Resources["ButtonBackground"];
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void BtnSelectColor_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.ShowColorPickerCommand.Execute(null);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void BtnSelectAvatarCode_OnClick(object sender, RoutedEventArgs e)
        {
            _avatarControl = new AvatarPicker();
            PopupManager.Instance.Show(_avatarControl);

            _avatarControl.SelectCompleted += delegate(string result) { TxtAvatarCode.Text = result; };
        }

        private void PolicyClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml?page=about", UriKind.RelativeOrAbsolute));
        }
    }
}