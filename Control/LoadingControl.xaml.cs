using System.Windows;
using System.Windows.Controls.Primitives;

namespace MyTube.Control
{
    public partial class LoadingControl
    {
        private Popup _popup = new Popup();

        public LoadingControl(string text = "Blocking")
        {
            InitializeComponent();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;

            LoadingIndicator.Content = text;
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

        //private void ClosePopup()
        //{
        //    var popclaim = this.Parent as Popup;

        //    if (popclaim != null) popclaim.IsOpen = false;
        //}
    }
}
