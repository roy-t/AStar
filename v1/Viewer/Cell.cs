using System.Collections.Generic;
using Newtonsoft.Json;
using ReactiveUI;

namespace Viewer
{
    /// <summary>
    /// ViewModel and serialization object for a cell inside a grid
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class Cell : ReactiveObject
    {
        private float cost;                      
        private CellState cellState;

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Cost = 1.0f;            
        }

        [JsonProperty]
        public int X { get; }

        [JsonProperty]
        public int Y { get; }

        [JsonProperty]
        public float Cost
        {
            get => this.cost;
            set => this.RaiseAndSetIfChanged(ref this.cost, value);
        }        

        [JsonProperty]
        public CellState CellState
        {
            get => this.cellState;
            set => this.RaiseAndSetIfChanged(ref this.cellState, value);
        }       

        public ReactiveCommand Command { get; set; }

        /// <summary>
        /// The cell state was set by the user view the editor and should not be changed
        /// </summary>
        public static HashSet<CellState> UserCellStates => new HashSet<CellState>
        {
            CellState.Blocked,
            CellState.Start,
            CellState.End
        };

        /// <summary>
        /// The cell state was set during replay
        /// </summary>
        public static HashSet<CellState> ReplayCellStates => new HashSet<CellState>
        {
            CellState.Current,
            CellState.Open,
            CellState.Closed,
            CellState.OnPath,
        };
    }

    /// <summary>
    /// Indicates how the cell should be handled by the pathfinding algorithm
    /// and what color the cell should be editor.
    /// </summary>
    internal enum CellState
    {
        Normal,

        Start,
        End,
        Blocked,

        Current,
        Open,
        Closed,
        OnPath        
    }
}
