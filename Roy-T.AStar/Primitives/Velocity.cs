using System;

namespace Roy_T.AStar.Primitives
{
    public struct Velocity : IComparable<Velocity>, IEquatable<Velocity>
    {
        private Velocity(float metersPerSecond)
        {
            this.MetersPerSecond = metersPerSecond;
        }

        public float MetersPerSecond { get; }

        public float KilometersPerHour => this.MetersPerSecond * 3.6f;


        public static Velocity FromMetersPerSecond(float metersPerSecond)
            => new Velocity(metersPerSecond);

        public static Velocity FromKilometersPerHour(float kilometersPerHour)
            => new Velocity(kilometersPerHour / 3.6f);

        public static Velocity operator +(Velocity a, Velocity b)
           => new Velocity(a.MetersPerSecond + b.MetersPerSecond);

        public static Velocity operator -(Velocity a, Velocity b)
            => new Velocity(a.MetersPerSecond - b.MetersPerSecond);

        public static bool operator >(Velocity a, Velocity b)
            => a.MetersPerSecond > b.MetersPerSecond;

        public static bool operator <(Velocity a, Velocity b)
            => a.MetersPerSecond < b.MetersPerSecond;

        public static bool operator >=(Velocity a, Velocity b)
            => a.MetersPerSecond >= b.MetersPerSecond;

        public static bool operator <=(Velocity a, Velocity b)
            => a.MetersPerSecond <= b.MetersPerSecond;

        public static bool operator ==(Velocity a, Velocity b)
            => a.Equals(b);

        public static bool operator !=(Velocity a, Velocity b)
            => !a.Equals(b);

        public override string ToString() => $"{this.MetersPerSecond:F2} m/s";

        public override bool Equals(object obj) => obj is Velocity velocity && this.MetersPerSecond == velocity.MetersPerSecond;

        public bool Equals(Velocity other) => this.MetersPerSecond == other.MetersPerSecond;

        public int CompareTo(Velocity other) => this.MetersPerSecond.CompareTo(other.MetersPerSecond);

        public override int GetHashCode() => -1419927970 + this.MetersPerSecond.GetHashCode();
    }
}
