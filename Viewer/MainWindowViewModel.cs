using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ReactiveUI;

namespace Viewer
{
    internal sealed class MainWindowViewModel
    {
        public MainWindowViewModel()
        {            
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

            this.Cells.First().State = CellState.Start;
            this.Cells.Last().State = CellState.End;            
        }

        public List<Cell> Cells { get; set; }


        private void EditCell(Cell cell)
        {
            var vm = new EditWindowViewModel(cell.X, cell.Y, cell.State, cell.Cost);
            var window = new EditWindow {DataContext = vm, Owner = Application.Current.MainWindow};
            window.ShowDialog();

            if (cell.State == CellState.Normal && vm.CellState == CellState.Start)
            {
                ClearState(CellState.Start);
            }

            if (cell.State == CellState.Normal && vm.CellState == CellState.End)
            {
                ClearState(CellState.End);
            }

            cell.State = vm.CellState;
            cell.Cost = vm.Cost;
        }

        private void ClearState(CellState state)
        {
            foreach (var c in this.Cells)
            {
                if (c.State == state)
                {
                    c.State = CellState.Normal;                    
                }
            }
        }
    }
}
