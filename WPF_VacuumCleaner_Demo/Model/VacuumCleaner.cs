using System;
using System.Collections.Generic;

namespace WPF_VacuumCleaner_Demo.Model
{
    public class VacuumCleaner
    {
        public int X { get; set; }
        public int Y { get; set; }
        public House House { get; set; }
        public List<Tuple<int, int>> Moves { get; private set; }

        public VacuumCleaner(int x, int y, House house)
        {
            X = x;
            Y = y;
            House = house;
            Moves = new();
        }

        public void Run()
        {
            do
            {
                do
                {
                    #region lines for visualization
                    House.ResetCheckedTiles();
                    Moves.Add(new(X, Y));
                    #endregion
                    CleanTile(); // at starting point

                } while (TryMoveToNeighborTile());

            } while (TryFindDirtyTile(X, Y));

            #region lines for visualization
            House.ResetCheckedTiles();
            #endregion
        }

        /// <summary>
        /// If any of the neighbor tiles is not wall or a clean tile, then move there and return true.
        /// Otherwise return false.
        /// </summary>
        /// <returns></returns>
        public bool TryMoveToNeighborTile()
        {
            if (X + 1 < House.Width && House.GetTile(X + 1, Y) == Tile.Dirty)
            {
                X++;
                return true;
            }
            else if (X - 1 >= 0 && House.GetTile(X - 1, Y) == Tile.Dirty)
            {
                X--;
                return true;
            }
            else if (Y - 1 >= 0 && House.GetTile(X, Y - 1) == Tile.Dirty)
            {
                Y--;
                return true;
            }
            else if (Y + 1 < House.Width && House.GetTile(X, Y + 1) == Tile.Dirty)
            {
                Y++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If able to find the nearest dirty tile, then move there and return true.
        /// Otherwise return false.
        /// </summary>
        /// <returns></returns>
        public bool TryFindDirtyTile(int x, int y)
        {
            if ((x < 0) || (x >= House.Width)) return false;
            if ((y < 0) || (y >= House.Height)) return false;

            if (House.GetTile(x, y) == Tile.Dirty)
            {
                SetPosition(x, y);
                return true;
            }
            else if (House.GetTile(x, y) == Tile.Clean)
            {
                House.SetTile(x, y, Tile.CleanAndChecked); // marking as visited
            }
            else if (House.GetTile(x, y) == Tile.Wall || House.GetTile(x, y) == Tile.CleanAndChecked)
            {
                return false;
            }

            return
                TryFindDirtyTile(x, y + 1)
                || TryFindDirtyTile(x + 1, y)
                || TryFindDirtyTile(x, y - 1)
                || TryFindDirtyTile(x - 1, y);
        }

        public void CleanTile()
        {
            House.SetTile(X, Y, Tile.Clean);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
