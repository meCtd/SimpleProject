using System.ComponentModel;
using System.Windows;
using Tamagotchi.ViewModel;

namespace Tamagotchi.View
{
	/// <summary>
	/// Логика взаимодействия для GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{
		private GameWindowViewModel _gameVm;
		public GameWindow()
		{
			InitializeComponent();
		}

		public GameWindow(GameWindowViewModel data) : this()
		{
			_gameVm = data;
			DataContext = _gameVm;
		}

		private void GameWindow_OnClosing(object sender, CancelEventArgs e)
		{
			_gameVm.Pet.SleepCommand.Execute(null);
			new MainMenuWindow().Show();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
