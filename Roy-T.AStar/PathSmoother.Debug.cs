using System.Collections.Generic;
using System.Diagnostics;

namespace RoyT.AStar
{
    public static partial class PathSmoother
    {
        internal static List<Swap> SwapList { get; } = new List<Swap>(0);
       
        [Conditional("DEBUG")]
        private static void MessageSwap(Position original, Position replacement)
            => SwapList.Add(new Swap(original, replacement));

        [Conditional("DEBUG")]
        internal static void ClearSwapList()
            => SwapList.Clear();
    }

    internal class Swap
    {        
        public Swap(Position original, Position replacement)
        {
            this.Original = original;
            this.Replacement = replacement;
        }
        
        public Position Original { get; }
        public Position Replacement { get; }
    }   
}
