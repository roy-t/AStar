using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Viewer")]

namespace RoyT.AStar
{
    
    internal static partial class PathFinder
    {
        internal static List<Step> StepList { get; } = new List<Step>(0);

        [Conditional("DEBUG")]
        private static void MessageCurrent(SearchNode node)
        {
            var path = new List<Position>();
            var current = node;
            do
            {
                path.Add(current.Position);
                current = current.Next;
            } while (current != null);

            StepList.Add(new Step(StepType.Current, node.Position, path));
        }            

        [Conditional("DEBUG")]
        private static void MessageOpen(Position position)
            => StepList.Add(new Step(StepType.Open, position, new List<Position>(0)));

        [Conditional("DEBUG")]
        private static void MessageClose(Position position)
            => StepList.Add(new Step(StepType.Close, position, new List<Position>(0)));

        [Conditional("DEBUG")]
        private static void ClearStepList()
            => StepList.Clear();
    }

    internal class Step
    {
        public Step(StepType type, Position position, IReadOnlyList<Position> path)
        {
            this.Type = type;
            this.Position = position;
            this.Path = path;
        }

        public StepType Type { get; }
        public Position Position { get; }
        public IReadOnlyList<Position> Path { get; }
    }

    internal enum StepType
    {
        Current,
        Open,
        Close
    }
}
