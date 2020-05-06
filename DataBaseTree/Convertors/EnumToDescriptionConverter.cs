using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace DataBaseTree.Convertors
{
	class EnumToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			FieldInfo info = value.GetType().GetField(value.ToString());
			return (info.GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault() as DescriptionAttribute)
				?.Description;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return targetType.GetFields()
				.First(s => (s.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute)?.Description == value.ToString()).GetValue(targetType);
		}
	}
}




