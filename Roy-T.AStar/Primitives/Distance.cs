using System;

namespace Roy_T.AStar.Primitives
{
    public struct Distance : IComparable<Distance>, IEquatable<Distance>
    {
        public static Distance Zero => new Distance(0);

        private Distance(float meters)
        {
            this.Meters = meters;
        }

        public float Meters { get; }

        public static Distance FromMeters(float meters) => new Distance(meters);

        public static Distance BeweenPositions(Position a, Position b)
        {
            var sX = a.X;
            var sY = a.Y;
            var eX = b.X;
            var eY = b.Y;

            var d0 = (eX - sX) * (eX - sX);
            var d1 = (eY - sY) * (eY - sY);

            return FromMeters((float)Math.Sqrt(d0 + d1));
        }

        public static Distance operator +(Distance a, Distance b)
            => new Distance(a.Meters + b.Meters);

        public static Distance operator -(Distance a, Distance b)
            => new Distance(a.Meters - b.Meters);

        public static Distance operator *(Distance a, float b)
            => new Distance(a.Meters * b);

        public static Distance operator /(Distance a, float b)
            => new Distance(a.Meters / b);

        public static bool operator >(Distance a, Distance b)
            => a.Meters > b.Meters;

        public static bool operator <(Distance a, Distance b)
            => a.Meters < b.Meters;

        public static bool operator >=(Distance a, Distance b)
            => a.Meters >= b.Meters;

        public static bool operator <=(Distance a, Distance b)
            => a.Meters <= b.Meters;

        public static bool operator ==(Distance a, Distance b)
            => a.Equals(b);

        public static bool operator !=(Distance a, Distance b)
            => !a.Equals(b);

        public static Duration operator /(Distance distance, Velocity velocity)
          => Duration.FromSeconds(distance.Meters / velocity.MetersPerSecond);

        public override string ToString() => $"{this.Meters:F2}m";

        public override bool Equals(object obj) => obj is Distance distance && this.Equals(distance);
        public bool Equals(Distance other) => this.Meters == other.Meters;

        public int CompareTo(Distance other) => this.Meters.CompareTo(other.Meters);

        public override int GetHashCode() => -1609761766 + this.Meters.GetHashCode();
    }
}
