using System;
using System.Windows;
using System.Windows.Controls;
using Roy_T.AStar.Viewer.Model;

namespace Roy_T.AStar.Viewer
{
    internal sealed class GraphDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (item is EdgeModel)
            {
                return element.FindResource("EdgeDataTemplate") as DataTemplate;
            }

            if (item is PathEdgeModel)
            {
                return element.FindResource("PathEdgeDataTemplate") as DataTemplate;
            }

            if (item is NodeModel)
            {
                return element.FindResource("NodeDataTemplate") as DataTemplate;
            }

            throw new NotSupportedException();
        }
    }
}
