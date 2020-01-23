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

        private readonly Grid GradientGrid;
        private readonly Grid GridWithHole;
        private readonly Grid GridWithRandomLimits;
        private readonly Grid GridWithRandomLimitsAndHoles;
        private readonly Grid GridWithUnreachableTarget;

        public AStarBenchmark()
        {
            this.GradientGrid = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.SetGradientLimits(this.GradientGrid);

            this.GridWithHole = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.DisconnectDiagonallyExceptForOneNode(this.GridWithHole);

            this.GridWithRandomLimits = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.SetRandomTraversalVelocities(this.GridWithRandomLimits);

            this.GridWithRandomLimitsAndHoles = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.SetRandomTraversalVelocities(this.GridWithRandomLimitsAndHoles);
            GridBuilder.DisconnectRandomNodes(this.GridWithRandomLimitsAndHoles);

            this.GridWithUnreachableTarget = new Grid(100, 100, 1, 1, MaxSpeed);
            GridBuilder.DisconnectRightHalf(this.GridWithUnreachableTarget);
        }

        [Benchmark]
        public void GradientGridBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour((this.GradientGrid.Rows * this.GradientGrid.Columns) + 1);
            PathFinder.FindPath(
                this.GradientGrid.GetNode(0, 0),
                this.GradientGrid.GetNode(this.GradientGrid.Columns - 1, this.GradientGrid.Rows - 1),
                maxSpeed);
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
        public void GridWithRandomLimitsBench()
        {
            PathFinder.FindPath(
                this.GridWithRandomLimits.GetNode(0, 0),
                this.GridWithRandomLimits.GetNode(this.GridWithRandomLimits.Columns - 1, this.GridWithRandomLimits.Rows - 1),
                MaxSpeed);
        }

        [Benchmark]
        public void GridWithRandomLimitsAndHolesBench()
        {
            PathFinder.FindPath(
                this.GridWithRandomLimitsAndHoles.GetNode(0, 0),
                this.GridWithRandomLimitsAndHoles.GetNode(this.GridWithRandomLimitsAndHoles.Columns - 1, this.GridWithRandomLimitsAndHoles.Rows - 1),
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


    }
}
