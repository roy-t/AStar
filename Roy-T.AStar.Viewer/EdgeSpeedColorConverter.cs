using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Viewer
{
    internal sealed class EdgeSpeedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Velocity velocity)
            {
                var range = Settings.MaxSpeed.MetersPerSecond - Settings.MinSpeed.MetersPerSecond;
                var lerp = (velocity.MetersPerSecond - Settings.MinSpeed.MetersPerSecond) / range;
                var invLerp = 1.0f - lerp;

                var color = Color.FromScRgb(1.0f, invLerp, lerp, 0.0f);
                return new SolidColorBrush(color);
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
