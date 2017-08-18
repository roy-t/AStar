using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ReactiveUI;

namespace Viewer
{
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private readonly PathController PathController;
        private List<Cell> cells;

        public MainWindowViewModel()
        {
            this.PathController = new PathController();
            this.cells = new List<Cell>(100);
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    var cell = new Cell(x, y);
                    cell.Command = ReactiveCommand.Create(
                        () =>
                        {
                            EditCell(cell);
                        });

                    this.cells.Add(cell);
                }
            }

            this.cells.First().CellState = CellState.Start;
            this.cells.Last().CellState = CellState.End;   
            
            this.StartCommand    = ReactiveCommand.Create(() => this.PathController.Start());
            this.EndCommand      = ReactiveCommand.Create(() => this.PathController.End());
            this.ForwardCommand  = ReactiveCommand.Create(() => this.PathController.Forward());
            this.BackwardCommand = ReactiveCommand.Create(() => this.PathController.Backward());

            this.SaveCommand = ReactiveCommand.Create(() => IO.Save(this.Cells));            
            this.ExitCommand = ReactiveCommand.Create(() => Application.Current.Shutdown());

            this.LoadCommand = ReactiveCommand.Create(
                () =>
                {
                    var ioCells = IO.Load();
                    foreach (var c in ioCells)
                    {
                        c.Command = ReactiveCommand.Create(
                            () =>
                            {
                                EditCell(c);
                            });
                    }

                    this.Cells = ioCells;
                });

            this.PathController.ComputePath(this.Cells);
        }

        public IReadOnlyList<Cell> Cells
        {
            get => this.cells;
            set => this.RaiseAndSetIfChanged(ref this.cells, value.ToList());
        }

        public ReactiveCommand StartCommand { get; }
        public ReactiveCommand EndCommand { get; }
        public ReactiveCommand ForwardCommand { get; }
        public ReactiveCommand BackwardCommand { get; }
        public ReactiveCommand SaveCommand { get; }
        public ReactiveCommand LoadCommand { get; }
        public ReactiveCommand ExitCommand { get; }

        private void EditCell(Cell cell)
        {
            var vm = new EditWindowViewModel(cell.X, cell.Y, cell.CellState, cell.Cost);
            var window = new EditWindow {DataContext = vm, Owner = Application.Current.MainWindow};
            window.ShowDialog();

            if (cell.CellState != CellState.Start && vm.CellState == CellState.Start)
            {
                ClearState(CellState.Start);
            }

            if (cell.CellState != CellState.End && vm.CellState == CellState.End)
            {
                ClearState(CellState.End);
            }

            cell.CellState = vm.CellState;
            cell.Cost = vm.Cost;

            this.PathController.ComputePath(this.Cells);
        }

        private void ClearState(CellState state)
        {
            foreach (var c in this.Cells)
            {
                if (c.CellState == state)
                {
                    c.CellState = CellState.Normal;                    
                }
            }
        }
    }
}
