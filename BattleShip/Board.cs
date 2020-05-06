using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace BattleShip
{
    [Serializable]
    internal class Board
    {
        public List<Ship> Ships { get; }
        public BoardElementType[,] BoardElements { get; }

        private readonly CurrentBoardType _boardType;

        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);
        
        public BoardElementType this[Position pos]
        {
            get { return BoardElements[pos.X, pos.Y]; }
            set { BoardElements[pos.X, pos.Y] = value; }
        }

        public Board(CurrentBoardType type)
        {
            BoardElements = new BoardElementType[12, 12];
            _boardType = type;
            if (type == CurrentBoardType.MyBoard)
            {
                Ships = new List<Ship>(10);
            }
            SetBoard();
        }
        
        /// <summary>
        /// Set elements at board
        /// </summary>
        private void SetBoard()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {

                    // Set an borders on board
                    if ((i == 0 || i == 11 || j == 0 || j == 11))
                    {
                        BoardElements[i, j] = BoardElementType.Border;
                    }

                    else
                    {
                        BoardElements[i, j] = BoardElementType.NotChecked;
                    }
                }
            }


            if (_boardType == CurrentBoardType.MyBoard)
            {
                SetShips();
            }

        }

        /// <summary>
        /// Set random ships at board
        /// </summary>
        private void SetShips()
        {

            var startedShips = new List<Ship>(10);

            for (int i = 4, k = 1; i > 0; i--, k++)
            {
                for (int j = 1; j <= k; j++)
                {
                    startedShips.Add(new Ship(i, (ShipOrientation)Rand.Next(2)));
                }
            }

            foreach (var ship in startedShips)
            {
                bool shipIsCorrect = false;

                while (!shipIsCorrect)
                {
                    ship.StartPosition = new Position(Rand.Next(1, 11), Rand.Next(1, 11));

                    if (!CheckShips(ship))
                        continue;
                    Ships.Add(ship);
                    SetStatusAtShipPos(ship, BoardElementType.Ship);
                    shipIsCorrect = true;
                }
            }
        }

        /// <summary>
        /// Check can the ship place his position
        /// </summary>
        /// <param name="checkShip">Ship for check</param>
        /// <returns>true - if can place ship, otherwise - false</returns>
        private bool CheckShips(Ship checkShip)
        {
            if ((checkShip.Orientation == ShipOrientation.Horisontal) && (11 - checkShip.StartPosition.Y < checkShip.Size))
            {
                return false;
            }
            if ((checkShip.Orientation == ShipOrientation.Vertical) && (11 - checkShip.StartPosition.X < checkShip.Size))
            {

                return false;
            }

            foreach (Ship ship in Ships)
            {
                if (Ship.GetShipArea(ship).Any(checkShip.IsLocated))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Update board after shot
        /// </summary>
        /// <param name="shotPosition">Shot position</param>
        /// <returns>Reference on shooted ship which has coordinates of shootPosition, otherwise - null</returns>
        public Ship UpdateBoard(Position shotPosition)
        {
            Ship targetShip = GetShootedShip(shotPosition);

            // If shoted into the ship
            if (targetShip != null)
            {
                if (BoardElements[shotPosition.X, shotPosition.Y] == BoardElementType.Ship)
                {
                    targetShip.Health--;
                    BoardElements[shotPosition.X, shotPosition.Y] = BoardElementType.WoundedShip;
                }

                // If ship is killed
                if (targetShip.Health == 0)
                {
                    SetStatusAtShipPos(targetShip, BoardElementType.KilledShip);
                   
                }
                return targetShip;
                 
            }
            //If miss
            else
            {
                BoardElements[shotPosition.X, shotPosition.Y] = BoardElementType.Empty;
                return null;
            }
            

        }


        /// <summary>
        /// Set on ship's position choosen BoardElemType
        /// </summary>
        /// <param name="selectedShip">Ship</param>
        /// <param name="elem">Type</param>
        public void SetStatusAtShipPos(Ship selectedShip, BoardElementType elem)
        {

            switch (selectedShip.Orientation)
            {
                case ShipOrientation.OneSizeShip:
                    BoardElements[selectedShip.StartPosition.X, selectedShip.StartPosition.Y] = elem;
                    break;

                case ShipOrientation.Horisontal:
                    for (int j = selectedShip.StartPosition.Y; j < selectedShip.StartPosition.Y + selectedShip.Size; j++)
                    {
                        BoardElements[selectedShip.StartPosition.X, j] = elem;
                    }

                    break;

                case ShipOrientation.Vertical:
                    for (int i = selectedShip.StartPosition.X; i < selectedShip.StartPosition.X + selectedShip.Size; i++)
                    {
                        BoardElements[i, selectedShip.StartPosition.Y] = elem;
                    }
                    break;
            }


            if (elem == BoardElementType.KilledShip)
            {
                //Set an empty Positions around killed ship
                foreach (Position positions in Ship.GetPositionsAroundShip(selectedShip))
                {
                    if (this[positions] != BoardElementType.Border)
                        this[positions] = BoardElementType.Empty;
                }
            }

        }
        
        /// <summary>
        /// Find ship at desired position
        /// </summary>
        /// <param name="possibleShiPosition">Desired position</param>
        /// <returns>Referense on shooted ship, otherwise - null</returns>
        private Ship GetShootedShip(Position possibleShiPosition)
        {

            foreach (Ship ship in Ships)
            {
                if (ship.IsLocated(possibleShiPosition))
                    return ship;
            }
            return null;
        }
        
    }
}
