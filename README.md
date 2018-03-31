# Roy-T.AStar
A fast 2D path finding library based on the A* algorithm for `.NETStandard 1.0` and `.Net 4.5` and higher. This library has no external dependencies. The library is licensed under the MIT license, see the `LICENSE` file for more details.

You can directly add this library to your project using [NuGet](https://www.nuget.org/packages/RoyT.AStar/):

```
Install-Package RoyT.AStar
```

For more information about the A* path finding algorithm and this library, please visit my blog at http://roy-t.nl.

There is also a documented [version history](versions.md).

## Why choose this library?
- Its very fast
- It generates lowest cost, visually appealing, paths using the A* algorithm
- Its flexible, you can define how your agent moves and this library will accomodate these restrictions
- Its easy to use, you can generate a path with two lines of code
- It has zero (0) external dependencies
- It works on both `.Net Core` and `.Net 4.5` and higher by targetting `.Net standard 1.0`


## Tutorial

Below I give short code-first example. If you are more interested in textual explanation you can find it right after the next section.

### Usage example in code
You can easily search for the lowest cost path for an agent that can move in all directions.

```csharp
using RoyT.AStar;

// Create a new grid and let each cell have a default traversal cost of 1.0
var grid = new Grid(100, 100, 1.0f);

// Block some cells (for example walls)
grid.BlockCell(new Position(5, 5))

// Make other cells harder to traverse (for example water)
grid.SetCellCost(new Position(6, 5), 3.0f);

// And finally start the search for the shortest path form start to end
Position[] path = grid.GetPath(new Position(0, 0), new Position(99, 99));

```
It is also posssible to define the agent's movement pattern.

```csharp
// Use one of the built-in ranges of motion
var path = grid.GetPath(new Position(0, 0), new Position(99, 99), MovementPatterns.DiagonalOnly);

// Or define the movement pattern of an agent yourself
// For example, here is an agent that can only move left and up
var movementPattern = new[] {new Offset(-1, 0), new Offset(0, -1)};
var path = grid.GetPath(new Position(0, 0), new Position(99, 99), movementPattern);

```

### Usage in text
Your agent might be able to move fluently through a world with hills and water but that representation is too complex for the A* algorithm. 
So the first thing you need to do is to is to create an abstract representation of your world that is simple enough for the path finding algorithm to understand.
In this library we use a grid to represent the traversable, and intraversable, space in your world. 

You can instantiate a grid using the `Grid` class. If you have a world that is a 100 by a 100 meters large, and you
create a grid of 100x100, each cell will represent a patch of land of 1x1 meters. 

Experiment with the size of the grid, a larger grid
will give you more fine grained control but it will also make the path finding algorithm slower.

Once you have a grid you need to configure which cells represent obstacles. Some obstacles, like a high wall, are intraversable. Use the `BlockCell` method on your grid to prevent the path finding algorithm from planning paths through that cell.
Other obstacles, like dense shrubbery, take more time to traverse. In that case give the cell a higher cost using the `SetCellCost` method.

Once you've configured your grid its time to start planning paths. Using the `GetPath` method you can immidately search for a path between two cells for an agent that can move in all directions. 
You can also plan paths for agents that are more limited in their movent using the overload of `GetPath` that takes a `movementPattern` parameter. In that case you can either select one of the predefined ranges of motion from the `MovementPatterns` class or you can configure yourself what kind of steps an agent can make.

You can define your own patterns for your agents. Below I have defined the movement pattern for an agent that can only move diagonally. (This movement pattern is included in the library as `MovementPatterns.DiagonalOnly`).

```csharp
var diagonalOnlyMovementPattern = new[] {
    new Offset(-1, -1), new Offset(1, -1), , new Offset(1, 1), , new Offset(-1, 1)
};
```


## Viewer
This repository also contains a viewer which you can use to build worlds and visualize paths.
In debug builds you can even replay the decision making process to get a feeling of what is going on.

![The viewer](viewer.png?raw=true "The viewer")
 
Click a cell in the editor to change it properties. A new path will be generated automatically.
The buttons and slider at the bottom of the window let you control the replay features.

Each cell is color coded

- White: a normal cell
- Light green: the start cell
- Dark green: the current cell, or end cell
- Black a blocked (intraversable) cell
- Gray: a cell that has been processed, *is closed*
- Orange: a cell that still needs to be processed, *is open*
- Purple: the cell that is currently being processed
- Blue: a cell on, what currently is believed to be, the shortest path


## Implementation details
While making this library I was mostly concerned with performance (how long does it take to find a path) and ease of use.
I use a custom `MinHeap` data structure to keep track of the best candidates for the shortest path. I've experimented with other data structures, like the standard `SortedSet` but they were consistently slower. 
Inside the A\* algorithm itself I use flat arrays to store the path and cost information. I use the Manhattan distance heuristic because its cheap to compute and is an admissible heuristic for all predefined movement patterns.

While micro-optimizing the code I've used the handy [BenchMark.Net](https://github.com/dotnet/BenchmarkDotNet) library to see if my changes had any effect. The benchmark suite is included in the source code here. So if you would like to try to make this implemention faster you can use the same benchmark and performance metrics I did.