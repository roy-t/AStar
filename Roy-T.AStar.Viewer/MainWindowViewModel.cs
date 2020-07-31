using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using DynamicData;
using Microsoft.Win32;
using ReactiveUI;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using Roy_T.AStar.Serialization;
using Roy_T.AStar.Viewer.Model;

namespace Roy_T.AStar.Viewer
{
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private readonly Random Random;
        private readonly PathFinder PathFinder;

        private NodeModel startNode;
        private NodeModel endNode;
        private Grid grid;
        private string outcome;

        public MainWindowViewModel()
        {
            this.PathFinder = new PathFinder();

            this.Models = new ObservableCollection<ReactiveObject>();

            this.Random = new Random();
            this.outcome = string.Empty;

            this.ExitCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.Shutdown();
            });

            this.OpenGitHubCommand = ReactiveCommand.Create(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "cmd",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"/c start http://github.com/roy-t/AStar"
                };
                Process.Start(psi);
            });

            this.ResetCommand = ReactiveCommand.Create(() => this.CreateNodes(Connections.LateralAndDiagonal));
            this.LateralCommand = ReactiveCommand.Create(() => this.CreateNodes(Connections.Lateral));
            this.DiagonalCommand = ReactiveCommand.Create(() => this.CreateNodes(Connections.Diagonal));
            this.RandomizeCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() =>
            {
                var value = this.Random.Next((int)Settings.MinSpeed.MetersPerSecond, (int)Settings.MaxSpeed.MetersPerSecond + 1);
                return Velocity.FromMetersPerSecond(value);
            }));

            this.MaxCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MaxSpeed));
            this.MinCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MinSpeed));

            this.SaveGridCommand = ReactiveCommand.Create(this.SaveGrid);
            this.OpenGridCommand = ReactiveCommand.Create(this.OpenGrid);

            this.CreateNodes(Connections.LateralAndDiagonal);
        }

        public string Outcome
        {
            get => this.outcome;
            set => this.RaiseAndSetIfChanged(ref this.outcome, value);
        }

        public ObservableCollection<ReactiveObject> Models { get; }

        public IReactiveCommand ExitCommand { get; }
        public IReactiveCommand OpenGitHubCommand { get; }

        public IReactiveCommand ResetCommand { get; }

        public IReactiveCommand LateralCommand { get; }

        public IReactiveCommand DiagonalCommand { get; }
        public IReactiveCommand RandomizeCommand { get; }
        public IReactiveCommand MaxCommand { get; }
        public IReactiveCommand MinCommand { get; }

        public IReactiveCommand SaveGridCommand { get; }

        public IReactiveCommand OpenGridCommand { get; }

        private void CreateNodes(Connections connections)
        {
            this.Clear();
            this.grid = CreateGrid(connections);
            var models = ModelBuilder.BuildModel(this.grid, n => this.EditNode(n), n => this.RemoveNode(n));
            this.Models.AddRange(models);

            this.startNode = this.Models.OfType<NodeModel>().FirstOrDefault();
            this.startNode.NodeState = NodeState.Start;

            this.endNode = this.Models.OfType<NodeModel>().LastOrDefault();
            this.endNode.NodeState = NodeState.End;

            this.CalculatePath();
        }

        private void SaveGrid()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, GridSerializer.SerializeGrid(this.grid));
        }

        private void OpenGrid()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                this.Clear();
                this.grid = GridSerializer.DeSerializeGrid(File.ReadAllText(openFileDialog.FileName));

                var models = ModelBuilder.BuildModel(this.grid, n => this.EditNode(n), n => this.RemoveNode(n));
                this.Models.AddRange(models);

                this.startNode = this.Models.OfType<NodeModel>().FirstOrDefault();
                this.startNode.NodeState = NodeState.Start;

                this.endNode = this.Models.OfType<NodeModel>().LastOrDefault();
                this.endNode.NodeState = NodeState.End;

                this.CalculatePath();
            }
        }

        private static Grid CreateGrid(Connections connections)
        {
            var gridSize = new GridSize(columns: 14, rows: 7);
            var cellSize = new Primitives.Size(Distance.FromMeters(100), Distance.FromMeters(100));

            return connections switch
            {
                Connections.Lateral => Grid.CreateGridWithLateralConnections(gridSize, cellSize, Settings.MaxSpeed),
                Connections.Diagonal => Grid.CreateGridWithDiagonalConnections(gridSize, cellSize, Settings.MaxSpeed),
                Connections.LateralAndDiagonal => Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, Settings.MaxSpeed),
                _ => throw new ArgumentOutOfRangeException(nameof(connections), $"Invalid connection type {connections}")
            };
        }

        private void Clear()
        {
            this.startNode = null;
            this.endNode = null;
            this.outcome = string.Empty;
            this.Models.Clear();
        }

        private void SetSpeedLimits(Func<Velocity> speedLimitFunc)
        {
            foreach (var edge in this.Models.OfType<EdgeModel>())
            {
                edge.Velocity = speedLimitFunc();
            }

            this.CalculatePath();
        }

        private void EditNode(NodeModel model)
        {
            switch (model.NodeState)
            {
                case NodeState.Start:
                    model.NodeState = NodeState.End;
                    if (this.endNode != null)
                    {
                        this.endNode.NodeState = NodeState.None;
                    }
                    this.startNode = null;
                    this.endNode = model;
                    break;
                case NodeState.End:
                    this.endNode = null;
                    model.NodeState = NodeState.None;
                    break;
                case NodeState.None:
                    model.NodeState = NodeState.Start;
                    if (this.startNode != null)
                    {
                        this.startNode.NodeState = NodeState.None;
                    }
                    this.startNode = model;
                    break;
            }

            this.CalculatePath();
        }

        private void RemoveNode(NodeModel model)
        {
            this.grid.DisconnectNode(model.GridPosition);
            this.grid.RemoveDiagonalConnectionsIntersectingWithNode(model.GridPosition);

            this.Models.Clear();
            this.Models.AddRange(ModelBuilder.BuildModel(this.grid, n => this.EditNode(n), n => this.RemoveNode(n)));

            if (this.startNode != null)
            {
                this.startNode = this.Models.OfType<NodeModel>().FirstOrDefault(n => n.GridPosition == this.startNode.GridPosition);
            }

            if (this.startNode != null)
            {
                this.startNode.NodeState = NodeState.Start;
            }

            if (this.endNode != null)
            {
                this.endNode = this.Models.OfType<NodeModel>().FirstOrDefault(n => n.GridPosition == this.endNode.GridPosition);
            }

            if (this.endNode != null)
            {
                this.endNode.NodeState = NodeState.End;
            }

            this.CalculatePath();
        }

        private void CalculatePath()
        {
            if (this.startNode != null && this.endNode != null)
            {
                var path = this.PathFinder.FindPath(this.startNode.Node, this.endNode.Node, Settings.MaxSpeed);
                var averageSpeed = Velocity.FromMetersPerSecond(path.Distance.Meters / path.Duration.Seconds);
                this.Outcome = $"Found path, type: {path.Type}, distance {path.Distance}, average speed {averageSpeed}, expected duration {path.Duration}";

                this.ClearPath();

                var toAdd = new List<PathEdgeModel>();

                foreach (var edge in path.Edges)
                {
                    var edgeModel = new PathEdgeModel(edge.Start.Position.X, edge.Start.Position.Y, edge.End.Position.X, edge.End.Position.Y);
                    toAdd.Add(edgeModel);
                }

                this.Models.AddRange(toAdd);
            }
            else
            {
                this.ClearPath();
            }
        }

        private void ClearPath()
        {
            var toRemove = new List<PathEdgeModel>();
            foreach (var edge in this.Models.OfType<PathEdgeModel>())
            {
                toRemove.Add(edge);
            }

            this.Models.RemoveMany(toRemove);
        }
    }
}
