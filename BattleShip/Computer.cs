using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip
{
    [Serializable]
    internal class Computer : SeaBattlePlayer
    {
        private readonly Random _rand;

        public Computer() : base()
        {
            _rand = new Random(DateTime.Now.Millisecond);

        }

        /// <summary>
        /// Computer's shot
        /// </summary>
        /// <returns>Random position on board</returns>
        public override Position Shot()
        {
            List<Position> possibleShootList = CheckForPosition(BoardElementType.NotChecked);

            if (possibleShootList.Count == 0)
                throw new Exception("There are no possible positions for the shot!");


            List<Position> woundedPositions = CheckForPosition(BoardElementType.WoundedShip);

            //if Opponent's board doesnt contain an wounded ship
            if (woundedPositions.Count == 0)
            {
                CurrentPosition = possibleShootList[_rand.Next(0, possibleShootList.Count - 1)];
            }
            else
            {
                //Positions around wounded ship
                List<Position> aroundWoundedShipsPositions = new List<Position>();
                foreach (var woundedPosition in woundedPositions)
                {
                    aroundWoundedShipsPositions.AddRange(possibleShootList.Where(s =>
                        ((Math.Abs(woundedPosition.X - s.X) == 1 && woundedPosition.Y == s.Y) ||
                         (Math.Abs(woundedPosition.Y - s.Y) == 1 && woundedPosition.X == s.X)) &&
                        OpponentsBoard[s] != BoardElementType.WoundedShip && OpponentsBoard[s] != BoardElementType.Empty));
                }
                CurrentPosition = aroundWoundedShipsPositions[_rand.Next(0, aroundWoundedShipsPositions.Count - 1)];
            }
            return CurrentPosition;
        }

        private List<Position> CheckForPosition(BoardElementType type)
        {
            List<Position> targetList = new List<Position>();
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (OpponentsBoard.BoardElements[i, j] == type)
                    {
                        targetList.Add(new Position(i, j));
                    }
                }
            }
            return targetList;
        }
    }
}
