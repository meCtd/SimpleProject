using System;

namespace BattleShip
{
    [Serializable]
    internal abstract class SeaBattlePlayer
    {
        public Board MyBoard { get; }

        public Board OpponentsBoard { get; }

        public Position CurrentPosition { get; protected set; }

        protected SeaBattlePlayer()
        {
            MyBoard = new Board(CurrentBoardType.MyBoard);
            OpponentsBoard = new Board(CurrentBoardType.OpponentBoard);
            CurrentPosition = new Position(1, 1);
        }

        public virtual Position Shot()
        {
            return CurrentPosition;
        }

        public virtual Ship GetShot(Position targetPosition)
        {
            return MyBoard.UpdateBoard(targetPosition);
        }
    }
}
