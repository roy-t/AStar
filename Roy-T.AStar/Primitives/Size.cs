using System;

namespace Roy_T.AStar.Primitives
{
    public struct Size : IEquatable<Size>
    {
        public Size(Distance width, Distance height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Distance Width { get; }
        public Distance Height { get; }

        public static bool operator ==(Size a, Size b)
            => a.Equals(b);

        public static bool operator !=(Size a, Size b)
            => !a.Equals(b);

        public override string ToString() => $"(width: {this.Width}, height: {this.Height})";

        public override bool Equals(object obj) => obj is Size Size && this.Equals(Size);

        public bool Equals(Size other) => this.Width == other.Width && this.Height == other.Height;

        public override int GetHashCode() => -1609761766 + this.Width.GetHashCode() + this.Height.GetHashCode();
    }
}
