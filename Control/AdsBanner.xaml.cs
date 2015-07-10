using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoogleAds;

namespace ChatRooms.Control
{
    public partial class AdsBanner : UserControl
    {
        public AdsBanner()
        {
            InitializeComponent();
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
        }

        private void AdView_OnFailedToReceiveAd(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("Load ads fail");
        }
    }
}
