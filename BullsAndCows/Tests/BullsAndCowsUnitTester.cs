using System;
using NUnit.Framework;

namespace BullsAndCows.Tests
{
    [TestFixture]
    public class BullsAndCowsUnitTester
    {
        [Category("Count of nums")]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void CheckCountOfNumbers(int count)
        {
            Game game = new Game();
            Assert.Throws<ArgumentException>(() => game.StartGame(count));
        }
        
        [Category("Wrong arguments of Bulls/Cows")]
        [TestCase(1, 2, 0)]
        [TestCase(2, 1, 2)]
        [TestCase(4, 3, -1)]
        [TestCase(4, -1, 2)]
        public void CheckForWrongArgs(int count, int bulls, int cows)
        {
            Game game = new Game();
            game.StartGame(count);
            Assert.Throws<ArgumentException>(() => game.NextTurn(bulls, cows));
        }

        [Category("Wrong answers while playing")]
        [TestCase(4, 3, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(3, 2, 1)]
       public void CheckForWrongAnswers(int count, int bulls, int cows)
        {
            Game game = new Game();
            game.StartGame(count);
            Assert.Throws<IndexOutOfRangeException>(() => game.NextTurn(bulls, cows));
        }

        [Category("Check game for result")]
        [TestCase(1, "9")]
        [TestCase(2, "13")]
        [TestCase(3, "666")]
        [TestCase(3, "123")]
        [TestCase(4, "0000")]
        [TestCase(4, "1212")]
        [TestCase(4, "9876")]
        public void CheckCalculaion(int count, string number)
        {
            Game currentGame = new Game();
            currentGame.StartGame(count);
            do
            {
                int bulls;
                int cows;
                ArgsValues(currentGame.PossibleResult, number, out bulls, out cows);
                currentGame.NextTurn(bulls, cows);
            } while (currentGame.Result == string.Empty);

            Assert.AreEqual(number, currentGame.Result);
        }

        //Правильно ли тут создавать private методы???
        private static void ArgsValues(string collection, string input, out int bulls, out int cows)
        {

            bulls = 0;
            for (int i = 0; i < collection.Length; i++)
                if (input[i] == collection[i])
                {
                    collection = collection.Remove(i, 1);
                    input = input.Remove(i, 1);
                    bulls++;
                    i = -1;
                }
            cows = 0;
            bool check;
            do
            {
                check = true;

                for (int i = 0; i < collection.Length; i++)
                {
                    for (int j = 0; j < input.Length; j++)
                        if (input[j] == collection[i])
                        {
                            input = input.Remove(j, 1);
                            collection = collection.Remove(i, 1);
                            cows++;
                            check = false;
                            break;
                        }
                    if (check == false)
                        break;
                }
            }
            while (check == false);
        }
    }
}
