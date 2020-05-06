using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    internal class Game
    {
        #region Fields
        /// <summary>
        /// Count of a numbers
        /// </summary>
        private  int _count;

        /// <summary>
        /// Collection of probability answers
        /// </summary>
        private  List<string> _collection;

        private readonly Random _random;
        #endregion

        #region Properties
        /// <summary>
        /// Current step count
        /// </summary>
        public int StepCount { get; private set; }

        /// <summary>
        /// Probably result of game
        /// </summary>
        public string PossibleResult { get; private set; }
        public string Result { get; private set; }

        #endregion

        public Game()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Start current game
        /// </summary>
        public void StartGame(int count)
        {
            if (count < 1)
                throw new ArgumentException("Wrong count of numbers!");

            StepCount = 0;
            Result = string.Empty;

            _count = count;
            _collection = new List<string>((int)Math.Pow(10, _count));

            for (int i = 0; i < Math.Pow(10, _count); i++)
                _collection.Add(i.ToString(new string('0', _count)));

            PossibleResult = _collection[_random.Next(0, _collection.Count)];

        }

        /// <summary>
        /// Set flag false for every wrong value in collection
        /// </summary>
        /// <param name="collection">Value in collection</param>
        /// <param name="input">Input value</param>
        /// <param name="bulls">Count of bulls</param>
        /// <param name="cows">Count of Cows</param>
        /// <returns>True if the number satisfies the conditions of the number of bulls and cows, else - false</returns>
        private static bool Filtration(string collection, string input, int bulls, int cows)
        {
            // Check bulls
            int x = 0;
            for (int i = 0; i < collection.Length; i++)
                if (input[i] == collection[i])
                {
                    collection = collection.Remove(i, 1);
                    input = input.Remove(i, 1);
                    x++;
                    i = -1;
                }

            if (x != bulls)
                return false;

            // Check cows
            int y = 0;
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
                            y++;
                            check = false;
                            break;
                        }
                    if (check == false)
                        break;
                }
            }
            while (check == false);

            if (cows != y)
                return false;

            return true;
        }

        public void NextTurn(int bulls, int cows)
        {
            if (bulls > _count || bulls < 0 || cows > _count || cows < 0 || bulls + cows > _count)
            {
                throw new ArgumentException("Wrong count of Bulls/Cows!");
            }

            StepCount++;

            if (bulls == _count)
            {
                Result = PossibleResult;
                return;
            }

            // Filtration
            for (int i = 0; i < _collection.Count; i++)
            {
                if (!(Filtration(_collection[i], PossibleResult, bulls, cows)))
                {
                    _collection.Remove(_collection[i]);
                    i--;
                }

            }

            // The worst case to throw base class of exception.
            if (_collection.Count == 0)
                throw new IndexOutOfRangeException("You did something wrong!");

            if (_collection.Count == 1)
                Result = _collection[0];
            PossibleResult = _collection[_random.Next(0, _collection.Count)];
        }
    }
}
