using ReactiveUI;

namespace Viewer
{
    internal sealed class Cell : ReactiveObject
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

        public int X { get; }

        public int Y { get; }

        public float Cost
        {
            get => this.cost;
            set => this.RaiseAndSetIfChanged(ref this.cost, value);
        }

        public float CostSoFar
        {
            get => this.costSoFar;
            set => this.RaiseAndSetIfChanged(ref this.costSoFar, value);
        }

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
