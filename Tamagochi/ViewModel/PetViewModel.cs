using System.Windows;
using System.Windows.Media;
using Tamagotchi.Model;

namespace Tamagotchi.ViewModel
{
	public class PetViewModel : ObservableObject
	{
		private Pet _pet;

		#region Properties

		public string Name => _pet.Name;

		public PetsEnum PetType => _pet.PetType;

		public int MaxAge => _pet.MaxAge;

		public double CurrentAge => _pet.CurrentAge;

		public bool IsAlive => _pet.IsAlive;

		public string Avatar => _pet.Avatar;

		public double Health => _pet.Health;

		public double Mood => _pet.Mood;

		public double Purity => _pet.Purity;

		public double Satiety => _pet.Satiety;

		public Brush HealthColor => GetPropertyColor(Health);

		public Brush MoodColor => GetPropertyColor(Mood);

		public Brush PurityColor => GetPropertyColor(Purity);

		public Brush SatietyColor => GetPropertyColor(Satiety);

		#endregion
		
		public PetViewModel(Pet pet)
		{
			_pet = pet;
			_pet.IndicatorChanged += PetIndicatorChanged;
			pet.IndicatorChanged += UpdateColors;
		}
		
		private void UpdateColors(object sender,StatsChanged e)
		{
			OnPropertyChanged(nameof(HealthColor));
			OnPropertyChanged(nameof(MoodColor));
			OnPropertyChanged(nameof(PurityColor));
			OnPropertyChanged(nameof(SatietyColor));
		}
		
		private void PetIndicatorChanged(object sender, StatsChanged e)
		{
			OnPropertyChanged(e.PropertyName);
		}

		private LinearGradientBrush GetPropertyColor(double value)
		{
			GradientStopCollection colors = new GradientStopCollection(2);
			if (value >= 0.75)
			{

				colors.Add(new GradientStop(Colors.DarkGreen, 0));
				colors.Add(new GradientStop(Colors.LightGreen,1));

			}

			else if (value >= 0.3)
			{
				colors.Add(new GradientStop(Colors.DarkOrange, 0));
				colors.Add(new GradientStop(Colors.Yellow, 1));
			}

			else
			{
				colors.Add(new GradientStop(Colors.DarkRed, 0));
				colors.Add(new GradientStop(Colors.Red, 1));
			}
				 
			LinearGradientBrush gradient = new LinearGradientBrush(colors);
			gradient.StartPoint = new Point(1, 0.5);
			gradient.EndPoint = new Point(0, 0.5);
			return gradient;
		}

		#region Commands

		private RelayCommand _sleepCommand;

		public RelayCommand SleepCommand
		{
			get
			{
				return _sleepCommand ?? (_sleepCommand = new RelayCommand(
						   (o) => _pet.Sleep()));
			}
		}

		#endregion
	}
}
