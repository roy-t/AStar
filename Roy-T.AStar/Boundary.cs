using System;
using System.Collections.Generic;
using System.Text;

namespace RoyT.AStar
{
    /// <summary>
    /// Rectangular boundary area
    /// </summary>
    public struct Boundary
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }

        /// <summary>
        /// Is position inside boundary ?
        /// </summary>
        /// <param name="p">Position</param>
        /// <returns>true if position is inside boundary</returns>
        public bool IsInside(Position p)
        {
            return (p.X >= X) && (p.X < X + Width) && (p.Y >= Y) && (p.Y < Y + Heigth);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>As text</returns>
        public override string ToString() => $"Boundary: ({X}, {Y} - {Width}, {Heigth})";
    }
}
