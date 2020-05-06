using System;
using System.Runtime.Serialization;
using System.Windows;
using Tamagotchi.Model;
using Tamagotchi.View;

namespace Tamagotchi.ViewModel
{
	class MainMenuWindowViewModel
	{
		private Pet _savedPet;
		private bool _isSaveSuccess => (_savedPet != null && _savedPet?.IsAlive == true);

		public MainMenuWindowViewModel()
		{
			try
			{
				_savedPet = Pet.Awake();
			}
			catch (SerializationException)
			{
				MessageBox.Show("Error", "Cant read the save file!", MessageBoxButton.OK, MessageBoxImage.Warning);
				_savedPet = null;
			}
			catch (InvalidOperationException)
			{
				MessageBox.Show("Error", "Yor save file was broken!", MessageBoxButton.OK, MessageBoxImage.Warning);
				_savedPet = null;
			}
			catch (Exception)
			{
				_savedPet = null;
			}
		}

		#region Commands

		#region NewGameCommand

		private RelayCommand<MainMenuWindow> _newGameCommand;

		public RelayCommand<MainMenuWindow> NewGameCommand
		{
			get
			{
				return _newGameCommand ?? (_newGameCommand = new RelayCommand<MainMenuWindow>(
						   NewGameClick));
			}
		}

		private void NewGameClick(MainMenuWindow o)
		{
			if (_isSaveSuccess)
			{
				MessageBoxResult result = MessageBox.Show($"Do you want to kill your your last pet named {_savedPet.Name} ?", "New game",
					MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{
					new ChosePetWindow().Show();
					o.Close();
				}
			}
			else
			{
				new ChosePetWindow().Show();
				o.Close();
			}
		}

		#endregion

		#region ContinueCommand

		private RelayCommand<MainMenuWindow> _continueCommand;

		public RelayCommand<MainMenuWindow> ContinueCommand
		{
			get
			{
				return _continueCommand ?? (_continueCommand = new RelayCommand<MainMenuWindow>(
						  ShowContinueWindow,
						  (o) => _isSaveSuccess));
			}
		}

		private void ShowContinueWindow(MainMenuWindow o)
		{
			new GameWindow(new GameWindowViewModel(_savedPet)).Show();
			o.Close();
		}

		#endregion

		#region ExitCommand

		private RelayCommand<MainMenuWindow> _exitCommand;

		public RelayCommand<MainMenuWindow> ExitCommand
		{
			get
			{
				return _exitCommand ?? (_exitCommand = new RelayCommand<MainMenuWindow>(
						   (o) => o.Close()));
			}
		}

		#endregion

		#endregion
	}
}
