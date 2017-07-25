using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RoyT.AStar;
using Grid = System.Windows.Controls.Grid;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Dimensions = 10;

        public MainWindow()
        {
            InitializeComponent();

            for (var x = 0; x < Dimensions; x++)
            {
                this.worldGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});
                this.worldGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            for (var y = 0; y < Dimensions; y++)
            {
                for (var x = 0; x < Dimensions; x++)
                {
                    var box = new TextBox();
                    if (x == 0 && y == 0)
                    {
                        box.Text = "s";
                    }
                    else if (x == Dimensions - 1 && y == Dimensions - 1)
                    {
                        box.Text = "e";
                    }
                    else
                    {
                        box.Text = "1";
                    }
                    
                    Grid.SetColumn(box, x);
                    Grid.SetRow(box, y);

                    this.worldGrid.Children.Add(box);
                }
            }
        }

        private void Go_OnClick(object sender, RoutedEventArgs e)
        {
            var sx = 0;
            var sy = 0;
            var ex = 0;
            var ey = 0;

            var grid = new RoyT.AStar.Grid(Dimensions, Dimensions, 1);

            foreach(var c in this.worldGrid.Children)
            {
                if (c is TextBox box)
                {
                    var x = Grid.GetColumn(box);
                    var y = Grid.GetRow(box);

                    if (box.Text == "s")
                    {
                        sx = x;
                        sy = y;
                    }
                    else if (box.Text == "e")
                    {
                        ex = x;
                        ey = y;
                    }
                    else
                    {
                        var cost = double.Parse(box.Text);
                        grid.SetCellCost(new Position(x, y), cost);
                    }
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var path = grid.GetPath(new Position(sx, sy), new Position(ex, ey));
            var elapsed = stopwatch.Elapsed;
            this.label.Text = $"Elapsed time: {elapsed.TotalMilliseconds}ms";

            foreach (var c in this.worldGrid.Children)
            {
                if (c is TextBox box)
                {
                    var x = Grid.GetColumn(box);
                    var y = Grid.GetRow(box);

                    if (path.Any(p => p.X == x && p.Y == y))
                    {
                        box.Background = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        box.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }

        }        
    }
}
