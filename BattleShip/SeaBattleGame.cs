using System;

namespace BattleShip
{
    [Serializable]
    internal class SeaBattleGame
    {
        public SeaBattlePlayer HumanPlayer { get; }

        public SeaBattlePlayer ComputerPlayer { get; }

        /// <summary>
        /// Current turn: true - human , false - computer
        /// </summary>
        public bool CurrentTurn { get; private set; }

        public bool Hack { get; set; }

        public SeaBattleGame()
        {
            //Set the current turn
            CurrentTurn = new Random().Next(0, 2) == 1;
            HumanPlayer = new Human();
            ComputerPlayer = new Computer();
            Hack = false;
        }

        public void Turn()
        {
            SeaBattlePlayer firstPlayer;
            SeaBattlePlayer secondPlayer;

            if (CurrentTurn)
            {
                firstPlayer = HumanPlayer;
                secondPlayer = ComputerPlayer;

            }
            else
            {
                firstPlayer = ComputerPlayer;
                secondPlayer = HumanPlayer;
            }
            Position shotPosition = firstPlayer.Shot();
            Ship targetShip = secondPlayer.MyBoard.UpdateBoard(shotPosition);

            if (targetShip == null)
                firstPlayer.OpponentsBoard[shotPosition] = BoardElementType.Empty;

            if (targetShip != null && firstPlayer.OpponentsBoard[shotPosition] != BoardElementType.KilledShip && firstPlayer.OpponentsBoard[shotPosition] != BoardElementType.WoundedShip)
            {
                if (targetShip.Health == 0)
                    firstPlayer.OpponentsBoard.SetStatusAtShipPos(targetShip, BoardElementType.KilledShip);
                else
                    firstPlayer.OpponentsBoard[shotPosition] = BoardElementType.WoundedShip;
                return;
            }

            CurrentTurn = !CurrentTurn;
        }
    }
}
