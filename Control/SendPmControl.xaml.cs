using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChatRooms.Control
{
    public delegate void DismissedMessageHandler(int args, string contentMsg);

    public partial class SendPmControl : UserControl
    {
        public event DismissedMessageHandler Dismissed; 
        //public string Message { get; set; }
        private Popup _popup = new Popup();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type">type = 0: 1 button, type = 1: 2 button</param>
        /// <param name="buttonText1"></param>
        /// <param name="buttonText2"></param>
        /// <param name="buttonText3"></param>
        public SendPmControl(string title)
        {
            InitializeComponent();

            AnimationShow.Begin();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;

            if (title == "")
            {
                TitlePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                TitleTextBlock.Text = title;    
            }
        }

        private void ClosePopup()
        {
            AnimationClose.Completed += (s, x) =>
            {
                var popclaim = Parent as Popup;

                if (popclaim != null) popclaim.IsOpen = false;
            };

            AnimationClose.Begin();
        }

        private void CenterButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Thuc hien dong Popup
            ClosePopup();

            if (Dismissed != null)
            {
                Dismissed(-1, string.Empty);
            } 
        }

        public void Show()
        {
            _popup.Child = this;
            _popup.IsOpen = true;
        }

        public void Close()
        {
            _popup.IsOpen = false;
        }

        private void LeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Dismissed != null)
            {
                Dismissed(0, string.Empty);//Left button
                ClosePopup();
            }
        }

        private void RightButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Dismissed != null)
            {
                Dismissed(1, TxtMessage.Text);//Right button
                ClosePopup();
            }
        }

        private void BackgroundTap(object sender, GestureEventArgs e)
        {
            ClosePopup();

            if (Dismissed != null)
            {
                Dismissed(-1, string.Empty);
            } 
        }

        private void TxtMessage_OnGotFocus(object sender, RoutedEventArgs e)
        {
            FocusAnimation.Begin();
        }

        private void TxtMessage_OnLostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusAnimation.Begin();
        }
    }
}
