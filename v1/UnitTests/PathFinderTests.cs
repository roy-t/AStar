using RoyT.AStar;
using Xunit;

namespace UnitTests
{
    public class PathFinderTests
    {
        [Fact]
        public void NonExistingPathShouldReturnEmtpyArray()
        {
            var grid = GridCatalog.GridWithEnclosedCenterTile();
            var path = grid.GetPath(new Position(0, 0), new Position(4, 4));

            Assert.NotNull(path);
            Assert.Empty(path);
        }

        [Fact]
        public void ShouldFindPathInUnobstructedGrid()
        {
            var grid = GridCatalog.UnobstructedGrid();
            var path = grid.GetPath(new Position(0, 0), new Position(8, 8));

            Assert.NotNull(path);
            Assert.Equal(9, path.Length);
        }

        [Fact]
        public void ShouldFindPathWithNonZeroStartingPosition()
        {
            var grid = GridCatalog.UnobstructedGrid();
            var path = grid.GetPath(new Position(3, 3), new Position(5, 3));

            Assert.NotNull(path);
            Assert.Equal(3, path.Length);
        }

        [Fact]
        public void ShouldRespectBlockedCells()
        {
            var grid = GridCatalog.GridWithBlockedCenterTile();
            var path = grid.GetPath(new Position(0, 0), new Position(8, 8));

            Assert.NotNull(path);
            Assert.DoesNotContain(new Position(4, 4), path);
            Assert.Equal(10, path.Length);
        }

        [Fact]
        public void ShouldRespectCellCost()
        {
            var grid = GridCatalog.GridWithHighCostCenterTile();
            var path = grid.GetPath(new Position(0, 0), new Position(8, 8));

            Assert.NotNull(path);
            Assert.DoesNotContain(new Position(4, 4), path);
            Assert.Equal(10, path.Length);
        }

        [Fact]
        public void ShouldRespectIterationLimit()
        {
            var grid = GridCatalog.UnobstructedGrid();
            var path = grid.GetPath(new Position(0, 0), new Position(8, 8), MovementPatterns.Full, AgentShapes.Dot, 8);

            Assert.NotNull(path);
            Assert.Empty(path);
        }

        [Fact]
        public void ShouldRespectMovementPattern()
        {
            var grid = GridCatalog.GridWithObstructedDiagonals();
            var path = grid.GetPath(new Position(0, 0), new Position(8, 8), MovementPatterns.LateralOnly, AgentShapes.Dot);

            Assert.NotNull(path);
            Assert.Empty(path);
        }
    }
}
