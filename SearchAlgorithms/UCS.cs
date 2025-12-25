using System.Collections.Generic;
using System.Linq;

namespace GoldSearchWebApp
{
    public static class UCS
    {
        public static (List<Cell>, int) FindPath(Grid grid, Cell start, Cell target)
        {
            if (start == null || target == null || start.IsObstacle || target.IsObstacle)
                return (null, 0);

            // Priority Queue: Stores cells to visit, ordered by lowest cost
            // A simple List + OrderBy is used here for simplicity. For large grids, a proper
            // min-heap based priority queue would be more efficient.
            List<Cell> openSet = new List<Cell>();
            HashSet<Cell> closedSet = new HashSet<Cell>(); // Cells already evaluated

            // Dictionary to store the cost from start to current cell
            Dictionary<Cell, int> gScore = new Dictionary<Cell, int>();

            // Dictionary to store the path (how we got to a cell)
            Dictionary<Cell, Cell> parentMap = new Dictionary<Cell, Cell>();

            gScore[start] = 0;
            openSet.Add(start);

            int[] dx = { -1, 1, 0, 0 }; // Up, Down, Left, Right
            int[] dy = { 0, 0, -1, 1 };

            while (openSet.Any())
            {
                // Get the cell with the lowest gScore from openSet
                Cell current = openSet.OrderBy(cell => gScore.ContainsKey(cell) ? gScore[cell] : int.MaxValue).First();

                if (current.Equals(target))
                {
                    return ReconstructPath(parentMap, start, target);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (grid.IsValidCoordinate(newX, newY))
                    {
                        Cell neighbor = grid.Cells[newX, newY];

                        if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                        {
                            continue; // Skip obstacles and already evaluated cells
                        }

                        // The cost from start to neighbor through current
                        int tentative_gScore = gScore[current] + neighbor.Cost;

                        if (!openSet.Contains(neighbor) || tentative_gScore < gScore[neighbor])
                        {
                            parentMap[neighbor] = current;
                            gScore[neighbor] = tentative_gScore;

                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }

            return (null, 0); // No path found
        }

        private static (List<Cell>, int) ReconstructPath(Dictionary<Cell, Cell> parentMap, Cell start, Cell target)
        {
            List<Cell> path = new List<Cell>();
            Cell current = target;
            int steps = 0;
            while (current != null && !current.Equals(start))
            {
                path.Add(current);
                steps++; // Count steps (edges)
                if (parentMap.ContainsKey(current))
                {
                    current = parentMap[current];
                }
                else
                {
                    return (null, 0);
                }
            }
            path.Add(start); // Add the starting cell
            path.Reverse(); // Path is built backwards, so reverse it
            return (path, steps);
        }
    }
}