using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ReactiveUI;

namespace Viewer
{
    internal sealed class MainWindowViewModel
    {
        private readonly PathController PathController;

        public MainWindowViewModel()
        {
            this.PathController = new PathController();
            this.Cells = new List<Cell>(100);
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

                    this.Cells.Add(cell);
                }
            }

            this.Cells.First().CellState = CellState.Start;
            this.Cells.Last().CellState = CellState.End;

            this.ComputeCommand = ReactiveCommand.Create(
                () =>
                {
                    this.PathController.ComputePath(this.Cells);
                }
            );
        }

        public ReactiveCommand ComputeCommand { get; }

        public List<Cell> Cells { get; }

        private void EditCell(Cell cell)
        {
            var vm = new EditWindowViewModel(cell.X, cell.Y, cell.CellState, cell.Cost);
            var window = new EditWindow {DataContext = vm, Owner = Application.Current.MainWindow};
            window.ShowDialog();

            if (cell.CellState != CellState.Start && vm.CellState == CellState.Start)
            {
                ClearState(CellState.Start);
            }

            if (cell.CellState == CellState.End && vm.CellState == CellState.End)
            {
                ClearState(CellState.End);
            }

            cell.CellState = vm.CellState;
            cell.Cost = vm.Cost;
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
