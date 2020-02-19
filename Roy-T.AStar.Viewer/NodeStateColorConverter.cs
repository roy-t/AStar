using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Roy_T.AStar.Viewer.Model;

namespace Roy_T.AStar.Viewer
{
    internal sealed class NodeStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NodeState cellState)
            {
                switch (cellState)
                {
                    case NodeState.None:
                        return Brushes.Black;
                    case NodeState.Start:
                        return Brushes.LightGreen;
                    case NodeState.End:
                        return Brushes.DarkGreen;
                }
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
