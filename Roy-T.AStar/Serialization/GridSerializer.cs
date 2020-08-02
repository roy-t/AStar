using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Roy_T.AStar.Serialization
{
    public static class GridSerializer
    {
        public static string SerializeGrid(Grid grid)
        {
            var gridDto = grid.ToDto();
            XmlSerializer xmlSerializer = new XmlSerializer(gridDto.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, gridDto);
                return textWriter.ToString();
            }
        }

        public static Grid DeSerializeGrid(string gridString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GridDto));
            using (StringReader textReader = new StringReader(gridString))
            {
                var gridDto = (GridDto)xmlSerializer.Deserialize(textReader);
                Node[,] nodes = new Node[gridDto.Nodes.Length, gridDto.Nodes[0].Length];
                for (int i = 0; i < gridDto.Nodes.Length; i++)
                {
                    for (int j = 0; j < gridDto.Nodes[0].Length; j++)
                    {
                        var nodeDto = gridDto.Nodes[i][j];
                        var node = new Node(nodeDto.Position.FromDto());
                        nodes[i, j] = node;
                    }
                }

                for (int i = 0; i < gridDto.Nodes.Length; i++)
                {
                    for (int j = 0; j < gridDto.Nodes[0].Length; j++)
                    {
                        var nodeDto = gridDto.Nodes[i][j];
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
        }

        private static GridDto ToDto(this Grid grid)
        {
            var nodeToGridPositionDict = new Dictionary<INode, GridPosition>();
            NodeDto[][] nodes = new NodeDto[grid.Columns][];
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
                nodes[i] = new NodeDto[grid.Rows];
                for (int j = 0; j < grid.Rows; j++)
                {
                    nodes[i][j] = grid.GetNode(new GridPosition(i, j)).ToDto(nodeToGridPositionDict);
                }
            }

            return new GridDto
            {
                Nodes = nodes
            };
        }

        private static NodeDto ToDto(this INode node, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return new NodeDto
            {
                Position = node.Position.ToDto(),
                GridPosition = nodeToGridPositionDict[node].ToDto(),
                OutGoingEdges = node.Outgoing.ToDto(nodeToGridPositionDict),
                IncomingEdges = node.Incoming.ToDto(nodeToGridPositionDict),
            };
        }

        private static List<EdgeDto> ToDto(this IList<IEdge> edge, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return edge.Select(e => e.ToDto(nodeToGridPositionDict)).ToList();
        }

        private static EdgeDto ToDto(this IEdge edge, Dictionary<INode, GridPosition> nodeToGridPositionDict)
        {
            return new EdgeDto
            {
                TraversalVelocity = edge.TraversalVelocity.ToDto(),
                Start = nodeToGridPositionDict[edge.Start].ToDto(),
                End = nodeToGridPositionDict[edge.End].ToDto()
            };
        }

        private static VelocityDto ToDto(this Velocity velocity)
        {
            return new VelocityDto
            {
                MetersPerSecond = velocity.MetersPerSecond
            };
        }

        private static Velocity FromDto(this VelocityDto velocity)
        {
            return Velocity.FromMetersPerSecond(velocity.MetersPerSecond);
        }

        private static PositionDto ToDto(this Position position)
        {
            return new PositionDto
            {
                X = position.X,
                Y = position.Y
            };
        }

        private static Position FromDto(this PositionDto position)
        {
            return new Position(position.X, position.Y);
        }

        private static GridPositionDto ToDto(this GridPosition position)
        {
            return new GridPositionDto
            {
                X = position.X,
                Y = position.Y
            };
        }

        private static GridPosition FromDto(this GridPositionDto position)
        {
            return new GridPosition(position.X, position.Y);
        }
    }
}