using System;

namespace Roy_T.AStar.Primitives
{
    public struct Position : IEquatable<Position>
    {
        public static Position Zero => new Position(0, 0);

        public Position(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Position FromOffset(Distance xDistanceFromOrigin, Distance yDistanceFromOrigin)
            => new Position(xDistanceFromOrigin.Meters, yDistanceFromOrigin.Meters);

        public float X { get; }
        public float Y { get; }

        public static bool operator ==(Position a, Position b)
            => a.Equals(b);

        public static bool operator !=(Position a, Position b)
            => !a.Equals(b);

        public override string ToString() => $"({this.X:F2}, {this.Y:F2})";

        public override bool Equals(object obj) => obj is Position position && this.Equals(position);

        public bool Equals(Position other) => this.X == other.X && this.Y == other.Y;

        public override int GetHashCode() => -1609761766 + this.X.GetHashCode() + this.Y.GetHashCode();
    }
}
