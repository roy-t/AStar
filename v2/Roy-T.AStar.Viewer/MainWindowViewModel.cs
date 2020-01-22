using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly Random Random;


        private NodeModel startNode;
        private NodeModel endNode;
        private string outcome;

        public MainWindowViewModel()
        {
            this.Random = new Random();
            this.outcome = string.Empty;

            this.ModelSource = new SourceList<ReactiveObject>();
            this.ModelSource
             .Connect()
             .ObserveOn(RxApp.MainThreadScheduler)
             .Bind(out this.ModelList)
             .Subscribe();

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
            this.ModelSource.Edit(inner =>
            {
                inner.Clear();
                foreach (var node in nodes)
                {
                    var model = new NodeModel(node);
                    model.ClickCommand = ReactiveCommand.Create(() => this.EditNode(model));

                    inner.Add(model);

                    foreach (var incomingEdge in node.Outgoing)
                    {
                        var edgeModel = new IncomingEdgeModel(incomingEdge);
                        inner.Add(edgeModel);
                    }

                    foreach (var incomingEdge in node.Incoming)
                    {
                        var edgeModel = new OutgoingEdgeModel(incomingEdge);
                        inner.Add(edgeModel);
                    }
                }
            });
        }



        private void SetSpeedLimits(Func<Velocity> speedLimitFunc)
        {
            this.ModelSource.Edit(inner =>
            {
                foreach (var obj in inner)
                {
                    if (obj is AEdgeModel edgeModel)
                    {
                        edgeModel.Velocity = speedLimitFunc();
                    }
                }
            });

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

            CalculatePath();
        }

        private void CalculatePath()
        {
            if (this.startNode != null && this.endNode != null)
            {
                var path = PathFinder.FindPath(this.startNode.Node, this.endNode.Node, Settings.MaxSpeed);
                var averageSpeed = Velocity.FromMetersPerSecond(path.Distance / path.ExpectedDuration.Seconds);
                this.Outcome = $"Found path, type: {path.Type}, distance {path.Distance:F2}m, average speed {averageSpeed}, expected duration {path.ExpectedDuration}";

                this.ClearPath();

                this.ModelSource.Edit(inner =>
                {
                    foreach (var edge in path.Edges)
                    {
                        var edgeModel = new PathEdgeModel(edge.A.X, edge.A.Y, edge.B.X, edge.B.Y);
                        inner.Add(edgeModel);
                    }
                });
            }
            else
            {
                this.ClearPath();
            }
        }

        private void ClearPath()
        {
            this.ModelSource.Edit(inner =>
            {
                var toRemove = new List<PathEdgeModel>();
                foreach (var obj in inner)
                {
                    if (obj is PathEdgeModel edgeModel)
                    {
                        toRemove.Add(edgeModel);
                    }
                }
                inner.RemoveMany(toRemove);
            });
        }
    }
}
