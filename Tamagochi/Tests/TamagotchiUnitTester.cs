using System;
using System.IO;
using NUnit.Framework;
using Tamagotchi.Model;

namespace Tamagotchi.Tests
{
	[TestFixture]
	class TamagotchiUnitTester
	{
		[Test]
		public void NullValueTest()
		{
			Assert.Throws<ArgumentNullException>(() => new Game(null));
		}

		[Test]
		public void TestGameKillPet()
		{
			Pet testPet = new Pet("Test", PetsEnum.Parrot);
			Game testGame = new Game(testPet);
			testGame.Kill();
			Assert.IsFalse(testPet.IsAlive);
		}

		[Test]
		public void TestFeed()
		{
			Pet testPet = new Pet("Test", PetsEnum.Parrot);
			double feedValue = testPet.Satiety;
			Game testGame = new Game(testPet);
			testGame.Feed();
			Assert.IsTrue(testPet.Satiety > feedValue);

		}

		[Test]
		public void TestPlay()
		{
			Pet testPet = new Pet("Test", PetsEnum.Parrot);
			double moodValue = testPet.Mood;
			Game testGame = new Game(testPet);
			testGame.Play();
			Assert.IsTrue(testPet.Mood > moodValue);

		}

		[Test]
		public void TestClean()
		{
			Pet testPet = new Pet("Test", PetsEnum.Parrot);
			double cleanValue = testPet.Purity;
			Game testGame = new Game(testPet);
			testGame.Clean();
			Assert.IsTrue(testPet.Satiety > cleanValue);

		}

		[Test]
		public void TestSaveGame()
		{
			if (File.Exists(Pet.SavedPath))
				File.Delete(Pet.SavedPath);
			Pet testPet = new Pet("Test", PetsEnum.None);
			testPet.Sleep();
			Assert.IsTrue(File.Exists(Pet.SavedPath));
		}

		[Test]
		public void OpenSavedGame()
		{
			if (File.Exists(Pet.SavedPath))
				File.Delete(Pet.SavedPath);
			new Pet("Test", PetsEnum.None).Sleep();
			Pet savedPet = Pet.Awake();
			Assert.NotNull(savedPet);


		}


	}
	[SetUpFixture]
	class DeleteTestSavedfile
	{

		[OneTimeTearDown]
		public void DeleteTestFolder()
		{
			if (File.Exists(Pet.SavedPath))
			{
				File.Delete(Pet.SavedPath);
			}
		}
	}

}

