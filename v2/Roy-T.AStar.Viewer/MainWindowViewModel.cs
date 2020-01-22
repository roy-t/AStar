using System;
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

        public MainWindowViewModel()
        {
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

            var nodeA = new Node(60, 60);
            var nodeB = new Node(250, 250);
            var edge = new Edge(nodeA, nodeB, Velocity.FromMetersPerSecond(3));

            nodeA.Outgoing.Add(edge);
            nodeB.Incoming.Add(edge);

            this.PopupulateModelList(nodeA, nodeB);
        }

        public ReadOnlyObservableCollection<ReactiveObject> Nodes => this.ModelList;

        public IReactiveCommand ExitCommand { get; }

        private void PopupulateModelList(params Node[] nodes)
        {
            this.ModelSource.Clear();

            foreach (var node in nodes)
            {
                var model = new NodeModel(node);
                model.ClickCommand = ReactiveCommand.Create(() => this.EditNode(model));

                this.ModelSource.Add(model);

                foreach (var incomingEdge in node.Outgoing)
                {
                    var edgeModel = new IncomingEdgeModel(incomingEdge);
                    this.ModelSource.Add(edgeModel);
                }

                foreach (var incomingEdge in node.Incoming)
                {
                    var edgeModel = new OutgoingEdgeModel(incomingEdge);
                    this.ModelSource.Add(edgeModel);
                }
            }
        }

        private void EditNode(NodeModel model)
        {
            var a = model.Node.Outgoing[0].A;
            var b = model.Node.Outgoing[0].B;

            var edgeModel = new PathEdgeModel(a.X, a.Y, b.X, b.Y);
            this.ModelSource.Add(edgeModel);
        }
    }
}
