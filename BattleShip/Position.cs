using System;
using System.Diagnostics;

namespace BattleShip
{
    /// <summary>
    /// Describes a point in a two-dimensional space with integer coordinates
    /// </summary>
    [DebuggerDisplay("x={X}, y={Y}")]
    [Serializable]
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return (pos1.X == pos2.X) && (pos1.Y == pos2.Y);
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return !(pos1 == pos2);
        }


        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return obj is Position && Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}
