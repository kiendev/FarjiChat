using JetImageLoader;

namespace ChatRooms.Converter
{
    public class SampleJetImageLoaderConverter : BaseJetImageLoaderConverter
    {
        protected override JetImageLoaderConfig GetJetImageLoaderConfig()
        {
            return SampleJetImageLoaderImpl.GetJetImageLoaderConfig();
        }

        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // you can do some manipulations with image url (value arguement)
            return base.Convert(value, targetType, parameter, culture);
        }
    }
}
