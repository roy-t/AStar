# Roy-T.AStar
A fast 2D path finding library based on the A* algorithm. Works with both grids and graphs. Supports any .NET variant that supports .NETStandard 2.0 or higher. This library has no external dependencies. The library is licensed under the MIT license, see the `LICENSE` file for more details.

A* is a greedy, graph based, path finding algorithm. It works by using a heuristic to guide the traveral along the graph. In this library we use the Euclidian distance heuristic. For a comprehensive overview of how the A* algorithm works I recommend this interactive [article](https://www.redblobgames.com/pathfinding/a-star/introduction.html) by Red Blob Games.

## Installation
Add this library to your project using [NuGet](https://www.nuget.org/packages/RoyT.AStar/):

```
Install-Package RoyT.AStar
```

## Usage Example
### Grids
```csharp
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;

// ....

var gridSize = new GridSize(columns: 10, rows: 10);
var cellSize = new Size(Distance.FromMeters(10), Distance.FromMeters(10));
var traversalVelocity = Velocity.FromKilometersPerHour(100);

// Create a new grid, each cell is laterally connected (like how a king moves over a chess board, other options are available)
// each cell is 10x10 meters large. The connection between cells can be traversed at 100KM/h.
var grid = Grid.CreateGridWithLateralConnections(gridSize, cellSize, traversalVelocity);

var pathFinder = new PathFinder();
var path = pathFinder.FindPath(new GridPosition(0, 0), new GridPosition(9, 9), grid);

Console.WriteLine($"type: {path.Type}, distance: {path.Distance}, duration {path.Duration}");
// prints: "type: Complete, distance: 180.00m, duration 6.48s"

// Use path.Edges to get the actual path
yourClass.TraversePath(path.Edges);

```

### Graphs
```csharp
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Primitives;

// ...

// Create directed graph with node a and b, and a one-way direction from a to b
var nodeA = new Node(Position.Zero);
var nodeB = new Node(new Position(10, 10));

var traversalVelocity = Velocity.FromKilometersPerHour(100);
nodeA.Connect(nodeB, traversalVelocity);

var pathFinder = new PathFinder();
var path = pathFinder.FindPath(nodeA, nodeB, maximumVelocity: traversalVelocity);

Console.WriteLine($"type: {path.Type}, distance: {path.Distance}, duration {path.Duration}");
// prints: "type: Complete, distance: 14.14m, duration 0.51s"

// Use path.Edges to get the actual path
yourClass.TraversePath(path.Edges);
```

### Incomplete paths
```csharp
// Create a graph with two nodes, but no connection between both nodes
var nodeA = new Node(Position.Zero);
var nodeB = new Node(new Position(10, 10));

var pathFinder = new PathFinder();
var path = pathFinder.FindPath(nodeA, nodeB, maximumVelocity: Velocity.FromKilometersPerHour(100));

Console.WriteLine($"type: {path.Type}, distance: {path.Distance}, duration {path.Duration}");
// prints: "type: ClosestApproach, distance: 0.00m, duration 0.00s"
```

## Details
This library uses a graph for all the underlying path finding. But for convenience there is also a grid class. Using this grid class you will never know that you are dealing with graphs, unless if you want too of course ;).

The goal of this library is to make the path finding extremely fast. Even for huge graphs, with 10.000 nodes and 40.000 edges, the algorithm will find a path in 10 miliseconds. For more details please check the [BenchmarkHistory.md](BenchmarkHistory.md) file.

This library is so fast because of the underlying data models we used. Especially the `MinHeap` data structure makes sure that we can efficiently look up the best candidates to advance the path. Another advantage is that most of the calculations (like costs of edges) are precomputed when building the graph. Which saves time when searching for a path.

## Viewer
This code repository contains a WPF application which you can use to visualize the pathfinding algorithm. Right click nodes to remove them, it will automatically update the best path. You can also use the options in the graph menu to create different types of grids/graphs, and to randomize the traversal velocity of the edges. Remember: A* will always find the fastest path, not the shortest path!

![The viewer](viewer.png?raw=true "The viewer")


## Advanced techniques/Migrating from older versions
Previous versions, before 3.0, had a few features that are no longer available in 3.0. However you can mimic most of these features in a more efficient way, using the new graph-first representation.

### Corner cutting
If you disconnect a node from a grid at grid position (1,1), using `grid.DisconnectNode` you can also remove the diagonal connections from (0, 1) to (1, 0), (1, 0) to (2, 1), (2, 1) to (1, 2), (1, 2), to (0, 1) using the `grid.RemoveDiagonalConnectionsIntersectingWithNode` method. This mimics the behavior of preventing corner cutting, which was available in the path finder settings in previous versions, but is more efficient.

### Movement patterns
If you have a grid you can mimic certain movement patterns. For example creating a grid using `Grids.CreateGridWithLateralConnections` will give you a grid where every agent can move only up/down and left/right between cells (like a king in chess). You can also use `Grids.CreateGridWithDiagonalConnections` (your agent can move diagonally, like a bishop) or `Grid.CreateGridWithLateralAndDiagonalConnections` (your agent cann move both diagonally and laterally, like a queen). This method mimics movement patterns, but is more efficient.

### Different agent sizes
In a previous version (which was only available on GitHub, not on Nuget). You can define different agent shapes and sizes. Unfortunately this slowed down the path finding algorithm considerably. Consider having different graphs for different sized agents, where you manually block off corners where they can't fit. If you really want to support different agent shapes in one grid  I recommend using a different algorithm. For example the Explicit Corridor Map Model ([ECCM](https://www.staff.science.uu.nl/~gerae101/UU_crowd_simulation_publications_ecm.html)).