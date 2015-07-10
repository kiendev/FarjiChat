using System;
using System.Reflection;
using System.Windows.Media;

namespace ChatRooms.Helper
{
    public class ColorsHelper
    {
        public static Color GetThisColor(string colorString)
        {
            Type colorType = (typeof(Colors));
            if (colorType.GetProperty(colorString) != null)
            {
                object o = colorType.InvokeMember(colorString,
                BindingFlags.GetProperty, null, null, null); if (o != null)
                {
                    return (Color)o;
                }
            }
            return Colors.Transparent;
        } 
    }
}
