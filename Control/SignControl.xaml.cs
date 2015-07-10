using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using SportTV.Helper;

namespace ChatRooms.Control
{
    public delegate void SignCompletedHandler(SignControlResult result);

    public class SignControlResult
    {
        public string Content { get; set; }
        //Action: 0 - Cancel, 1 - Ok, 
        public int Action { get; set; }
    }

    public partial class SignControl
    {
        public event SignCompletedHandler SignCompleted;

        public SignControl()
        {
            InitializeComponent();

            Loaded += SignControl_Loaded;

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;
        }

        private void SignControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxtRoomName.Focus();

            LayoutRoot.Margin = new Thickness(0,0,0,200);
        }

        private void ClosePopup()
        {
            var popclaim = Parent as Popup;

            if (popclaim != null) popclaim.IsOpen = false;
        }


        private void SignButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SignCompleted != null)
            {
                var roomName = TxtRoomName.Text;

                AnimationHide.Begin();
                AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                SignCompleted(new SignControlResult { Action = 1, Content = roomName });
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SignCompleted != null)
            {
                AnimationHide.Begin();
                AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                SignCompleted(new SignControlResult { Action = 0, Content = string.Empty });
            }
        }

        private void TxtComment_OnLostFocus(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Margin = new Thickness(0);
        }

        private void TxtComment_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Margin = new Thickness(0, 0, 0, 200);
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            CloseButton_OnClick(null, null);
        }
    }
}
