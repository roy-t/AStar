using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using DynamicData;
using ReactiveUI;
using Roy_T.AStar.V2.Graphs;
using Roy_T.AStar.V2.Grids;
using Roy_T.AStar.V2.Paths;
using Roy_T.AStar.V2.Primitives;
using Roy_T.AStar.Viewer.Model;

namespace Roy_T.AStar.Viewer
{
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private readonly Dictionary<INode, NodeModel> NodeDict;
        private readonly Dictionary<IEdge, EdgeModel> EdgeDict;

        private readonly Random Random;
        private readonly PathFinder PathFinder;

        private NodeModel startNode;
        private NodeModel endNode;
        private string outcome;

        public MainWindowViewModel()
        {
            this.PathFinder = new PathFinder();

            this.Nodes = new ObservableCollection<ReactiveObject>();

            this.NodeDict = new Dictionary<INode, NodeModel>();
            this.EdgeDict = new Dictionary<IEdge, EdgeModel>();

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

            this.ResetCommand = ReactiveCommand.Create(() => this.CreateNodes());
            this.RandomizeCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() =>
            {
                var value = this.Random.Next((int)Settings.MinSpeed.MetersPerSecond, (int)Settings.MaxSpeed.MetersPerSecond + 1);
                return Velocity.FromMetersPerSecond(value);
            }));

            this.MaxCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MaxSpeed));
            this.MinCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MinSpeed));

            this.CreateNodes();
        }

        public string Outcome
        {
            get => this.outcome;
            set => this.RaiseAndSetIfChanged(ref this.outcome, value);
        }

        public ObservableCollection<ReactiveObject> Nodes { get; }

        public IReactiveCommand ExitCommand { get; }
        public IReactiveCommand OpenGitHubCommand { get; }

        public IReactiveCommand ResetCommand { get; }
        public IReactiveCommand RandomizeCommand { get; }
        public IReactiveCommand MaxCommand { get; }
        public IReactiveCommand MinCommand { get; }

        private void CreateNodes()
        {
            this.Clear();

            var grid = new Grid(14, 7, 100, 100, Settings.MaxSpeed);
            this.PopupulateModelList(grid.GetAllNodes());

            this.startNode = this.NodeDict[grid.GetNode(0, 0)];
            this.startNode.NodeState = NodeState.Start;

            this.endNode = this.NodeDict[grid.GetNode(grid.Columns - 1, grid.Rows - 1)];
            this.endNode.NodeState = NodeState.End;
            this.CalculatePath();
        }

        private void Clear()
        {
            this.NodeDict.Clear();
            this.EdgeDict.Clear();
            this.startNode = null;
            this.endNode = null;
            this.outcome = string.Empty;
            this.Nodes.Clear();
        }

        private void PopupulateModelList(IEnumerable<INode> nodes)
        {
            var toAdd = new List<ReactiveObject>();
            foreach (var node in nodes)
            {
                var model = new NodeModel(node);
                model.LeftClickCommand = ReactiveCommand.Create(() => this.EditNode(model));
                model.RightClickCommand = ReactiveCommand.Create(() => this.RemoveNode(model));

                this.NodeDict.Add(node, model);
                toAdd.Add(model);

                foreach (var edge in node.Outgoing)
                {
                    var edgeModel = new EdgeModel(edge);
                    this.EdgeDict.Add(edge, edgeModel);
                    toAdd.Add(edgeModel);
                }
            }

            this.Nodes.AddRange(toAdd);
        }

        private void SetSpeedLimits(Func<Velocity> speedLimitFunc)
        {
            foreach (var edge in this.Nodes.OfType<EdgeModel>())
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
            var toRemove = new List<ReactiveObject>();

            var node = model.Node;

            foreach (var incomingEdge in node.Incoming)
            {
                var opposite = incomingEdge.Start;
                opposite.Outgoing.Remove(incomingEdge);

                toRemove.Add(this.EdgeDict[incomingEdge]);
            }

            node.Incoming.Clear();

            foreach (var outgoingEdge in node.Outgoing)
            {
                var opposite = outgoingEdge.End;
                opposite.Incoming.Remove(outgoingEdge);

                toRemove.Add(this.EdgeDict[outgoingEdge]);
            }

            node.Outgoing.Clear();

            toRemove.Add(model);
            this.Nodes.RemoveMany(toRemove);

            this.startNode = this.startNode != model ? this.startNode : null;
            this.endNode = this.endNode != model ? this.endNode : null;
            this.CalculatePath();
        }

        private void CalculatePath()
        {
            if (this.startNode != null && this.endNode != null)
            {
                var path = this.PathFinder.FindPath(this.startNode.Node, this.endNode.Node, Settings.MaxSpeed);
                var averageSpeed = Velocity.FromMetersPerSecond(path.Distance.Meters / path.Duration.Seconds);
                this.Outcome = $"Found path, type: {path.Type}, distance {path.Distance:F2}m, average speed {averageSpeed}, expected duration {path.Duration}";

                this.ClearPath();

                var toAdd = new List<PathEdgeModel>();

                foreach (var edge in path.Edges)
                {
                    var edgeModel = new PathEdgeModel(edge.Start.Position.X, edge.Start.Position.Y, edge.End.Position.X, edge.End.Position.Y);
                    toAdd.Add(edgeModel);
                }

                this.Nodes.AddRange(toAdd);
            }
            else
            {
                this.ClearPath();
            }
        }

        private void ClearPath()
        {
            var toRemove = new List<PathEdgeModel>();
            foreach (var edge in this.Nodes.OfType<PathEdgeModel>())
            {
                toRemove.Add(edge);
            }

            this.Nodes.RemoveMany(toRemove);
        }
    }
}
