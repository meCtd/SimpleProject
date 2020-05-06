using System;
using System.Globalization;
using System.Windows.Data;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Convertors
{
	public class DbEntityEnumConverter : IValueConverter
	{
		private DbEntityEnum targetValue;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			targetValue = (DbEntityEnum)value;
			return targetValue.HasFlag((DbEntityEnum)parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return targetValue ^= (DbEntityEnum)parameter;
		}

	}
}