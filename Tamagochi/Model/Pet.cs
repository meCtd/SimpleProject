using System;
using System.IO;
using System.Xml.Serialization;

namespace Tamagotchi.Model
{
	public class Pet
	{
		#region Fields

		private string _avatar;
		private bool _isAlive;
		private double _currentAge;
		private double _health;
		private double _mood;
		private double _purity;
		private double _satiety;

		public static string SavedPath = AppDomain.CurrentDomain.BaseDirectory + @"SavedPet.tmg";

		#endregion

		#region Properties

		public string Name { get; set; }

		public int MaxAge { get; set; }

		public PetsEnum PetType { get; set; }

		public string Avatar
		{
			get { return _avatar; }
			set
			{
				_avatar = value;
				AnIndicatorChanged(nameof(Avatar));
			}
		}

		public double CurrentAge
		{
			get { return _currentAge; }
			set
			{
				_currentAge = value;
				AnIndicatorChanged(nameof(CurrentAge));
			}
		}

		public double Health
		{
			get { return _health; }
			set
			{
				_health = value < 0 ? 0 : value >= 1 ? 1 : value;

				AnIndicatorChanged(nameof(Health));
			}
		}

		public double Mood
		{
			get { return _mood; }
			set
			{
				_mood = value < 0 ? 0 : value >= 1 ? 1 : value;
				AnIndicatorChanged(nameof(Mood));
			}
		}

		public double Purity
		{
			get { return _purity; }
			set
			{
				_purity = value < 0 ? 0 : value >= 1 ? 1 : value;
				AnIndicatorChanged(nameof(Purity));
			}
		}

		public double Satiety
		{
			get { return _satiety; }
			set
			{
				_satiety = value < 0 ? 0 : value >= 1.6 ? 1.6 : value;
				AnIndicatorChanged(nameof(Satiety));
			}
		}

		public bool IsAlive
		{
			get { return _isAlive; }
			set
			{
				_isAlive = value;
				AnIndicatorChanged(nameof(IsAlive));
			}
		}

		public event EventHandler<StatsChanged> IndicatorChanged;

		#endregion

		private void AnIndicatorChanged(string indicatorName)
		{
			IndicatorChanged?.Invoke(this, new StatsChanged(indicatorName));
		}

		public Pet()
		{

		}

		public Pet(string name, PetsEnum type)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			PetType = type;
			MaxAge = new Random(DateTime.Today.Millisecond).Next(10, 21);
			_isAlive = true;
			_currentAge = 0;
			SetPetStats();
		}

		private void SetPetStats()
		{
			switch (PetType)
			{
				case PetsEnum.Cat:
					_health = 0.75;
					_mood = 0.9;
					_purity = 0.6;
					_satiety = 0.5;
					_avatar = @"/Resources/Cat.png";
					break;

				case PetsEnum.Panda:
					_health = 0.6;
					_mood = 1;
					_purity = 0.9;
					_satiety = 0.75;
					_avatar = @"/Resources/Panda.png";
					break;

				case PetsEnum.Parrot:
					_health = 0.75;
					_mood = 0.9;
					_purity = 0.6;
					_satiety = 0.8;
					_avatar = @"/Resources/Parrot.png";
					break;

				case PetsEnum.Turtle:
					_health = 0.1;
					_mood = 0.7;
					_purity = 0.9;
					_satiety = 0.75;
					_avatar = @"/Resources/Turtle.png";
					break;
			}
		}

		public static Pet Awake()
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Pet));
				using (FileStream file = new FileStream(SavedPath, FileMode.Open))
				{
					return (Pet)serializer.Deserialize(file);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Sleep()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Pet));
			
			using (FileStream file = new FileStream(SavedPath, FileMode.Create))
			{
				serializer.Serialize(file, this);
			}
		}
	}
}
