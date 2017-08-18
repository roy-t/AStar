using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Viewer
{
    internal sealed class CellStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CellState cellState)
            {
                switch (cellState)
                {
                    case CellState.Normal:                        
                        return Brushes.White;                        
                    case CellState.Start:
                        return Brushes.LightGreen;                        
                    case CellState.End:
                        return Brushes.DarkGreen;
                    case CellState.Blocked:
                        return Brushes.Black;
                    case CellState.Open:
                        return Brushes.SandyBrown;
                    case CellState.Closed:
                        return Brushes.Gray;
                    case CellState.OnPath:
                        return Brushes.DeepSkyBlue;
                }
            }

            return Brushes.CornflowerBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
