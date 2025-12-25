using System.Collections.Generic;
using System.Linq;

namespace GoldSearchWebApp
{
    public static class BFS
    {
        public static (List<Cell>, int) FindPath(Grid grid, Cell start, Cell target)
        {
            if (start == null || target == null || start.IsObstacle || target.IsObstacle)
                return (null, 0);

            Queue<Cell> queue = new Queue<Cell>();
            HashSet<Cell> visited = new HashSet<Cell>();
            Dictionary<Cell, Cell> parentMap = new Dictionary<Cell, Cell>();

            queue.Enqueue(start);
            visited.Add(start);

            int[] dx = { -1, 1, 0, 0 }; // Up, Down, Left, Right
            int[] dy = { 0, 0, -1, 1 };

            while (queue.Any())
            {
                Cell current = queue.Dequeue();

                if (current.Equals(target))
                {
                    return ReconstructPath(parentMap, start, target);
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (grid.IsValidCoordinate(newX, newY))
                    {
                        Cell neighbor = grid.Cells[newX, newY];
                        if (!visited.Contains(neighbor) && !neighbor.IsObstacle)
                        {
                            visited.Add(neighbor);
                            parentMap[neighbor] = current;
                            queue.Enqueue(neighbor);
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
            while (current != null && !current.Equals(start))
            {
                path.Add(current);
                if (parentMap.ContainsKey(current))
                {
                    current = parentMap[current];
                }
                else
                {
                    // This case should ideally not happen if a path was truly found
                    // but it's a safeguard against corrupted parentMap
                    return (null, 0);
                }
            }
            path.Add(start); // Add the starting cell
            path.Reverse(); // Path is built backwards, so reverse it
            return (path, path.Count - 1); // Steps = number of edges (nodes - 1)
        }
    }
}