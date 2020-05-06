using Tamagotchi.Model;
using Tamagotchi.View;

namespace Tamagotchi.ViewModel
{
    class ChooseWindowViewModel : ObservableObject
    {
        #region Fields

        private string _petName;
        private PetsEnum _petType;

        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _petName;
            }
            set
            {
                _petName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public PetsEnum Type
        {
            get
            {
                return _petType;
            }
            set
            {
                _petType = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        #endregion

        public ChooseWindowViewModel()
        {

        }

        #region Commands

        private RelayCommand<ChosePetWindow> _openGameWindowCommand;

        public RelayCommand<ChosePetWindow> OpenGameWindowCommand
        {
            get
            {
                return _openGameWindowCommand ?? (_openGameWindowCommand = new RelayCommand<ChosePetWindow>(
                           OpenGameVindow,
                          (o) => !string.IsNullOrWhiteSpace(Name) && Type != PetsEnum.None));
            }
        }

        private void OpenGameVindow(ChosePetWindow o)
        {
            ChooseWindowViewModel data = (ChooseWindowViewModel)o.DataContext;
            new GameWindow(new GameWindowViewModel(data.Name, data.Type)).Show();
            o.Close();
        }

        #endregion

    }
}



