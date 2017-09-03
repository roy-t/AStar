using System.Collections.Generic;
using System.Diagnostics;

namespace RoyT.AStar
{
    // PathFinder.Debug already makes the internals visibile to the viewer

    public static partial class PathSmoother
    {
        internal static List<Swap> SwapList { get; } = new List<Swap>(0);

        [Conditional("DEBUG")]
        private static void MessageIterate(Position original, Position replacement)
            => SwapList.Add(new Swap(SwapType.Current, original, replacement));

        [Conditional("DEBUG")]
        private static void MessageSwap(Position original, Position replacement)
            => SwapList.Add(new Swap(SwapType.Swap, original, replacement));

        [Conditional("DEBUG")]
        internal static void ClearSwapList()
            => SwapList.Clear();
    }

    internal class Swap
    {        
        public Swap(SwapType swapType, Position original, Position replacement)
        {
            this.SwapType = swapType;
            this.Original = original;
            this.Replacement = replacement;
        }

        public SwapType SwapType { get; }
        public Position Original { get; }
        public Position Replacement { get; }
    }


    internal enum SwapType
    {
        Current,
        Swap
    }

}
