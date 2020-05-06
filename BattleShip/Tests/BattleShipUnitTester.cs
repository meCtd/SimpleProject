using System;
using System.IO;
using System.Linq;
using BattleShip;
using NUnit.Framework;

namespace Battleship.Tests
{
    [SetUpFixture]
    internal class FolderCreater
    {
        public static readonly string SavePath = AppDomain.CurrentDomain.BaseDirectory+"\\TestFolder";
        [OneTimeSetUp]
        public void CreateTestFolder()
        {
            Directory.CreateDirectory(SavePath);
        }


        [OneTimeTearDown]
        public void DeleteTestFolder()
        {
            Directory.Delete(SavePath,true);
        }
    }

    [TestFixture]
    public class BattleShipUnitTester
    {
        [Category("Normal work")]
        [Test]
        public void TestGame()
        {
            SeaBattleGame testGame = new SeaBattleGame();
            do
            {
                testGame.Turn();
            } while ((testGame.ComputerPlayer.MyBoard.Ships.Count(s => s.Health != 0) == 0) ||
                     (testGame.HumanPlayer.MyBoard.Ships.Count(s => s.Health != 0) == 0));
        }



        [Category("Check save")]
        [Test]
        public void CheckSave()
        {

            GameController test = new GameController("Test",new ConsoleDrawer());
            test.NewGame();

            string savedFilePath = test.SaveGame(FolderCreater.SavePath);
            Assert.IsTrue(File.Exists(savedFilePath));

        }

        [Category("Check open")]
        [Test]
        public void OpenGame()
        {
            GameController test = new GameController("Test",new ConsoleDrawer());
            string filePath = test.SaveGame(FolderCreater.SavePath);
            test.NewGame();

            Assert.DoesNotThrow(() =>
            {
                test.OpenGame(filePath);
            });

        }
    }
}
