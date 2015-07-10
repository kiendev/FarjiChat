using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SportTV.Helper;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChatRooms.Control
{
    public delegate void SelectColorCompletedHandler(ColorResult result);

    public class ColorResult
    {
        public Color Color { get; set; }
        //Action: 0 - Cancel, 1 - Ok, 
        public int Action { get; set; }
    }

    public partial class ColorPickerControl : UserControl
    {
        public event SelectColorCompletedHandler SelectCompleted;
        private Color _selectedColor;

        public ColorPickerControl()
        {
            InitializeComponent();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;
        }

        private void SignButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectCompleted != null)
            {
                AnimationHide.Begin();
                AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                SelectCompleted(new ColorResult { Action = 1, Color = _selectedColor });
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectCompleted != null)
            {
                AnimationHide.Begin();
                AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();

                SelectCompleted(new ColorResult { Action = 0, Color = new Color() });
            }
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            AnimationHide.Begin();
            AnimationHide.Completed += (o, args) => PopupManager.Instance.Hide();
        }

        private void Picker_ColorChanged(object sender, Color color)
        {
            try
            {
                Debug.WriteLine(color.ToString());

                _selectedColor = color;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
