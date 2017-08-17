using ReactiveUI;

namespace Viewer
{
    internal sealed class Cell : ReactiveObject
    {
        private float cost;
        private CellState state;

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

        public CellState State
        {
            get => this.state;
            set => this.RaiseAndSetIfChanged(ref this.state, value);
        }

        public ReactiveCommand Command { get; set; }
    }

    internal enum CellState
    {
        Normal,
        Start,
        End,
        Blocked
    }
}
