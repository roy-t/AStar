using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using Roy_T.AStar.Serialization;

namespace Roy_T.AStar.Tests
{
    public sealed class GridSerializationTests
    {
        [Test]
        public void GraphIsEqualAfterSerializeAndDeSerialize()
        {
            var grid = Grid.CreateGridWithLateralConnections(new GridSize(2, 4),
                new Size(Distance.FromMeters(2.0f), Distance.FromMeters(1.0f)), Velocity.FromKilometersPerHour(3));

            var stringGrid = GridSerializer.SerializeGrid(grid);
            var deserializedGrid = GridSerializer.DeSerializeGrid(stringGrid);

            Assert.AreEqual(grid.Rows, deserializedGrid.Rows);
            Assert.AreEqual(grid.Columns, deserializedGrid.Columns);
            for (int i = 0; i < grid.Columns; i++)
            {
                for (int j = 0; j < grid.Rows; j++)
                {
                    var gridPosition = new GridPosition(i, j);
                    var originalNode = grid.GetNode(gridPosition);
                    var deserializedNode = grid.GetNode(gridPosition);
                    Assert.AreEqual(originalNode.Position, deserializedNode.Position);
                    Assert.AreEqual(originalNode.Outgoing.Count, deserializedNode.Outgoing.Count);
                    Assert.AreEqual(originalNode.Incoming.Count, deserializedNode.Incoming.Count);
                    foreach (var edge in originalNode.Outgoing)
                    {
                        var matchingEdge = deserializedNode.Outgoing.Single(o =>
                            o.Start.Position.Equals(edge.Start.Position) && o.End.Position.Equals(edge.End.Position));
                        Assert.AreEqual(edge.Distance, matchingEdge.Distance);
                        Assert.AreEqual(edge.TraversalDuration, matchingEdge.TraversalDuration);
                        Assert.AreEqual(edge.TraversalVelocity, matchingEdge.TraversalVelocity);
                    }

                    foreach (var edge in originalNode.Incoming)
                    {
                        var matchingEdge = deserializedNode.Incoming.Single(o =>
                            o.Start.Position.Equals(edge.Start.Position) && o.End.Position.Equals(edge.End.Position));
                        Assert.AreEqual(edge.Distance, matchingEdge.Distance);
                        Assert.AreEqual(edge.TraversalDuration, matchingEdge.TraversalDuration);
                        Assert.AreEqual(edge.TraversalVelocity, matchingEdge.TraversalVelocity);
                    }
                }
            }
        }
    }
}
