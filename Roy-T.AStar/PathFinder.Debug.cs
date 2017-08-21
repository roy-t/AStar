using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Viewer")]

namespace RoyT.AStar
{
    
    internal static partial class PathFinder
    {
        internal static List<Step> StepList { get; } = new List<Step>(0);

        [Conditional("DEBUG")]
        private static void MessageCurrent(Position position)
            =>  StepList.Add(new Step(StepType.Current, position));        

        [Conditional("DEBUG")]
        private static void MessageOpen(Position position)
            => StepList.Add(new Step(StepType.Open, position));

        [Conditional("DEBUG")]
        private static void MessageClose(Position position)
            => StepList.Add(new Step(StepType.Close, position));

        [Conditional("DEBUG")]
        private static void ClearStepList()
            => StepList.Clear();
    }

    internal struct Step
    {
        public Step(StepType type, Position position)
        {
            this.Type = type;
            this.Position = position;
        }

        public StepType Type { get; }
        public Position Position { get; }
    }

    internal enum StepType
    {
        Current,
        Open,
        Close
    }
}
