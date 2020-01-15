using System;

namespace Roy_T.AStar.V2
{
    public class Grid
    {
        private readonly INode[,] Nodes;

        public Grid(int width, int height, float defaultCost = 1.0f, Connections connections = Connections.LateralAndDiagonal)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(width), $"Argument {nameof(width)} is {width} but should be >= 1");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(height), $"Argument {nameof(height)} is {height} but should be >= 1");
            }

            if (defaultCost < 1.0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(defaultCost), $"Argument {nameof(defaultCost)} is {defaultCost} but should be >= 1.0f");
            }

            this.Width = width;
            this.Height = height;

            this.Nodes = new INode[width, height];

            this.CreateNodes();

            switch (connections)
            {
                case Connections.Lateral:
                    this.CreateLateralConnections(defaultCost);
                    break;
                case Connections.Diagonal:
                    this.CreateDiagonalConnections(defaultCost);
                    break;
                default:
                    this.CreateLateralConnections(defaultCost);
                    this.CreateDiagonalConnections(defaultCost);
                    break;
            }
        }

        public INode GetNode(int x, int y) => this.Nodes[x, y];

        private void CreateNodes()
        {
            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    this.Nodes[x, y] = new Node(x, y);
                }
            }
        }

        private void CreateLateralConnections(float cost)
        {
            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    var node = this.Nodes[x, y];
                    if (x > 0)
                    {
                        var westNode = this.Nodes[x - 1, y];
                        node.Connect(westNode, cost);
                        westNode.Connect(node, cost);
                    }

                    if (y > 0)
                    {
                        var northNode = this.Nodes[x, y - 1];
                        node.Connect(northNode, cost);
                        northNode.Connect(node, cost);
                    }

                    if (x < this.Width - 1)
                    {
                        var eastNode = this.Nodes[x + 1, y];
                        node.Connect(eastNode, cost);
                        eastNode.Connect(node, cost);
                    }

                    if (y < this.Height - 1)
                    {
                        var southNode = this.Nodes[x, y + 1];
                        node.Connect(southNode, cost);
                        southNode.Connect(node, cost);
                    }
                }
            }
        }

        private void CreateDiagonalConnections(float cost)
        {
            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    var node = this.Nodes[x, y];
                    if (x > 0 && y > 0)
                    {
                        var northWestNode = this.Nodes[x - 1, y - 1];
                        node.Connect(northWestNode, cost);
                        northWestNode.Connect(node, cost);
                    }

                    if (x < this.Width - 1 && y > 0)
                    {
                        var northEastNode = this.Nodes[x + 1, y - 1];
                        node.Connect(northEastNode, cost);
                        northEastNode.Connect(node, cost);
                    }

                    if (x < this.Width - 1 && y < this.Height - 1)
                    {
                        var southEastNode = this.Nodes[x + 1, y + 1];
                        node.Connect(southEastNode, cost);
                        southEastNode.Connect(node, cost);
                    }

                    if (x > 0 && y < this.Height - 1)
                    {
                        var southWestNode = this.Nodes[x, y + 1];
                        node.Connect(southWestNode, cost);
                        southWestNode.Connect(node, cost);
                    }
                }
            }
        }

        public int Width { get; }
        public int Height { get; }
    }
}
