using System;
using Windows.UI.Xaml.Data;

namespace UWPMaterialControls.Converters
{
    public class VerticalOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return 0 - (double)value - 8;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
