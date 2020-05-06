using System;
using System.Windows;
using Tamagotchi.Model;

namespace Tamagotchi.ViewModel
{
    public class GameWindowViewModel : ObservableObject
    {
        private Game _game;

        public bool IsPaused
        {
            get { return _game.IsPaused; }
            set
            {
                _game.IsPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }

        public PetViewModel Pet { get; }

        public GameWindowViewModel(string name, PetsEnum petType) : this(new Pet(name, petType))
        {
        }

        public GameWindowViewModel(Pet pet)
        {
            Pet = new PetViewModel(pet);
            _game = new Game(pet);
            _game.OverFeeded += OverFeededMessage;
        }

        private void OverFeededMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Your pet was overfeeded, indicators have been decreased!", "Attention!", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #region Commands

        #region FeedCommand

        private RelayCommand _feedCommand;

        public RelayCommand FeedCommand
        {
            get
            {
                return _feedCommand ?? (_feedCommand = new RelayCommand(
                           (o) => _game.Feed(),
                           CanAction));
            }
        }

        #endregion

        #region KillCommand

        private RelayCommand _killCommand;

        public RelayCommand KillCommand
        {
            get
            {
                return _killCommand ?? (_killCommand = new RelayCommand(
                           (o) => KillPet(),
                           CanAction));
            }
        }

        private void KillPet()
        {
            MessageBoxResult result = MessageBox.Show($"Do you really want to kill {Pet.Name} ?", "Question",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                _game.Kill();
        }

        #endregion

        #region CleanCommand

        private RelayCommand _cleanCommand;

        public RelayCommand CleanCommand
        {
            get
            {
                return _cleanCommand ?? (_cleanCommand = new RelayCommand(
                           (o) => _game.Clean(),
                           CanAction));
            }
        }

        #endregion

        #region PlayCommand

        private RelayCommand _playCommand;

        public RelayCommand PlayCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new RelayCommand(
                           (o) => _game.Play(),
                           CanAction));
            }
        }

        #endregion

        #region PauseCommand

        private RelayCommand _pauseCommand;

        public RelayCommand PauseCommand
        {
            get
            {
                return _pauseCommand ?? (_pauseCommand = new RelayCommand(
                           (o) => IsPaused = !IsPaused,
                           (o) => Pet.IsAlive));
            }
        }

        #endregion

        #endregion

        private bool CanAction(object o)
        {
            return !_game.IsPaused && Pet.IsAlive;
        }
    }
}



