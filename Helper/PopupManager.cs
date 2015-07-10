using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SportTV.Helper
{
    public class PopupManager
    {
        private static PopupManager _instance;

        public static PopupManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PopupManager();
                }
                return _instance;
            }
        }

        public bool IsOpen
        {
            get { return _popup.IsOpen; }
        }

        private Popup _popup = new Popup();

        public void Show(UserControl control)
        {
            if (!_popup.IsOpen)
            {
                _popup.Child = control;
                _popup.IsOpen = true;
            }
        }

        public void Hide()
        {
            if (_popup.IsOpen)
            {
                _popup.Child = null;
                _popup.IsOpen = false;
            }
                
        }


    }
}
