using System.Windows;
using Tamagotchi.ViewModel;

namespace Tamagotchi.View
{
	/// <summary>
	/// Логика взаимодействия для MainMenuWindow.xaml
	/// </summary>
	public partial class MainMenuWindow : Window
	{
		public MainMenuWindow()
		{
			InitializeComponent();
			DataContext = new MainMenuWindowViewModel();
		}
	}
}
