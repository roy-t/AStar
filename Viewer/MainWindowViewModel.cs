using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ReactiveUI;
using RoyT.AStar;

namespace Viewer
{
    /// <summary>
    /// ViewModel for the MainView, visualized the path finding algorithm
    /// and allows the user to edit the world
    /// </summary>
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private readonly PathController PathController;
        private readonly ReplayController ReplayController;
        private List<Cell> cells;
        private bool smoothPath;

        public MainWindowViewModel()
        {
            this.PathController = new PathController();
            this.ReplayController = new ReplayController();                     

            this.StartCommand = ReactiveCommand.Create(
                () =>
                {                                        
                    this.ReplayController.Start(this.cells, this.smoothPath);
                    UpdatePathBindings();
                });

            this.EndCommand = ReactiveCommand.Create(
                () =>
                {                                        
                    this.ReplayController.End(this.cells, this.smoothPath);
                    UpdatePathBindings();
                });

            this.ForwardCommand = ReactiveCommand.Create(
                () =>
                {                                        
                    this.ReplayController.Forward(this.cells, this.smoothPath);
                    UpdatePathBindings();
                });

            this.BackwardCommand = ReactiveCommand.Create(
                () =>
                {                                        
                    this.ReplayController.Backward(this.cells, this.smoothPath);
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

        public bool SmoothPath
        {
            get => this.smoothPath;
            set
            {
                this.RaiseAndSetIfChanged(ref this.smoothPath, value);                
                UpdatePath();
            }
        }

        public int StepCount => ReplayController.GetMaxStep(this.smoothPath);

        public int CurrentStep
        {
            get => this.ReplayController.CurrentStep;
            set
            {
                this.ReplayController.CurrentStep = value;
                this.ReplayController.ReplayPathFindingSteps(this.Cells, this.smoothPath);
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
           
            UpdatePath();
        }

        private void UpdatePath()
        {
            this.PathController.ComputePath(this.Cells, this.smoothPath);
            this.ReplayController.End(this.Cells, this.smoothPath);
            UpdatePathBindings();
        }

        private void UpdatePathBindings()
        {
            this.RaisePropertyChanged(nameof(this.StepCount));
            this.RaisePropertyChanged(nameof(this.CurrentStep));
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
