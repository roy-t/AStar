namespace Roy_T.AStar.V2.Primitives
{
    public struct Velocity
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

        public override string ToString() => $"{this.MetersPerSecond:F2} m/s";
        public override bool Equals(object obj) => obj is Velocity velocity && this.MetersPerSecond == velocity.MetersPerSecond;
        public override int GetHashCode() => -1419927970 + this.MetersPerSecond.GetHashCode();
    }
}
