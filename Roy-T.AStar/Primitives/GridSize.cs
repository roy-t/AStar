using System;

namespace Roy_T.AStar.Primitives
{
    public struct GridSize : IEquatable<GridSize>
    {
        public GridSize(int columns, int rows)
        {
            this.Columns = columns;
            this.Rows = rows;
        }

        public int Columns { get; }
        public int Rows { get; }

        public static bool operator ==(GridSize a, GridSize b)
            => a.Equals(b);

        public static bool operator !=(GridSize a, GridSize b)
            => !a.Equals(b);

        public override string ToString() => $"(columns: {this.Columns}, rows: {this.Rows})";

        public override bool Equals(object obj) => obj is GridSize GridSize && this.Equals(GridSize);

        public bool Equals(GridSize other) => this.Columns == other.Columns && this.Rows == other.Rows;

        public override int GetHashCode() => -1609761766 + this.Columns.GetHashCode() + this.Rows.GetHashCode();
    }
}
