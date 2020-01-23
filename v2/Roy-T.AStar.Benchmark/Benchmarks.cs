using BenchmarkDotNet.Attributes;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Benchmark
{
    /// <summary>
    /// For more thorough explanation, and benchmark history, see BenchmarkHistory.md
    /// </summary>
    public class AStarBenchmark
    {
        private static readonly Velocity MaxSpeed = Velocity.FromKilometersPerHour(100);

        private readonly Grid Grid;
        private readonly Grid GridWithGradient;
        private readonly Grid GridWithHole;
        private readonly Grid GridWithRandomLimits;
        private readonly Grid GridWithRandomHoles;
        private readonly Grid GridWithUnreachableTarget;

        public AStarBenchmark()
        {
            this.Grid = new Grid(100, 100, 1, 1, MaxSpeed);

            this.GridWithGradient = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.SetGradientLimits(this.GridWithGradient);

            this.GridWithHole = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.DisconnectDiagonallyExceptForOneNode(this.GridWithHole);

            this.GridWithRandomLimits = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.SetRandomTraversalVelocities(this.GridWithRandomLimits);

            this.GridWithRandomHoles = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.DisconnectRandomNodes(this.GridWithRandomHoles);

            this.GridWithUnreachableTarget = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.DisconnectRightHalf(this.GridWithUnreachableTarget);
        }

        [Benchmark]
        public void GridBench()
        {
            PathFinder.FindPath(
                this.Grid.GetNode(0, 0),
                this.Grid.GetNode(this.Grid.Columns - 1, this.Grid.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithHoleBench()
        {
            PathFinder.FindPath(
                this.GridWithHole.GetNode(0, 0),
                this.GridWithHole.GetNode(this.GridWithHole.Columns - 1, this.GridWithHole.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithRandomHolesBench()
        {
            PathFinder.FindPath(
                this.GridWithRandomHoles.GetNode(0, 0),
                this.GridWithRandomHoles.GetNode(this.GridWithRandomHoles.Columns - 1, this.GridWithRandomHoles.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithRandomLimitsBench()
        {
            PathFinder.FindPath(
                this.GridWithRandomLimits.GetNode(0, 0),
                this.GridWithRandomLimits.GetNode(this.GridWithRandomLimits.Columns - 1, this.GridWithRandomLimits.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithUnreachableTargetBench()
        {
            PathFinder.FindPath(
                this.GridWithUnreachableTarget.GetNode(0, 0),
                this.GridWithUnreachableTarget.GetNode(this.GridWithUnreachableTarget.Columns - 1, this.GridWithUnreachableTarget.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithGradientBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour((this.GridWithGradient.Rows * this.GridWithGradient.Columns) + 1);
            PathFinder.FindPath(
                this.GridWithGradient.GetNode(0, 0),
                this.GridWithGradient.GetNode(this.GridWithGradient.Columns - 1, this.GridWithGradient.Rows - 1),
                maxSpeed);
        }
    }
}
