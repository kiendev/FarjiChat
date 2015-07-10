using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ChatRooms.Control
{
    public delegate void DismissedHandler(int args);
    
    public partial class DialogMessage
    {
        public event DismissedHandler Dismissed; 
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
        public DialogMessage(string title, string content, int type, string buttonText1, string buttonText2 = "", string buttonText3 = "")
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
            
            ContentTextBlock.Text = content;

            if (type == 0)
            {
                //Chi hien thi button o giua
                LeftButton.Visibility = Visibility.Collapsed;
                RightButton.Visibility = Visibility.Collapsed;
                CenterButton.Visibility = Visibility.Visible;
                CenterButton.Content = buttonText1;
            }
            else if (type == 1)
            {
                CenterColumn.Width = new GridLength(0, GridUnitType.Star);

                //Hien thi button left va right
                LeftButton.Visibility = Visibility.Visible;
                RightButton.Visibility = Visibility.Visible;
                CenterButton.Visibility = Visibility.Collapsed;
                LeftButton.Content = buttonText1;
                RightButton.Content = buttonText2;
            }
            else
            {
                LeftButton.Visibility = Visibility.Visible;
                RightButton.Visibility = Visibility.Visible;
                CenterButton.Visibility = Visibility.Visible;
                LeftButton.Content = buttonText1;
                CenterButton.Content = buttonText2;
                RightButton.Content = buttonText3;
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
                Dismissed(-1);
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
                Dismissed(0);//Left button
                ClosePopup();
            }
        }

        private void RightButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Dismissed != null)
            {
                Dismissed(1);//Right button
                ClosePopup();
            }
        }

        private void BackgroundTap(object sender, GestureEventArgs e)
        {
            ClosePopup();

            if (Dismissed != null)
            {
                Dismissed(-1);
            } 
        }
    }
}
