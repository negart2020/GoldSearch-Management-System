using System.Collections.Generic;
using System.Linq;
using System; // For Math.Abs

namespace GoldSearchWebApp
{
    public static class AStar
    {
        public static (List<Cell>, int) FindPath(Grid grid, Cell start, Cell target)
        {
            if (start == null || target == null || start.IsObstacle || target.IsObstacle)
                return (null, 0);

            List<Cell> openSet = new List<Cell>();
            HashSet<Cell> closedSet = new HashSet<Cell>();

            start.GCost = 0;
            start.HCost = CalculateHeuristic(start, target);
            openSet.Add(start);

            int[] dx = { -1, 1, 0, 0 }; // Up, Down, Left, Right
            int[] dy = { 0, 0, -1, 1 };

            while (openSet.Any())
            {
                // Get the cell with the lowest FCost from the openSet
                Cell current = openSet.OrderBy(cell => cell.FCost).ThenBy(cell => cell.HCost).First(); // Break ties with HCost

                if (current.Equals(target))
                {
                    return ReconstructPath(current, start);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                for (int i = 0; i < 4; i++) // 4 directions (up, down, left, right)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (grid.IsValidCoordinate(newX, newY))
                    {
                        Cell neighbor = grid.Cells[newX, newY];

                        if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                        {
                            continue;
                        }

                        // Tentative gCost from start to neighbor through current
                        int tentative_gCost = current.GCost + neighbor.Cost;

                        if (!openSet.Contains(neighbor) || tentative_gCost < neighbor.GCost)
                        {
                            neighbor.Parent = current;
                            neighbor.GCost = tentative_gCost;
                            neighbor.HCost = CalculateHeuristic(neighbor, target);

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

        private static int CalculateHeuristic(Cell a, Cell b)
        {
            // Manhattan distance heuristic (sum of absolute differences in coordinates)
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static (List<Cell>, int) ReconstructPath(Cell endNode, Cell startNode)
        {
            List<Cell> path = new List<Cell>();
            Cell current = endNode;
            int steps = 0;

            while (current != null && !current.Equals(startNode))
            {
                path.Add(current);
                steps++;
                current = current.Parent;
            }
            path.Add(startNode); // Add the starting cell

            path.Reverse(); // Path is built backwards, so reverse it
            return (path, steps);
        }
    }
}