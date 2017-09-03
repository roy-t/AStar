using System;

namespace RoyT.AStar
{
    /// <summary>
    /// A 2D offset structure. You can use an array of offsets to represent the movement pattern
    /// of your agent, for example an offset of (-1, 0) means your character is able
    /// to move a single cell to the left <see cref="MovementPatterns"/> for some predefined
    /// options.
    /// </summary>
    public struct Offset : IEquatable<Offset>
    {
        public Offset(int x, int y)
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

        public override string ToString() => $"Offset: ({this.X}, {this.Y})";

        public bool Equals(Offset other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Offset && Equals((Offset)obj);
        }

        public static bool operator ==(Offset a, Offset b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Offset a, Offset b)
        {
            return !a.Equals(b);
        }

        public static Offset operator +(Offset a, Offset b)
        {
            return new Offset(a.X + b.X, a.Y + b.Y);
        }

        public static Offset operator -(Offset a, Offset b)
        {
            return new Offset(a.X - b.X, a.Y - b.Y);
        }

        public static Position operator +(Offset a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Offset a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public static Position operator +(Position a, Offset b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Position a, Offset b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
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
