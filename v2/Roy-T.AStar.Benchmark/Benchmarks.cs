using BenchmarkDotNet.Attributes;
using Roy_T.AStar.V2.Grids;
using Roy_T.AStar.V2.Paths;
using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.Benchmark
{
    /// <summary>
    /// For more thorough explanation, and benchmark history, see BenchmarkHistory.md
    /// </summary>
    public class AStarBenchmark
    {
        private static readonly Velocity MaxSpeed = Velocity.FromKilometersPerHour(100);

        private readonly PathFinder PathFinder;

        private readonly Grid Grid;
        private readonly Grid GridWithGradient;
        private readonly Grid GridWithHole;
        private readonly Grid GridWithRandomLimits;
        private readonly Grid GridWithRandomHoles;
        private readonly Grid GridWithUnreachableTarget;

        public AStarBenchmark()
        {
            this.PathFinder = new PathFinder();

            var gridSize = new GridSize(100, 100);
            var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));

            this.Grid = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);

            this.GridWithGradient = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.SetGradientLimits(this.GridWithGradient);

            this.GridWithHole = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectDiagonallyExceptForOneNode(this.GridWithHole);

            this.GridWithRandomLimits = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.SetRandomTraversalVelocities(this.GridWithRandomLimits);

            this.GridWithRandomHoles = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectRandomNodes(this.GridWithRandomHoles);

            this.GridWithUnreachableTarget = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectRightHalf(this.GridWithUnreachableTarget);
        }

        [Benchmark]
        public void GridBench()
        {
            this.PathFinder.FindPath(
                this.Grid.GetNode(GridPosition.Zero),
                this.Grid.GetNode(new GridPosition(this.Grid.Columns - 1, this.Grid.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithHoleBench()
        {
            this.PathFinder.FindPath(
                this.GridWithHole.GetNode(GridPosition.Zero),
                this.GridWithHole.GetNode(new GridPosition(this.GridWithHole.Columns - 1, this.GridWithHole.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithRandomHolesBench()
        {
            this.PathFinder.FindPath(
                this.GridWithRandomHoles.GetNode(GridPosition.Zero),
                this.GridWithRandomHoles.GetNode(new GridPosition(this.GridWithRandomHoles.Columns - 1, this.GridWithRandomHoles.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithRandomLimitsBench()
        {
            this.PathFinder.FindPath(
                this.GridWithRandomLimits.GetNode(GridPosition.Zero),
                this.GridWithRandomLimits.GetNode(new GridPosition(this.GridWithRandomLimits.Columns - 1, this.GridWithRandomLimits.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithUnreachableTargetBench()
        {
            this.PathFinder.FindPath(
                this.GridWithUnreachableTarget.GetNode(GridPosition.Zero),
                this.GridWithUnreachableTarget.GetNode(new GridPosition(this.GridWithUnreachableTarget.Columns - 1, this.GridWithUnreachableTarget.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithGradientBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour((this.GridWithGradient.Rows * this.GridWithGradient.Columns) + 1);
            this.PathFinder.FindPath(
                this.GridWithGradient.GetNode(GridPosition.Zero),
                this.GridWithGradient.GetNode(new GridPosition(this.GridWithGradient.Columns - 1, this.GridWithGradient.Rows - 1)),
                maxSpeed);
        }
    }
}
