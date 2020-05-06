using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace DataBaseTree.Convertors
{
	class EnumToItemsSource : MarkupExtension
	{
		private readonly Type _type;

		public EnumToItemsSource(Type type)
		{
			_type = type;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _type.GetMembers().SelectMany(s => s.GetCustomAttributes(typeof(DescriptionAttribute), false)).Cast<DescriptionAttribute>().Select(s => s.Description);
		}
	}
}
