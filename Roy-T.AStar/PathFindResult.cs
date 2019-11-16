using System;
using System.Collections.Generic;
using System.Text;

namespace RoyT.AStar
{
    /// <summary>
    /// Path finding result
    /// </summary>
    public enum PathFindResult
    {
        /// <summary>
        /// Complete path found from start to end
        /// </summary>
        PathFound,

        /// <summary>
        /// Can't reach the end, but path to closest reachable point is returned
        /// </summary>
        PartialPathFound,

        /// <summary>
        /// Happens when agent has no room for movement and can't even get partial path to end
        /// </summary>
        Stuck,

        /// <summary>
        /// Already at the end point
        /// </summary>
        AlreadyAtTheEnd,

        /// <summary>
        /// Start point or some part of agent is outside of map borders
        /// </summary>
        StartOutsideBoundaries,

        /// <summary>
        /// End point is outside of map borders
        /// </summary>
        EndOutsideBoundaries,
    }
}
