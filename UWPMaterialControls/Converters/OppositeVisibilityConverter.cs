using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPMaterialControls.Converters
{
    public class OppositeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var input = (Visibility)value;
            if (input == Visibility.Collapsed)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
