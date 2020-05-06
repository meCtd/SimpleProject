using System.ComponentModel;
using System.Windows;
using Tamagotchi.ViewModel;

namespace Tamagotchi.View
{
    /// <summary>
    /// Логика взаимодействия для ChosePetWindow.xaml
    /// </summary>
    public partial class ChosePetWindow : Window
    {
        public ChosePetWindow()
        {
            InitializeComponent();
            DataContext = new ChooseWindowViewModel();
        }
        
    }
}
