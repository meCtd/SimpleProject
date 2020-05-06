using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tamagotchi.Convertors
{
    class EnumConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object parameterValue = Enum.Parse(value.GetType(), (string)parameter);
            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(targetType,(string)parameter);
        }
    }
}
