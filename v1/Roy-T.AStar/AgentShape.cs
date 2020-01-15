using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoyT.AStar
{
    /// <summary>
    /// Agent shape.
    /// Shape is made of cells. Center point of agent is (0, 0). Cells should be around that.
    /// Note: agent is not rotated when traversing cells so the recommendation is to keep shape symmetrical.
    /// </summary>
    public struct AgentShape
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cells">Cells of agent</param>
        public AgentShape(params Displacement[] cells)
        {
            Cells = cells;
            TopLeft = new Position(cells.Min(d => d.X), cells.Min(d => d.Y));
            BottomRight = new Position(cells.Max(d => d.X), cells.Max(d => d.Y));
        }

        /// <summary>
        /// Top-left cell of agent
        /// </summary>
        public Position TopLeft { get; private set; }

        /// <summary>
        /// Bottom-right cell of agent
        /// </summary>
        public Position BottomRight { get; private set; }

        /// <summary>
        /// Cells of agent shape
        /// </summary>
        public Displacement[] Cells { get; private set; }
    }
}
