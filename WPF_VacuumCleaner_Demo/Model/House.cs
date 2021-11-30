namespace WPF_VacuumCleaner_Demo.Model
{
    public enum Tile
    {
        Dirty,
        Clean,
        CleanAndChecked,
        Wall
    }

    public class House
    {
        public int Width;
        public int Height;
        private readonly Tile[,] _tiles;

        public House(int width, int height)
        {
            Width = width;
            Height = height;

            _tiles = new Tile[width, height];
        }

        public Tile GetTile(int x, int y)
        {
            return _tiles[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            _tiles[x, y] = tile;
        }

        public void ResetCheckedTiles()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (GetTile(i, j) == Tile.CleanAndChecked)
                    {
                        SetTile(i, j, Tile.Clean);
                    }
                }
            }
        }
    }
}
