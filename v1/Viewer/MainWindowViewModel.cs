using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ReactiveUI;

namespace Viewer
{
    /// <summary>
    /// ViewModel for the MainView, visualized the path finding algorithm
    /// and allows the user to edit the world
    /// </summary>
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private const int DefaultIterationLimit = 100;

        private readonly PathController PathController;
        private readonly ReplayController ReplayController;
        private List<Cell> cells;
        private int iterationLimit;

        public MainWindowViewModel()
        {
            this.PathController = new PathController();
            this.ReplayController = new ReplayController();
            this.iterationLimit = DefaultIterationLimit;

            this.StartCommand = ReactiveCommand.Create(
                () =>
                {
                    this.ReplayController.Start(this.cells);
                    UpdatePathBindings();
                });

            this.EndCommand = ReactiveCommand.Create(
                () =>
                {
                    this.ReplayController.End(this.cells);
                    UpdatePathBindings();
                });

            this.ForwardCommand = ReactiveCommand.Create(
                () =>
                {
                    this.ReplayController.Forward(this.cells);
                    UpdatePathBindings();
                });

            this.BackwardCommand = ReactiveCommand.Create(
                () =>
                {
                    this.ReplayController.Backward(this.cells);
                    UpdatePathBindings();
                });

            this.SaveCommand = ReactiveCommand.Create(() => IO.Save(this.Cells));
            this.ExitCommand = ReactiveCommand.Create(() => Application.Current.Shutdown());

            this.LoadCommand = ReactiveCommand.Create(
                () =>
                {
                    var ioCells = IO.Load();
                    FillCells(ioCells);
                });

            List<Cell> startCells;
            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                startCells = IO.Load(args[1]).ToList();
            }
            else
            {
                startCells = new List<Cell>(100);
                for (var y = 0; y < 10; y++)
                {
                    for (var x = 0; x < 10; x++)
                    {
                        var cell = new Cell(x, y);
                        startCells.Add(cell);
                    }
                }

                startCells.First().CellState = CellState.Start;
                startCells.Last().CellState = CellState.End;
            }

            FillCells(startCells);

#if DEBUG
            this.IsDebugBuild = true;

#else
            this.IsDebugBuild = false;
#endif            
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

        public int StepCount => ReplayController.GetMaxStep();

        public int CurrentStep
        {
            get => this.ReplayController.CurrentStep;
            set
            {
                this.ReplayController.CurrentStep = value;
                this.ReplayController.ReplayPathFindingSteps(this.Cells);
                UpdatePathBindings();
            }
        }



        public string IterationLimit
        {
            get => this.iterationLimit.ToString();
            set
            {
                this.iterationLimit = int.TryParse(value, out int result)
                    ? result
                    : DefaultIterationLimit;

                UpdatePath();
            }
        }


        public bool IsDebugBuild { get; }
        public bool IsReleaseBuild => !this.IsDebugBuild;

        private void FillCells(IReadOnlyList<Cell> ioCells)
        {
            foreach (var c in ioCells)
            {
                c.Command = ReactiveCommand.Create(
                    () =>
                    {
                        EditCell(c);
                    });
            }

            this.Cells = ioCells;

            UpdatePath();
        }

        // Shows the edit window, and makes sure a new path is calculated after anything changed
        private void EditCell(Cell cell)
        {
            var vm = new EditWindowViewModel(cell.X, cell.Y, cell.CellState, cell.Cost);
            var window = new EditWindow { DataContext = vm, Owner = Application.Current.MainWindow };
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

            UpdatePath();
        }

        private void UpdatePath()
        {
            this.PathController.ComputePath(this.Cells, this.iterationLimit);
            this.ReplayController.End(this.Cells);
            UpdatePathBindings();
        }

        private void UpdatePathBindings()
        {
            this.RaisePropertyChanged(nameof(this.StepCount));
            this.RaisePropertyChanged(nameof(this.CurrentStep));
            this.RaisePropertyChanged(nameof(this.IterationLimit));
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
