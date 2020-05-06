using System;
using System.Threading;

namespace Tamagotchi.Model
{
	public class Game
	{
		#region Fields

		private bool _isPaused;

		private Timer _gameTimer;

		#region CriticalValues
		private const double CriticalSatietylValue = 0.3;
		private const double CriticalMoodValue = 0.3;
		private const double CriticalPurityValue = 0.3;
		#endregion

		#region PenaltyValues

		private const double HealthPenaltyForSatiety = 0.1;
		private const double MoodPenaltyForSatiety = 0.05;
		private const double HealthPenaltyForMood = 0.05;
		private const double HealthPenaltyForPurity = 0.1;
		private const double MoodPenaltyForPurity = 0.15;

		#endregion

		#endregion

		#region Properties

		#region Events

		public event EventHandler OverFeeded;

		private void AnOverFeeded()
		{
			OverFeeded?.Invoke(this, EventArgs.Empty);
		}

		private event EventHandler PauseChanged;

		private void AnPauseChanged()
		{
			PauseChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		public bool IsPaused
		{
			get { return _isPaused; }
			set
			{
				_isPaused = value;
				AnPauseChanged();
			}
		}

		public Pet Pet { get; }

		#endregion

		public Game(Pet pet)
		{
			Pet = pet ?? throw new ArgumentNullException(nameof(pet));
			_isPaused = false;

			PauseChanged += TimerController;
			OverFeeded += CheckFoodLimit;
			Pet.IndicatorChanged += CheckPetStats;

			_gameTimer = new Timer(UpdateStatsPerTicks, null, 500, 1000);
		}

		private void TimerController(object sender, EventArgs e)
		{
			if (IsPaused)
				_gameTimer.Dispose();
			else
				_gameTimer = new Timer(UpdateStatsPerTicks, null, 500, 1000);

		}

		private void CheckPetStats(object sender, StatsChanged e)
		{
			if (Pet.IsAlive)
			{
				if (Pet.Health <= 0 || Pet.CurrentAge >= Pet.MaxAge)
				{
					Kill();
				}
			}
		}

		private void UpdateStatsPerTicks(object state)
		{
			if (Pet.IsAlive)
			{
				switch (Pet.PetType)
				{
					case PetsEnum.Cat:

						Pet.Satiety -= 0.0042;
						Pet.Mood -= 0.0024;
						Pet.Purity -= 0.0031;
						break;

					case PetsEnum.Panda:

						Pet.Satiety -= 0.0022;
						Pet.Mood -= 0.0044;
						Pet.Purity -= 0.0021;
						break;

					case PetsEnum.Parrot:

						Pet.Satiety -= 0.0029;
						Pet.Mood -= 0.0026;
						Pet.Purity -= 0.0051;
						break;

					case PetsEnum.Turtle:

						Pet.Satiety -= 0.0036;
						Pet.Mood -= 0.0034;
						Pet.Purity -= 0.0025;
						break;

				}
				Pet.CurrentAge += 1d / 60;
				Penaltys();
				HealthRegen();

			}
			else
			{
				_gameTimer.Dispose();
			}
		}

		private void Penaltys()
		{
			if (Pet.Mood < CriticalMoodValue)
			{
				Pet.Health -= HealthPenaltyForMood;
			}

			if (Pet.Purity < CriticalPurityValue)
			{
				Pet.Health -= HealthPenaltyForPurity;
				Pet.Mood -= MoodPenaltyForPurity;
			}

			if (Pet.Satiety < CriticalSatietylValue)
			{
				Pet.Health -= HealthPenaltyForSatiety;
				Pet.Mood -= MoodPenaltyForSatiety;
			}

		}

		private void HealthRegen()
		{
			if (Pet.Mood > 0.75 && Pet.Purity > 0.75 && Pet.Satiety > 0.75)
				Pet.Health += 0.005;
		}

		public void CheckFoodLimit(object sender, EventArgs e)
		{
			Pet.Health = 0.63 * Pet.Health;
			Pet.Mood = 0.6 * Pet.Mood;
			Pet.Satiety = 0.5 * Pet.Satiety;
			Pet.Purity = 0.72 * Pet.Purity;

		}

		#region ActionsWithPet

		public void Play()
		{
			Pet.Mood += Pet.Mood * 0.05;
			Pet.Satiety -= Pet.Satiety * 0.015;
			Pet.Purity -= Pet.Purity * 0.009;
		}

		public void Feed()
		{
			Pet.Satiety += 0.07;
			Pet.Health += 0.02;
			Pet.Purity -= 0.06;
			if (Pet.Satiety >= 1.5)
				AnOverFeeded();
		}

		public void Clean()
		{
			Pet.Purity += 0.07;
			Pet.Mood -= 0.01;
			Pet.Satiety -= 0.02;
		}

		public void Kill()
		{
			Pet.IsAlive = false;
			Pet.Health = 0;
			Pet.Mood = 0;
			Pet.Purity = 0;
			Pet.Satiety = 0;
			Pet.Avatar = @"/Resources/Dead.png";

		}

		#endregion

	}
}

