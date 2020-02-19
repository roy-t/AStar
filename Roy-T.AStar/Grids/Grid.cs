using System;
using System.Collections.Generic;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Grids
{
    public sealed class Grid
    {
        private readonly Node[,] Nodes;

        public static Grid CreateGridWithLateralConnections(GridSize gridSize, Size cellSize, Velocity traversalVelocity)
        {
            CheckArguments(gridSize, cellSize, traversalVelocity);

            var grid = new Grid(gridSize, cellSize);

            grid.CreateLateralConnections(traversalVelocity);

            return grid;
        }

        public static Grid CreateGridWithDiagonalConnections(GridSize gridSize, Size cellSize, Velocity traversalVelocity)
        {
            CheckArguments(gridSize, cellSize, traversalVelocity);

            var grid = new Grid(gridSize, cellSize);

            grid.CreateDiagonalConnections(traversalVelocity);

            return grid;
        }

        public static Grid CreateGridWithLateralAndDiagonalConnections(GridSize gridSize, Size cellSize, Velocity traversalVelocity)
        {
            CheckArguments(gridSize, cellSize, traversalVelocity);

            var grid = new Grid(gridSize, cellSize);

            grid.CreateDiagonalConnections(traversalVelocity);
            grid.CreateLateralConnections(traversalVelocity);

            return grid;
        }

        private static void CheckArguments(GridSize gridSize, Size cellSize, Velocity defaultSpeed)
        {
            if (gridSize.Columns < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(gridSize), $"Argument {nameof(gridSize.Columns)} is {gridSize.Columns} but should be >= 1");
            }

            if (gridSize.Rows < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(gridSize), $"Argument {nameof(gridSize.Rows)} is {gridSize.Rows} but should be >= 1");
            }

            if (cellSize.Width <= Distance.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cellSize), $"Argument {nameof(cellSize.Width)} is {cellSize.Width} but should be > {Distance.Zero}");
            }

            if (cellSize.Height <= Distance.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cellSize), $"Argument {nameof(cellSize.Height)} is {cellSize.Height} but should be > {Distance.Zero}");
            }

            if (defaultSpeed.MetersPerSecond <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(defaultSpeed), $"Argument {nameof(defaultSpeed)} is {defaultSpeed} but should be > 0.0 m/s");
            }
        }

        private Grid(GridSize gridSize, Size cellSize)
        {
            this.GridSize = gridSize;
            this.Nodes = new Node[gridSize.Columns, gridSize.Rows];

            this.CreateNodes(cellSize);
        }

        private void CreateNodes(Size cellSize)
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    this.Nodes[x, y] = new Node(Position.FromOffset(cellSize.Width * x, cellSize.Height * y));
                }
            }
        }

        private void CreateLateralConnections(Velocity defaultSpeed)
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    var node = this.Nodes[x, y];

                    if (x < this.Columns - 1)
                    {
                        var eastNode = this.Nodes[x + 1, y];
                        node.Connect(eastNode, defaultSpeed);
                        eastNode.Connect(node, defaultSpeed);
                    }

                    if (y < this.Rows - 1)
                    {
                        var southNode = this.Nodes[x, y + 1];
                        node.Connect(southNode, defaultSpeed);
                        southNode.Connect(node, defaultSpeed);
                    }
                }
            }
        }

        private void CreateDiagonalConnections(Velocity defaultSpeed)
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    var node = this.Nodes[x, y];

                    if (x < this.Columns - 1 && y < this.Rows - 1)
                    {
                        var southEastNode = this.Nodes[x + 1, y + 1];
                        node.Connect(southEastNode, defaultSpeed);
                        southEastNode.Connect(node, defaultSpeed);
                    }

                    if (x > 0 && y < this.Rows - 1)
                    {
                        var southWestNode = this.Nodes[x - 1, y + 1];
                        node.Connect(southWestNode, defaultSpeed);
                        southWestNode.Connect(node, defaultSpeed);
                    }
                }
            }
        }

        public GridSize GridSize { get; }

        public int Columns => this.GridSize.Columns;

        public int Rows => this.GridSize.Rows;

        public INode GetNode(GridPosition position) => this.Nodes[position.X, position.Y];

        public IReadOnlyList<INode> GetAllNodes()
        {
            var list = new List<INode>(this.Columns * this.Rows);

            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    list.Add(this.Nodes[x, y]);
                }
            }

            return list;
        }

        public void DisconnectNode(GridPosition position)
        {
            var node = this.Nodes[position.X, position.Y];

            foreach (var outgoingEdge in node.Outgoing)
            {
                var opposite = outgoingEdge.End;
                opposite.Incoming.Remove(outgoingEdge);
            }

            node.Outgoing.Clear();

            foreach (var incomingEdge in node.Incoming)
            {
                var opposite = incomingEdge.Start;
                opposite.Outgoing.Remove(incomingEdge);
            }

            node.Incoming.Clear();
        }

        public void RemoveDiagonalConnectionsIntersectingWithNode(GridPosition position)
        {
            var left = new GridPosition(position.X - 1, position.Y);
            var top = new GridPosition(position.X, position.Y - 1);
            var right = new GridPosition(position.X + 1, position.Y);
            var bottom = new GridPosition(position.X, position.Y + 1);

            if (this.IsInsideGrid(left) && this.IsInsideGrid(top))
            {
                this.RemoveEdge(left, top);
                this.RemoveEdge(top, left);
            }

            if (this.IsInsideGrid(top) && this.IsInsideGrid(right))
            {
                this.RemoveEdge(top, right);
                this.RemoveEdge(right, top);
            }

            if (this.IsInsideGrid(right) && this.IsInsideGrid(bottom))
            {
                this.RemoveEdge(right, bottom);
                this.RemoveEdge(bottom, right);
            }

            if (this.IsInsideGrid(bottom) && this.IsInsideGrid(left))
            {
                this.RemoveEdge(bottom, left);
                this.RemoveEdge(left, bottom);
            }
        }

        public void RemoveEdge(GridPosition from, GridPosition to)
        {
            var fromNode = this.Nodes[from.X, from.Y];
            var toNode = this.Nodes[to.X, to.Y];

            fromNode.Disconnect(toNode);
        }

        private bool IsInsideGrid(GridPosition position) => position.X >= 0 && position.X < this.Columns && position.Y >= 0 && position.Y < this.Rows;
    }
}
