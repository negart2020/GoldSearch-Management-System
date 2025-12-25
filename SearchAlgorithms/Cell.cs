namespace GoldSearchWebApp
{
    public class Cell
    {
        public int X { get; set; } // Row
        public int Y { get; set; } // Column
        public bool IsPlayer { get; set; }
        public bool IsGold { get; set; }
        public bool IsObstacle { get; set; }

        // For pathfinding algorithms (e.g., UCS, A*)
        public int Cost { get; set; } = 1; // Default cost for moving to this cell
        public Cell Parent { get; set; } // To reconstruct the path

        // For A* algorithm
        public int GCost { get; set; } // Cost from start node to current node
        public int HCost { get; set; } // Heuristic cost from current node to end node (estimated)
        public int FCost => GCost + HCost; // Total cost (G + H)

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   X == cell.X &&
                   Y == cell.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}