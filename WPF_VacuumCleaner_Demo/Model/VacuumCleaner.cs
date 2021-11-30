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

        public void RunBFS()
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

            } while (TryFindDirtyTileBFS(X, Y));

            #region lines for visualization
            House.ResetCheckedTiles();
            #endregion
        }

        public void RunDFS()
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

            } while (TryFindDirtyTileDFS(X, Y));

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
        /// algo: DFS
        /// If able to find the nearest dirty tile, then move there and return true.
        /// Otherwise return false.
        /// </summary>
        /// <returns></returns>
        public bool TryFindDirtyTileDFS(int x, int y)
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
                TryFindDirtyTileDFS(x, y + 1)
                || TryFindDirtyTileDFS(x + 1, y)
                || TryFindDirtyTileDFS(x, y - 1)
                || TryFindDirtyTileDFS(x - 1, y);
        }

        /// <summary>
        /// algo: BFS
        /// If able to find the nearest dirty tile, then move there and return true.
        /// Otherwise return false.
        /// </summary>
        /// <returns></returns>
        public bool TryFindDirtyTileBFS(int x, int y)
        {
            Queue<Tuple<int, int>> TilesToCheck = new();
            Queue<Tuple<int, int>> TilesToCheckNext;
            TilesToCheck.Enqueue(new(x, y)); // start

            do
            {
                TilesToCheckNext = new();

                do
                {
                    Tuple<int, int> tile = TilesToCheck.Dequeue();
                    x = tile.Item1;
                    y = tile.Item2;

                    if (House.GetTile(x, y) == Tile.Dirty)
                    {
                        SetPosition(x, y);
                        return true;
                    }
                    else if (House.GetTile(x, y) == Tile.Clean)
                    {
                        House.SetTile(x, y, Tile.CleanAndChecked); // marking as visited

                        // look around
                        if (x + 1 < House.Width) // RIGHT
                        {
                            TilesToCheckNext.Enqueue(new(x + 1, y));
                        }
                        if (y + 1 < House.Width) // UP
                        {
                            TilesToCheckNext.Enqueue(new(x, y + 1));
                        }
                        if (x - 1 >= 0) // LEFT
                        {
                            TilesToCheckNext.Enqueue(new(x - 1, y));
                        }
                        if (y - 1 >= 0) // DOWN
                        {
                            TilesToCheckNext.Enqueue(new(x, y - 1));
                        }
                    }

                } while (TilesToCheck.Count > 0);

                TilesToCheck = TilesToCheckNext;

            } while (TilesToCheckNext.Count > 0);

            return false;
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
