using System;
using System.Collections.Generic;
using System.Text;

namespace RoyT.AStar
{    
    /// <summary>
    /// Predefined options of agent shapes.
    /// </summary>
    public static class AgentShapes
    {
        /// <summary>
        /// Single dot
        /// </summary>
        public static readonly Displacement[] Dot = { new Displacement(0, 0) };

        /// <summary>
        /// Circle with radius of 1 cell
        /// </summary>
        public static readonly Displacement[] CircleR1 =
        {
                                     new Displacement(0, -1),
            new Displacement(-1, 0), new Displacement(0, 0),  new Displacement(1, 0),
                                     new Displacement(0, 1)
        };

        /// <summary>
        /// Square with width of 3 cells
        /// </summary>
        public static readonly Displacement[] SquareW3 =
        {
            new Displacement(-1, -1), new Displacement(+0, -1), new Displacement(+1, -1),
            new Displacement(-1, +0), new Displacement(+0, +0), new Displacement(+1, +0),
            new Displacement(-1, +1), new Displacement(+0, +1), new Displacement(+1, +1),
        };
    }
}
