using WPF_VacuumCleaner_Demo.Persistence;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPF_VacuumCleaner_Demo.Model;
using Microsoft.Win32;

namespace WPF_VacuumCleaner_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void Run(string fileName = null)
        {
            House house;
            if (fileName != null)
            {
                house = VacuumCleanerFileDataAccess.LoadAsync(fileName);
            }
            else
            {
                house = new(8, 9);
                CreateWalls(house);
            }

            VacuumCleaner vacuum = new(1, 1, house);

            RefreshCanvas(house, vacuum);

            // Vacuum Cleaner Main Program

            vacuum.Run();

            RefreshCanvas(house, vacuum);
        }

        private void RefreshCanvas(House house, VacuumCleaner vacuumCleaner)
        {
            Canvas1.Children.Clear();

            const int tileSize = 20;
            for (int i = 0; i < house.Width; i++)
            {
                for (int j = 0; j < house.Height; j++)
                {
                    var rect = new Rectangle()
                    {
                        Stroke = Brushes.LightBlue,
                        StrokeThickness = 1,
                        Width = tileSize,
                        Height = tileSize
                    };

                    switch (house.GetTile(i, j))
                    {
                        case Tile.Unclean:
                            rect.Fill = Brushes.White;
                            break;
                        case Tile.Clean:
                            rect.Fill = Brushes.LightGreen;
                            break;
                        case Tile.CleanAndChecked:
                            rect.Fill = Brushes.LightCyan;
                            break;
                        case Tile.Wall:
                            rect.Fill = Brushes.DarkGray;
                            break;
                        default:
                            break;
                    }

                    Canvas.SetLeft(rect, i * tileSize);
                    Canvas.SetTop(rect, ((house.Height - 1) * tileSize) - j * tileSize);
                    Canvas1.Children.Add(rect);
                }
            }

            for (int i = 0; i < vacuumCleaner.Moves.Count - 1; i++)
            {
                var lineStart = vacuumCleaner.Moves[i];
                var lineEnd = vacuumCleaner.Moves[i + 1];

                var line = new Line()
                {
                    Stroke = Brushes.Orange,
                    StrokeThickness = 1,
                    X1 = lineStart.Item1 * tileSize + tileSize / 2,
                    Y1 = house.Height * tileSize - (lineStart.Item2 * tileSize + tileSize / 2),
                    X2 = lineEnd.Item1 * tileSize + tileSize / 2,
                    Y2 = house.Height * tileSize - (lineEnd.Item2 * tileSize + tileSize / 2)
                };

                Canvas1.Children.Add(line);
            }
        }

        private static void CreateWalls(House house)
        {
            house.SetTile(0, 0, Tile.Wall);
            house.SetTile(0, 1, Tile.Wall);
            house.SetTile(0, 2, Tile.Wall);
            house.SetTile(0, 3, Tile.Wall);
            house.SetTile(0, 4, Tile.Wall);
            house.SetTile(0, 5, Tile.Wall);

            house.SetTile(5, 0, Tile.Wall);
            house.SetTile(5, 1, Tile.Wall);
            house.SetTile(5, 2, Tile.Wall);
            house.SetTile(5, 3, Tile.Wall);
            house.SetTile(5, 4, Tile.Wall);
            house.SetTile(5, 5, Tile.Wall);

            house.SetTile(1, 0, Tile.Wall);
            house.SetTile(2, 0, Tile.Wall);
            house.SetTile(3, 0, Tile.Wall);
            house.SetTile(4, 0, Tile.Wall);

            house.SetTile(1, 5, Tile.Wall);
            house.SetTile(2, 5, Tile.Wall);
            house.SetTile(3, 5, Tile.Wall);
            house.SetTile(4, 5, Tile.Wall);

            house.SetTile(3, 3, Tile.Wall); // chimney
            house.SetTile(3, 4, Tile.Wall); // chimney
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                Run(openFileDialog.FileName);
            }
        }
    }
}
