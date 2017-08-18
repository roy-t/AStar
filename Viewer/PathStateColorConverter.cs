using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Viewer
{
    internal sealed class PathStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PathState pathState)
            {
                switch (pathState)
                {
                    case PathState.Undetermined:
                        return Brushes.LightGray;
                    case PathState.Open:
                        return Brushes.Pink;
                    case PathState.Closed:
                        return Brushes.Gray;                        
                    case PathState.OnPath:
                        return Brushes.Green;      
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
