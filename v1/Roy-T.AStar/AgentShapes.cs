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
        /// Single dot (1 cell)
        /// </summary>
        public static readonly AgentShape Dot = new AgentShape(new Displacement(0, 0));

        /// <summary>
        /// Approximate circle with radius of 1.5 cells (diameter 3 cells)
        /// </summary>
        public static readonly AgentShape CircleR1 = new AgentShape(
                                      new Displacement(+0, -1),
            new Displacement(-1, +0), new Displacement(+0, +0), new Displacement(+1, +0),
                                      new Displacement(+0, +1)
        );

        /// <summary>
        /// Approximate circle with radius of 2.5 cells (diameter 5 cells)
        /// </summary>
        public static readonly AgentShape CircleR2 = new AgentShape(
                                      new Displacement(-1, -2), new Displacement(+0, -2), new Displacement(+1, -2),
            new Displacement(-2, -1), new Displacement(-1, -1), new Displacement(+0, -1), new Displacement(+1, -1), new Displacement(+2, -1),
            new Displacement(-2, +0), new Displacement(-1, +0), new Displacement(+0, +0), new Displacement(+1, +0), new Displacement(+2, +0),
            new Displacement(-2, +1), new Displacement(-1, +1), new Displacement(+0, +1), new Displacement(+1, +1), new Displacement(+2, +1),
                                      new Displacement(-1, +2), new Displacement(+0, +2), new Displacement(+1, +2)
        );

        /// <summary>
        /// Square with width of 3 cells
        /// </summary>
        public static readonly AgentShape SquareW3 = new AgentShape(
            new Displacement(-1, -1), new Displacement(+0, -1), new Displacement(+1, -1),
            new Displacement(-1, +0), new Displacement(+0, +0), new Displacement(+1, +0),
            new Displacement(-1, +1), new Displacement(+0, +1), new Displacement(+1, +1)
        );

        /// <summary>
        /// Square with width of 5 cells
        /// </summary>
        public static readonly AgentShape SquareW5 = new AgentShape(
            new Displacement(-2, -2), new Displacement(-1, -2), new Displacement(+0, -2), new Displacement(+1, -2), new Displacement(+2, -2),
            new Displacement(-2, -1), new Displacement(-1, -1), new Displacement(+0, -1), new Displacement(+1, -1), new Displacement(+2, -1),
            new Displacement(-2, +0), new Displacement(-1, +0), new Displacement(+0, +0), new Displacement(+1, +0), new Displacement(+2, +0),
            new Displacement(-2, +1), new Displacement(-1, +1), new Displacement(+0, +1), new Displacement(+1, +1), new Displacement(+2, +1),
            new Displacement(-2, +2), new Displacement(-1, +2), new Displacement(+0, +2), new Displacement(+1, +2), new Displacement(+2, +2)
        );
    }
}
