using System;

namespace RoyT.AStar
{
    /// <summary>
    /// A 2D displacement structure. You can use an array of displacements to represent the shape of the agent.
    /// </summary>
    public struct Displacement : IEquatable<Displacement>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x-displacement</param>
        /// <param name="y">y-displacement</param>
        public Displacement(int x, int y)
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

        public override string ToString() => $"Displacement: ({this.X}, {this.Y})";

        public bool Equals(Displacement other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Displacement && Equals((Displacement)obj);
        }

        public static bool operator ==(Displacement a, Displacement b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Displacement a, Displacement b)
        {
            return !a.Equals(b);
        }

        public static Position operator +(Displacement a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Displacement a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public static Position operator +(Position a, Displacement b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Position a, Displacement b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 413) ^ this.Y;
            }
        }
    }
}
