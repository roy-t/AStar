using ReactiveUI;

namespace Viewer
{
    /// <summary>
    /// ViewModel for the editor window
    /// </summary>
    internal sealed class EditWindowViewModel : ReactiveObject
    {        
        private float cost;

        public EditWindowViewModel(int x, int y, CellState state, float cost)
        {
            this.X = x;
            this.Y = y;
            this.CellState = state;
            this.Cost = cost;
        }

        public int X { get; }
        public int Y { get; }

        public string Title => $"Cell {this.X}, {this.Y}";

        public CellState CellState { get; private set; }

        public bool IsStart
        {
            get => this.CellState == CellState.Start;
            set
            {
                this.CellState = value ? CellState.Start : CellState.Normal;
                InvalidateStates();
            }
        }

        public bool IsEnd
        {
            get => this.CellState == CellState.End;
            set
            {
                this.CellState = value ? CellState.End : CellState.Normal;
                InvalidateStates();
            }
        }

        public bool IsBlocked
        {
            get => this.CellState == CellState.Blocked;
            set
            {
                this.CellState = value ? CellState.Blocked : CellState.Normal;
                InvalidateStates();
            }
        }

        public float Cost
        {
            get => this.cost;
            set => this.RaiseAndSetIfChanged(ref this.cost, value);
        }        

        private void InvalidateStates()
        {
            this.RaisePropertyChanged(nameof(this.IsStart));
            this.RaisePropertyChanged(nameof(this.IsEnd));
            this.RaisePropertyChanged(nameof(this.IsBlocked));
        }
    }
}
