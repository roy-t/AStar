using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Serialization
{
    public static class GridSerializer
    {
        public static string SerializeGrid(Grid grid)
        {
            return JsonConvert.SerializeObject(grid.ToDto());
        }

        public static Grid DeSerializeGrid(string gridString)
        {
            var gridDto = JsonConvert.DeserializeObject<GridDto>(gridString);
            Node[,] nodes = new Node[gridDto.Nodes.GetLength(0), gridDto.Nodes.GetLength(1)];
            for (int i = 0; i < gridDto.Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < gridDto.Nodes.GetLength(1); j++)
                {
                    var nodeDto = gridDto.Nodes[i, j];
                    var node = new Node(nodeDto.Position.FromDto());
                    nodes[i, j] = node;
                }
            }

            for (int i = 0; i < gridDto.Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < gridDto.Nodes.GetLength(1); j++)
                {
                    var nodeDto = gridDto.Nodes[i, j];
                    var node = nodes[i, j];
                    foreach (var outGoingEdge in nodeDto.OutGoingEdges)
                    {
                        var toNode = nodes[outGoingEdge.End.X, outGoingEdge.End.Y];
                        node.Connect(toNode, outGoingEdge.TraversalVelocity.FromDto());
                    }
                }
            }

            return Grid.CreateGridFrom2DArrayOfNodes(nodes);
        }

        public static GridDto ToDto(this Grid grid)
        {
            var nodeToGridPositionDict = new Dictionary<INode, GridPosition>();
            NodeDto[,] nodes = new NodeDto[grid.Columns, grid.Rows];
            for (int i = 0; i < grid.Columns; i++)
            {
                for (int j = 0; j < grid.Rows; j++)
                {
                    var gridPosition = new GridPosition(i, j);
                    nodeToGridPositionDict[grid.GetNode(gridPosition)] = gridPosition;
                }
            }

            for (int i = 0; i < grid.Columns; i++)
            {
                for (int j = 0; j < grid.Rows; j++)
                {
                    nodes[i, j] = grid.GetNode(new GridPosition(i, j)).ToDto(nodeToGridPositionDict);
                }
            }

            return new GridDto
            {
                Nodes = nodes
            };
        }

        public static NodeDto ToDto(this INode node, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return new NodeDto
            {
                Position = node.Position.ToDto(),
                GridPosition = nodeToGridPositionDict[node].ToDto(),
                OutGoingEdges = node.Outgoing.ToDto(nodeToGridPositionDict),
                IncomingEdges = node.Incoming.ToDto(nodeToGridPositionDict),
            };
        }

        public static List<EdgeDto> ToDto(this IList<IEdge> edge, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return edge.Select(e => e.ToDto(nodeToGridPositionDict)).ToList();
        }

        public static EdgeDto ToDto(this IEdge edge, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return new EdgeDto
            {
                TraversalVelocity = edge.TraversalVelocity.ToDto(),
                Start = nodeToGridPositionDict[edge.Start].ToDto(),
                End = nodeToGridPositionDict[edge.End].ToDto()
            };
        }

        public static VelocityDto ToDto(this Velocity velocity)
        {
            return new VelocityDto
            {
                MetersPerSecond = velocity.MetersPerSecond
            };
        }

        public static Velocity FromDto(this VelocityDto velocity)
        {
            return Velocity.FromMetersPerSecond(velocity.MetersPerSecond);
        }

        public static PositionDto ToDto(this Position position)
        {
            return new PositionDto
            {
                X = position.X,
                Y = position.Y
            };
        }

        public static Position FromDto(this PositionDto position)
        {
            return new Position(position.X, position.Y);
        }

        public static GridPositionDto ToDto(this GridPosition position)
        {
            return new GridPositionDto
            {
                X = position.X,
                Y = position.Y
            };
        }

        public static GridPosition FromDto(this GridPositionDto position)
        {
            return new GridPosition(position.X, position.Y);
        }
    }
}