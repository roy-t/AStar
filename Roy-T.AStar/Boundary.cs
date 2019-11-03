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
        /// Consructor
        /// </summary>
        /// <param name="x1">Top left X coordinate</param>
        /// <param name="y1">Top left Y coordinate</param>
        /// <param name="x2">Bottom right X coordinate</param>
        /// <param name="y2">Bottom right Y coordinate</param>
        public Boundary(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Boundary from rectangle
        /// </summary>
        /// <param name="x">Top left X coordinate</param>
        /// <param name="y">Top left Y coordinate</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle heigth</param>
        /// <returns>Boundary made from rectangle</returns>
        public static Boundary FromRectangle(int x, int y, int width, int height)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("Width can't be less than 1");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("Height can't be less than 1");
            }

            return new Boundary(x, y, x + width - 1, y + height - 1);
        }

        /// <summary>
        /// Boundary from grid size and agent shape
        /// </summary>
        /// <param name="width">Grid width</param>
        /// <param name="height">Grid height</param>
        /// <returns>Boundary which ensured any part of agent can't get outside of grid</returns>
        public static Boundary FromSizeAndShape(int width, int height, AgentShape shape)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("Width can't be less than 1");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("Height can't be less than 1");
            }

            return new Boundary(
                -shape.TopLeft.X, -shape.TopLeft.Y,
                width - shape.BottomRight.X - 1, height - shape.BottomRight.Y - 1);
        }

        /// <summary>
        /// Is position inside boundary ?
        /// </summary>
        /// <param name="p">Position</param>
        /// <returns>true if position is inside boundary</returns>
        public bool IsInside(Position p)
        {
            return (p.X >= X1) && (p.X <= X2) && (p.Y >= Y1) && (p.Y <= X2);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>As text</returns>
        public override string ToString() => $"Boundary: ({X1}, {Y1} - {X2}, {Y2})";
    }
}
