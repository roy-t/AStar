using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using DynamicData;
using ReactiveUI;
using Roy_T.AStar.V2;
using Roy_T.AStar.Viewer.Model;

namespace Roy_T.AStar.Viewer
{
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        private readonly SourceList<ReactiveObject> ModelSource;
        private readonly ReadOnlyObservableCollection<ReactiveObject> ModelList;

        private readonly Dictionary<INode, NodeModel> NodeDict;
        private readonly Dictionary<IEdge, AEdgeModel> EdgeDict;

        private readonly Random Random;

        private NodeModel startNode;
        private NodeModel endNode;
        private string outcome;

        public MainWindowViewModel()
        {
            this.ModelSource = new SourceList<ReactiveObject>();
            this.ModelSource
             .Connect()
             .ObserveOn(RxApp.MainThreadScheduler)
             .Bind(out this.ModelList)
             .Subscribe();

            this.NodeDict = new Dictionary<INode, NodeModel>();
            this.EdgeDict = new Dictionary<IEdge, AEdgeModel>();

            this.Random = new Random();
            this.outcome = string.Empty;

            this.ExitCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.Shutdown();
            });

            this.RandomizeCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() =>
            {
                var value = this.Random.Next((int)Settings.MinSpeed.MetersPerSecond, (int)Settings.MaxSpeed.MetersPerSecond + 1);
                return Velocity.FromMetersPerSecond(value);
            }));
            this.MaxCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MaxSpeed));
            this.MinCommand = ReactiveCommand.Create(() => this.SetSpeedLimits(() => Settings.MinSpeed));

            var grid = new Grid(5, 5, 100, 100, Settings.MaxSpeed);
            this.PopupulateModelList(grid.GetAllNodes());

            //var o = new ObservableCollection<ReactiveObject>();
        }

        public string Outcome
        {
            get => this.outcome;
            set => this.RaiseAndSetIfChanged(ref this.outcome, value);
        }

        public ReadOnlyObservableCollection<ReactiveObject> Nodes => this.ModelList;

        public IReactiveCommand ExitCommand { get; }

        public IReactiveCommand RandomizeCommand { get; }
        public IReactiveCommand MaxCommand { get; }
        public IReactiveCommand MinCommand { get; }

        private void PopupulateModelList(IEnumerable<INode> nodes)
        {
            this.NodeDict.Clear();
            this.EdgeDict.Clear();

            var toAdd = new List<ReactiveObject>();
            foreach (var node in nodes)
            {
                var model = new NodeModel(node);
                model.LeftClickCommand = ReactiveCommand.Create(() => this.EditNode(model));
                model.RightClickCommand = ReactiveCommand.Create(() => this.RemoveNode(model));

                this.NodeDict.Add(node, model);
                toAdd.Add(model);

                foreach (var outgoingEdge in node.Outgoing)
                {
                    var edgeModel = new IncomingEdgeModel(outgoingEdge);
                    this.EdgeDict.Add(outgoingEdge, edgeModel);
                    toAdd.Add(edgeModel);
                }

                foreach (var incomingEdge in node.Incoming)
                {
                    var edgeModel = new OutgoingEdgeModel(incomingEdge);
                    this.EdgeDict.Add(incomingEdge, edgeModel);
                    toAdd.Add(edgeModel);
                }
            }

            this.ModelSource.AddRange(toAdd);
        }

        private void SetSpeedLimits(Func<Velocity> speedLimitFunc)
        {
            foreach (var edge in this.ModelSource.Items.OfType<AEdgeModel>())
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

            foreach (var outgoingEdge in node.Outgoing)
            {
                var opposite = outgoingEdge.GetOppositeNode(node);
                var incomingEdge = opposite.GetEdgeFrom(node);
                opposite.Incoming.Remove(incomingEdge);

                toRemove.Add(this.EdgeDict[outgoingEdge]);
                toRemove.Add(this.EdgeDict[incomingEdge]);
            }

            foreach (var incomingEdge in node.Incoming)
            {
                var opposite = incomingEdge.GetOppositeNode(node);
                var outgoingEdge = opposite.GetEdgeTo(node);
                opposite.Outgoing.Remove(outgoingEdge);

                toRemove.Add(this.EdgeDict[incomingEdge]);
                toRemove.Add(this.EdgeDict[outgoingEdge]);
            }

            toRemove.Add(model);

            node.Outgoing.Clear();
            node.Incoming.Clear();

            this.ModelSource.RemoveMany(toRemove);

            this.startNode = this.startNode != model ? this.startNode : null;
            this.endNode = this.endNode != model ? this.endNode : null;
            this.CalculatePath();
        }

        private void CalculatePath()
        {
            if (this.startNode != null && this.endNode != null)
            {
                var path = PathFinder.FindPath(this.startNode.Node, this.endNode.Node, Settings.MaxSpeed);
                var averageSpeed = Velocity.FromMetersPerSecond(path.Distance / path.ExpectedDuration.Seconds);
                this.Outcome = $"Found path, type: {path.Type}, distance {path.Distance:F2}m, average speed {averageSpeed}, expected duration {path.ExpectedDuration}";

                this.ClearPath();

                var toAdd = new List<PathEdgeModel>();

                foreach (var edge in path.Edges)
                {
                    var edgeModel = new PathEdgeModel(edge.A.X, edge.A.Y, edge.B.X, edge.B.Y);
                    toAdd.Add(edgeModel);
                }

                this.ModelSource.AddRange(toAdd);
            }
            else
            {
                this.ClearPath();
            }
        }

        private void ClearPath()
        {
            var toRemove = new List<PathEdgeModel>();
            foreach (var edge in this.ModelSource.Items.OfType<PathEdgeModel>())
            {
                toRemove.Add(edge);
            }

            this.ModelSource.RemoveMany(toRemove);
        }
    }
}
