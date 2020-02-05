using Roy_T.AStar.V2.Grids;
using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.Benchmark
{
    public static class GridBuilder
    {
        private static readonly float[] RandomNumbers = new float[]
        {
            23, 24, 95, 72, 34, 63, 46, 43, 57, 61,
            12, 77, 30, 25, 49, 83, 54, 64, 42, 4,
            14, 43, 61, 81, 44, 51, 5, 62, 84, 60,
            42, 35, 90, 32, 7, 78, 58, 77, 67, 12,
            65, 47, 11, 66, 37, 12, 27, 61, 73, 42,
            51, 58, 27, 42, 42, 41, 43, 76, 72, 86,
            49, 74, 96, 20, 50, 13, 85, 71, 51, 48,
            13, 15, 35, 47, 87, 100, 53, 1, 9, 41,
            1, 28, 59, 15, 38, 70, 92, 41, 84, 87,
            6, 81, 80, 70, 1, 64, 94
        };

        /// <summary>
        /// Disconnects the left part of the right part of the given graph.
        /// </summary>        
        public static void DisconnectRightHalf(Grid grid)
        {
            for (var y = 0; y < grid.Rows; y++)
            {
                grid.BlockNode(grid.Columns / 2, y);
            }
        }


        /// <summary>
        /// Pseudo randomly disconnects roughly 50% of the nodes. Does not disconnect nodes 
        /// in the top left and bottom right, corners.
        /// </summary>        
        public static void DisconnectRandomNodes(Grid grid)
        {
            var z = 0;
            for (var y = 2; y < grid.Rows - 2; y++)
            {
                for (var x = 2; x < grid.Columns - 2; x++)
                {
                    var rand = RandomNumbers[z];
                    if (rand < 50)
                    {
                        grid.BlockNode(x, y);
                    }
                    z = (z + 1) % RandomNumbers.Length;
                }
            }
        }

        /// <summary>
        /// Disconnects the top left part of the bottom right part of the given graph, 
        /// except for in a single point near the center.
        /// </summary>        
        public static void DisconnectDiagonallyExceptForOneNode(Grid grid)
        {
            for (var i = grid.Rows - 1; i >= 0; i--)
            {
                if (i != (grid.Rows / 2) - 1)
                {
                    grid.BlockNode(i, i);
                }
            }
        }

        /// <summary>
        /// Makes edges in the top-left of the graph faster to traverse, while
        /// making the edges in the bottom right of the graph slower to traverse.
        /// </summary>       
        public static void SetGradientLimits(Grid grid)
        {
            var speedLimit = (grid.Rows * grid.Columns) + 1;
            for (var y = 0; y < grid.Rows; y++)
            {
                for (var x = 0; x < grid.Columns; x++)
                {
                    var node = grid.GetNode(x, y);
                    foreach (var edge in node.Incoming)
                    {
                        edge.TraversalVelocity = Velocity.FromKilometersPerHour(speedLimit);
                    }

                    speedLimit -= 1;
                }
            }
        }

        /// <summary>
        /// Pseudo randomly assigns traversal velocities in [80..100km/h] to edges.
        /// </summary>        
        public static void SetRandomTraversalVelocities(Grid grid)
        {
            var z = 0;
            for (var y = 0; y < grid.Rows; y++)
            {
                for (var x = 0; x < grid.Columns; x++)
                {
                    var node = grid.GetNode(x, y);
                    foreach (var edge in node.Incoming)
                    {
                        var speed = (RandomNumbers[z] / 100.0f * 20) + 80;
                        edge.TraversalVelocity = Velocity.FromKilometersPerHour(speed);
                        z = (z + 1) % RandomNumbers.Length;
                    }
                }
            }
        }
    }
}
