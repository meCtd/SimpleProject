using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShip
{
    [DebuggerDisplay("S:{Size},H:{Health},(S.x{StartPosition.X},S.y{StartPosition.Y})")]
    [Serializable]
    internal class Ship
    {
        /// <summary>
        /// Current health of ship
        /// </summary>
        /// 
        public int Health { get; set; }

        /// <summary>
        /// Size of ship 
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Ship's orientation
        /// </summary>
        public ShipOrientation Orientation { get; }

        public Position StartPosition { get; set; }

        public Ship(int size)
        {
            Size = size;
            Health = size;
        }

        public Ship(int size, ShipOrientation orientation)
        {
            Size = size;
            Health = size;
            Orientation = size == 1 ? ShipOrientation.OneSizeShip : orientation;
        }

        /// <summary>
        /// Checks if the current ship is in the desired location
        /// </summary>
        /// <param name="desiredPosition">Desired Position</param>
        /// <returns></returns>
        public bool IsLocated(Position desiredPosition)
        {
            switch (Orientation)
            {
                case ShipOrientation.OneSizeShip:
                    return StartPosition == desiredPosition;

                case ShipOrientation.Horisontal:
                    if (desiredPosition.X == StartPosition.X)
                    {
                        return desiredPosition.Y >= StartPosition.Y &&
                               desiredPosition.Y < StartPosition.Y + Size;
                    }
                    break;
                case ShipOrientation.Vertical:
                    if (desiredPosition.Y == StartPosition.Y)
                    {
                        return desiredPosition.X >= StartPosition.X &&
                               desiredPosition.X < StartPosition.X + Size;
                    }
                    break;
            }
            return false;
        }
        
        /// <summary>
        /// Return enumerable of positions around ship
        /// </summary>
        /// <param name="ship"> Ship</param>
        /// <returns>Enumerable of positions</returns>
        public static IEnumerable<Position> GetPositionsAroundShip(Ship ship)
        {
            List<Position> positionsAroundShip = new List<Position>(14);


            switch (ship.Orientation)
            {
                case ShipOrientation.OneSizeShip:
                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y + 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X, ship.StartPosition.Y + 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, ship.StartPosition.Y));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, ship.StartPosition.Y + 1));
                    break;

                case ShipOrientation.Horisontal:

                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, ship.StartPosition.Y - 1));

                    for (int i = ship.StartPosition.Y; i < ship.StartPosition.Y + ship.Size; i++)
                    {
                        positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, i));
                        positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, i));

                    }

                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y + 1 + (ship.Size - 1)));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X, ship.StartPosition.Y + 1 + (ship.Size - 1)));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + 1, ship.StartPosition.Y + 1 + (ship.Size - 1)));
                    break;


                case ShipOrientation.Vertical:

                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X - 1, ship.StartPosition.Y + 1));

                    for (int i = ship.StartPosition.X; i < ship.StartPosition.X + ship.Size; i++)
                    {
                        positionsAroundShip.Add(new Position(i, ship.StartPosition.Y + 1));
                        positionsAroundShip.Add(new Position(i, ship.StartPosition.Y - 1));

                    }
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + ship.Size, ship.StartPosition.Y - 1));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + ship.Size, ship.StartPosition.Y));
                    positionsAroundShip.Add(new Position(ship.StartPosition.X + ship.Size, ship.StartPosition.Y + 1));

                    break;
            }

            return positionsAroundShip;

        }
        
        /// <summary>
        /// Returns all positions the ship occupies
        /// </summary>
        /// <param name="ship">Target ship</param>
        /// <returns>Enumerable of occupied positions</returns>
        public static IEnumerable<Position> GetShipArea(Ship ship)
        {

            List<Position> shipArea = new List<Position>(18);
            shipArea.AddRange(GetPositionsAroundShip(ship));

            switch (ship.Orientation)
            {
                case ShipOrientation.OneSizeShip:
                    shipArea.Add(ship.StartPosition);
                    break;



                case ShipOrientation.Horisontal:
                    for (int i = ship.StartPosition.Y; i < ship.StartPosition.Y + ship.Size; i++)
                    {
                        shipArea.Add(new Position(ship.StartPosition.X, i));
                    }
                    break;

                case ShipOrientation.Vertical:
                    for (int i = ship.StartPosition.X; i < ship.StartPosition.X + ship.Size; i++)
                    {
                        shipArea.Add(new Position(i, ship.StartPosition.Y));
                    }
                    break;
            }

            return shipArea;
        }
    }
}
