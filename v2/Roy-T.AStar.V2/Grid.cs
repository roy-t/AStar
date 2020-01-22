using System;
using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    public class Grid
    {
        private readonly INode[,] Nodes;

        public Grid(int columns, int rows, float xDistance, float yDistance, Velocity defaultSpeed, Connections connections = Connections.LateralAndDiagonal)
        {
            if (columns < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(columns), $"Argument {nameof(columns)} is {columns} but should be >= 1");
            }

            if (rows < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rows), $"Argument {nameof(rows)} is {rows} but should be >= 1");
            }

            if (defaultSpeed.MetersPerSecond <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(defaultSpeed), $"Argument {nameof(defaultSpeed)} is {defaultSpeed} but should be > 0.0 m/s");
            }

            if (xDistance <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(xDistance), $"Argument {nameof(xDistance)} is {xDistance} but should be > 0");
            }

            if (yDistance <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(yDistance), $"Argument {nameof(yDistance)} is {yDistance} but should be > 0");
            }

            this.Columns = columns;
            this.Rows = rows;

            this.Nodes = new INode[columns, rows];

            this.CreateNodes(xDistance, yDistance);

            switch (connections)
            {
                case Connections.Lateral:
                    this.CreateLateralConnections(defaultSpeed);
                    break;
                case Connections.Diagonal:
                    this.CreateDiagonalConnections(defaultSpeed);
                    break;
                default:
                    this.CreateLateralConnections(defaultSpeed);
                    this.CreateDiagonalConnections(defaultSpeed);
                    break;
            }
        }

        public INode GetNode(int x, int y) => this.Nodes[x, y];

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

        public void BlockNode(int x, int y)
        {
            var node = this.Nodes[x, y];

            for (var i = 0; i < node.Incoming.Count; i++)
            {
                var edge = node.Incoming[i];
                var neighbour = edge.GetOppositeNode(node);
                neighbour.Outgoing.Remove(edge);
            }

            node.Incoming.Clear();

            for (var i = 0; i < node.Outgoing.Count; i++)
            {
                var edge = node.Outgoing[i];
                var neighbour = edge.GetOppositeNode(node);
                neighbour.Incoming.Remove(edge);
            }

            node.Outgoing.Clear();
        }

        private void CreateNodes(float xDistance, float yDistance)
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    this.Nodes[x, y] = new Node(x * xDistance, y * yDistance);
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
                    if (x > 0)
                    {
                        var westNode = this.Nodes[x - 1, y];
                        node.Connect(westNode, defaultSpeed);
                        westNode.Connect(node, defaultSpeed);
                    }

                    if (y > 0)
                    {
                        var northNode = this.Nodes[x, y - 1];
                        node.Connect(northNode, defaultSpeed);
                        northNode.Connect(node, defaultSpeed);
                    }

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
                    if (x > 0 && y > 0)
                    {
                        var northWestNode = this.Nodes[x - 1, y - 1];
                        node.Connect(northWestNode, defaultSpeed);
                        northWestNode.Connect(node, defaultSpeed);
                    }

                    if (x < this.Columns - 1 && y > 0)
                    {
                        var northEastNode = this.Nodes[x + 1, y - 1];
                        node.Connect(northEastNode, defaultSpeed);
                        northEastNode.Connect(node, defaultSpeed);
                    }

                    if (x < this.Columns - 1 && y < this.Rows - 1)
                    {
                        var southEastNode = this.Nodes[x + 1, y + 1];
                        node.Connect(southEastNode, defaultSpeed);
                        southEastNode.Connect(node, defaultSpeed);
                    }

                    if (x > 0 && y < this.Rows - 1)
                    {
                        var southWestNode = this.Nodes[x, y + 1];
                        node.Connect(southWestNode, defaultSpeed);
                        southWestNode.Connect(node, defaultSpeed);
                    }
                }
            }
        }

        public int Columns { get; }
        public int Rows { get; }
    }
}
