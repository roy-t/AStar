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
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        /// <summary>
        /// Is position inside boundary ?
        /// </summary>
        /// <param name="p">Position</param>
        /// <returns>true if position is inside boundary</returns>
        public bool IsInside(Position p)
        {
            return (p.X >= X1) && (p.X <= X2) && (p.Y >= Y1) && (p.Y <= Y2);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>As text</returns>
        public override string ToString() => $"Boundary: ({X1}, {Y1} - {X2}, {Y2})";
    }
}
