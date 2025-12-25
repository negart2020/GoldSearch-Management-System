namespace GoldSearchWebApp
{
    public class Grid
    {
        public Cell[,] Cells { get; set; }
        public int Size { get; set; }
        public Cell PlayerStart { get; set; }
        public Cell GoldLocation { get; set; }

        public Grid(int size)
        {
            Size = size;
            Cells = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = new Cell(i, j);
                }
            }
        }

        public bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        public void PlacePlayer(int x, int y)
        {
            if (IsValidCoordinate(x, y))
            {
                Cells[x, y].IsPlayer = true;
                PlayerStart = Cells[x, y];
            }
        }

        public void PlaceGold(int x, int y)
        {
            if (IsValidCoordinate(x, y))
            {
                Cells[x, y].IsGold = true;
                GoldLocation = Cells[x, y];
            }
        }

        public void PlaceObstacle(int x, int y)
        {
            if (IsValidCoordinate(x, y))
            {
                Cells[x, y].IsObstacle = true;
            }
        }
    }
}