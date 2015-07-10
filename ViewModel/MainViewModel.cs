using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using BeautifulGirl.Helper;
using ChatRooms.Control;
using ChatRooms.Helper;
using ChatRooms.Utils;
using ChatRooms.View;
using Coding4Fun.Toolkit.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Phone.Info;
using SportTV.Helper;

namespace ChatRooms.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Public member

        readonly NavigationHelper _navigation = (NavigationHelper)SimpleIoc.Default.GetService(typeof(NavigationHelper));

        private SolidColorBrush _nickColor;

        public SolidColorBrush NickColor
        {
            get { return _nickColor; }

            set
            {
                _nickColor = value;
                RaisePropertyChanged("NickColor");
            }
        }

        public RelayCommand<Dictionary<string, string>> LoginCommand { get; set; }
        public RelayCommand ShowColorPickerCommand { get; set; }

        #endregion

        #region Contructor

        public MainViewModel()
        {
            LoginCommand = new RelayCommand<Dictionary<string, string>>(LoginSystem);
            ShowColorPickerCommand = new RelayCommand(ShowColorPicker);

            NickColor = (SolidColorBrush)Application.Current.Resources["ButtonBackground2"];
        }

        private void ShowColorPicker()
        {
            try
            {
                var control = new ColorPickerControl();
                PopupManager.Instance.Show(control);

                control.SelectCompleted += control_PickerControl;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void control_PickerControl(ColorResult result)
        {
            try
            {
                if (result.Action.Equals(1))
                {
                    NickColor = new SolidColorBrush(result.Color);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private async void LoginSystem(Dictionary<string, string> param)
        {
            try
            {
                var nickname = param["nickName"];
                var age = param["age"];
                var avatarCode = param["avatarCode"];
                var gender = param["gender"];
                var nickColor = NickColor.Color;
                var udid = GetUdid().Replace(" ","").Trim();

                if (string.IsNullOrEmpty(udid))
                {
                    var hash = DeviceStatus.DeviceName.Replace(" ", "") + DeviceStatus.DeviceFirmwareVersion.Replace(" ", "") + nickname.Replace(" ", "");
                    udid = Hash(hash);
                }

                var getUrl = String.Format(Const.LoginPath, nickname, gender, age, nickColor.ToString().Replace("#FF", ""), udid, avatarCode);

                Debug.WriteLine(udid);

                IsolatedStorageSettings.ApplicationSettings["LoginPath"] = getUrl;
                IsolatedStorageSettings.ApplicationSettings["NickName"] = nickname;
                IsolatedStorageSettings.ApplicationSettings["Age"] = age;
                IsolatedStorageSettings.ApplicationSettings["AvatarCode"] = avatarCode;
                IsolatedStorageSettings.ApplicationSettings["Gender"] = gender;
                IsolatedStorageSettings.ApplicationSettings["NickColor"] = nickColor;
                IsolatedStorageSettings.ApplicationSettings["Logging"] = "0";
                IsolatedStorageSettings.ApplicationSettings.Save();

                Debug.WriteLine(getUrl);

                _navigation.Navigate<Home>();

                //var reponse = await DownloadService.LoginServer(getUrl);

                //Debug.WriteLine(reponse);
            }
            catch (NetworkExceptionEx networkExceptionEx)
            {
                PopupManager.Instance.Hide();
                var toast = new ToastPrompt { Title = "", Message = networkExceptionEx.MessageEx };
                toast.Show();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length*2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private string GetUdid()
        {
            try
            {
                // get the unique device id for the publisher per device
                Byte[] deviceArrayId = (Byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");
                string uniqueDeviceId = Convert.ToBase64String(deviceArrayId);

                return uniqueDeviceId;
            }
            catch (Exception)
            {
                return Guid.NewGuid().ToString();
            }
        }

        #endregion
    }
}