using System.Collections.Generic;

namespace Roy_T.AStar.Serialization
{
    public class NodeDto
    {
        public PositionDto Position { get; set; }
        public GridPositionDto GridPosition { get; set; }
        public List<EdgeDto> IncomingEdges { get; set; }
        public List<EdgeDto> OutGoingEdges { get; set; }
    }
}
