using System;
using System.IO;
using WPF_VacuumCleaner_Demo.Model;

namespace WPF_VacuumCleaner_Demo.Persistence
{
    /// <summary>
    /// RunAway fájlkezelő típusa.
    /// </summary>
    public class VacuumCleanerFileDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public static House LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new(path)) // fájl megnyitása
                {
                    string line = reader.ReadLine();
                    string[] numbers = line.Split(' ');
                    int width = int.Parse(numbers[0]);
                    int height = int.Parse(numbers[1]);
                    House house = new(width, height);

                    for (int j = house.Height - 1; j >= 0; j--)
                    {
                        line = reader.ReadLine();
                        numbers = line.Split(' ');

                        for (int i = 0; i < house.Width; i++)
                        {
                            switch (int.Parse(numbers[i]))
                            {
                                case 0:
                                    house.SetTile(i, j, Tile.Unclean);
                                    break;
                                case 1:
                                    house.SetTile(i, j, Tile.Wall);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    return house;
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
