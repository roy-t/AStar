# Roy-T.AStar
A 2D path finding library based on the A* algorithm for `.NETStandard 1.0` and `.Net 4.5` and higher. This library has no external dependencies.
## Tutorial

### Basic Usage
```csharp
using RoyT.AStar;

// Create a new grid and let each cell have a default traversal cost of 1.0
var grid = new Grid(100, 100, 1.0f);

// Block some cells (for example walls)
grid.BlockCell(new Position(5, 5))

// Make other cells harder to traverse (for example water)
grid.SetCellCost(new Position(6, 5), 3.0f);

// And finally start the search for the shortest path form start to end
IList<Position> path = grid.GetPath(new Position(0, 0), new Position(99, 99));

```


### Defining the agent's range of motion

```csharp
// Use one of the built in ranges of motion
var path = grid.GetPath(new Position(0, 0), new Position(99, 99), RangeOfMotion.DiagonalOnly);

// Or define the range of motion of an agent yourself
// For example, here is an agent that can only move left and up
var rom = new[] {new Offset(-1, 0), new Offset(0, -1)};
var path = grid.GetPath(new Position(0, 0), new Position(99, 99), rom);
```


## Implementation details
This implementation 

