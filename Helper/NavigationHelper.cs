using System;
using System.Windows;
using Microsoft.Phone.Controls;
using SportTV.Helper;

namespace BeautifulGirl.Helper
{
    /// <summary>
    /// Navigation helper, to manage navigation from anywhere
    /// </summary>
    public class NavigationHelper
    {
        private PhoneApplicationFrame _frame;

        public NavigationHelper(PhoneApplicationFrame frame)
        {
            _frame = frame;
            _frame.BackKeyPress += _frame_BackKeyPress;
            _frame.Navigated += _frame_Navigated;
        }

        void _frame_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PopupManager.Instance.IsOpen)
            {
                PopupManager.Instance.Hide();
                e.Cancel = true;
            }
            
        }

        void _frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            PopupManager.Instance.Hide();
            
        }

        /// <summary>
        /// Initialize the navigation helper with a navigation frame
        /// </summary>
        /// <param name="frame"></param>
        public void Initialize(PhoneApplicationFrame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Go back in navigation history
        /// </summary>
        public void GoBack()
        {
            if (_frame.CanGoBack)
                _frame.GoBack();
        }

        /// <summary>
        /// Navigate to an url
        /// </summary>
        /// <param name="url"></param>
        public void Navigate<T>()
        {
            var type = typeof(T);

            var testvalue = (type.FullName.Substring(type.FullName.IndexOf('.')).Replace('.', '/')) + ".xaml";

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _frame.Navigate(new Uri(testvalue, UriKind.Relative));
            });
        }

        /// <summary>
        /// Removes an entry from the navigation back stack
        /// </summary>
        public void RemoveBackEntry()
        {
            _frame.RemoveBackEntry();
        }

        /// <summary>
        /// Exits the application 
        /// </summary>
        public void Exit()
        {

        }
    }
}
