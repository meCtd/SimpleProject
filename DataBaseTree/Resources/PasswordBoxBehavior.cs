using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DataBaseTree.Resources
{
	public class PasswordBoxBehavior : Behavior<PasswordBox>
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxBehavior), new PropertyMetadata(null));

		public string Password
		{
			get { return (string)GetValue(PasswordProperty); }
			set { SetValue(PasswordProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.PasswordChanged += PasswordValueChanged;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PasswordChanged -= PasswordValueChanged;
		}

		private void PasswordValueChanged(object sender, RoutedEventArgs e)
		{
			Password = AssociatedObject.Password;
		}

	}
}

