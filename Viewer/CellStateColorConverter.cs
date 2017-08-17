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
                        return Brushes.LightGray;                        
                    case CellState.Start:
                        return Brushes.LightGreen;
                    case CellState.End:
                        return Brushes.LightYellow;
                    case CellState.Blocked:
                        return Brushes.Black;                                            
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
