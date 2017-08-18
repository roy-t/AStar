using Newtonsoft.Json;
using ReactiveUI;

namespace Viewer
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Cell : ReactiveObject
    {
        private float cost,
                      costSoFar;
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
        public float CostSoFar
        {
            get => this.costSoFar;
            set => this.RaiseAndSetIfChanged(ref this.costSoFar, value);
        }

        [JsonProperty]
        public CellState CellState
        {
            get => this.cellState;
            set => this.RaiseAndSetIfChanged(ref this.cellState, value);
        }       

        public ReactiveCommand Command { get; set; }
    }

    internal enum CellState
    {        
        Normal,
        Start,
        End,
        Blocked,
        Open,
        Closed,
        OnPath
    }   
}
