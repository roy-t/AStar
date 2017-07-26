using System;

namespace RoyT.AStar
{
    /// <summary>
    /// A 2D position structure
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X-position
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y-position
        /// </summary>
        public int Y { get; }

        public override string ToString() => $"({this.X}, {this.Y})";

        public bool Equals(Position other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }
    }
}
