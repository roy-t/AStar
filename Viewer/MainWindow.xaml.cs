using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RoyT.AStar;
using Grid = System.Windows.Controls.Grid;

namespace Viewer
{
    /// <summary>
    /// Extremely hacky 'editor' to visualize our A* algorithm
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Size = 10;
        private const string StartSymbol = "s";
        private const string EndSymbol = "e";

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the grid (defined in XAML) with 10 columns and 10 rows
            for (var x = 0; x < Size; x++)
            {
                this.worldGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});
                this.worldGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // Adds a text block to every row and column
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {                    
                    var box = new TextBox();
                    if (x == 0 && y == 0)
                    {
                        // The contents of the top left cell is the start symbol
                        box.Text = StartSymbol;
                    }
                    else if (x == Size - 1 && y == Size - 1)
                    {
                        // The contents of the bottom right cell is the end symbol
                        box.Text = EndSymbol;
                    }
                    else
                    {
                        // All other cells are filled with a cost of 1
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
            
            var grid = new RoyT.AStar.Grid(Size, Size, 1);

            // Iterate all cells, store the coordinates of the start and the end symbol,
            // fill the grid with the correct traversal costs
            foreach(var c in this.worldGrid.Children)
            {
                if (c is TextBox box)
                {                    
                    var x = Grid.GetColumn(box);
                    var y = Grid.GetRow(box);

                    if (box.Text == StartSymbol)
                    {
                        sx = x;
                        sy = y;
                    }
                    else if (box.Text == EndSymbol)
                    {
                        ex = x;
                        ey = y;
                    }
                    else
                    {
                        var cost = 1.0f;
                        if (float.TryParse(box.Text, out float cellCost))
                        {
                            cost = cellCost;
                        }
                        grid.SetCellCost(new Position(x, y), cost);
                    }
                }
            }

            // Kick of the A* search
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var path = grid.GetPath(new Position(sx, sy), new Position(ex, ey));

            // Display the approximate speed
            var elapsed = stopwatch.Elapsed;
            this.label.Text = $"Elapsed time: {elapsed.TotalMilliseconds}ms";

            // Color the fastest path
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
