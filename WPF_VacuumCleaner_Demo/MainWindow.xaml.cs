using WPF_VacuumCleaner_Demo.Persistence;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPF_VacuumCleaner_Demo.Model;
using System;

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

        private void RunBFS(string fileName, int x, int y)
        {
            House house;
            try
            {
                house = VacuumCleanerFileDataAccess.LoadAsync(fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't load file.");
                return;
            }

            VacuumCleaner vacuum = new(x, y, house);

            vacuum.RunBFS();

            RefreshCanvas(house, vacuum);
        }

        private void RunDFS(string fileName, int x, int y)
        {
            House house;
            try
            {
                house = VacuumCleanerFileDataAccess.LoadAsync(fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't load file.");
                return;
            }

            VacuumCleaner vacuum = new(x, y, house);

            vacuum.RunDFS();

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
                    Rectangle rect = new()
                    {
                        Stroke = Brushes.LightBlue,
                        StrokeThickness = 1,
                        Width = tileSize,
                        Height = tileSize
                    };

                    switch (house.GetTile(i, j))
                    {
                        case Tile.Dirty:
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

        private void btnSimpleRoomDFS_Click(object sender, RoutedEventArgs e)
        {
            RunDFS(Environment.CurrentDirectory + @"\Resources\Simple room.txt", 1, 1);
        }

        private void btnNarrowPathsDFS_Click(object sender, RoutedEventArgs e)
        {
            RunDFS(Environment.CurrentDirectory + @"\Resources\Narrow paths.txt", 1, 1);
        }

        private void btnComplexHouseDFS_Click(object sender, RoutedEventArgs e)
        {
            RunDFS(Environment.CurrentDirectory + @"\Resources\Complex house.txt", 1, 1);
        }

        private void btnSimpleRoomBFS_Click(object sender, RoutedEventArgs e)
        {
            RunBFS(Environment.CurrentDirectory + @"\Resources\Simple room.txt", 1, 1);
        }

        private void btnNarrowPathsBFS_Click(object sender, RoutedEventArgs e)
        {
            RunBFS(Environment.CurrentDirectory + @"\Resources\Narrow paths.txt", 1, 1);
        }

        private void btnComplexHouseBFS_Click(object sender, RoutedEventArgs e)
        {
            RunBFS(Environment.CurrentDirectory + @"\Resources\Complex house.txt", 1, 1);
        }
    }
}
