using System;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AI_15puzzle {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int[,] board;
        private Puzzle puzzle;
        private int delay;
        public MainWindow() {
            InitializeComponent();
            newGame_Click(null, null);
            delay = 500;
        }

        private int[,] ShuffleTiles() {
            infoTextBox.Text = "";
            showSolution.IsEnabled = false;
            Random rnd = new Random();
            int[] tiles;
            #if DEBUG
                tiles = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 13, 14, 15 };
            #else
                tiles = Enumerable.Range(0, 16).OrderBy(c => rnd.Next()).ToArray();
            #endif
            int[,] board = new int[4, 4];
            foreach (Button button in gridButtons.Children) {
                int index = int.Parse((string)button.Tag) - 1;
                int content = tiles[index];
                button.Visibility = Visibility.Visible;
                button.Content = content.ToString();
                if (content == 0)
                    button.Visibility = Visibility.Hidden;
                board[index / 4, index % 4] = content;
            }
            return board;
        }

        private void newGame_Click(object sender, RoutedEventArgs e) {
            board = ShuffleTiles();
            puzzle = new Puzzle(board);
            if (puzzle.Solvable) {
                infoTextBox.Text = "Решение существует";
                findSolution.IsEnabled = true;
            } else {
                infoTextBox.Text = "Решения не существует";
                findSolution.IsEnabled = false;
            }
        }

        private void tile_Click(object sender, RoutedEventArgs e) {
            Button neighbour = null;
            int index = int.Parse((string)((Button)sender).Tag) - 1;
            int i = index / 4, j = index % 4;
            bool moved = false;
            // перемещение
            // вверх
            if (i > 0 && board[i - 1, j] == 0) {
                board[i - 1, j] = board[i, j];
                board[i, j] = 0;
                neighbour = (Button)gridButtons.Children[index - 4];
                moved = true;
            }
            // вниз
            if (!moved && i < 3 && board[i + 1, j] == 0) {
                board[i + 1, j] = board[i, j];
                board[i, j] = 0;
                neighbour = (Button)gridButtons.Children[index + 4];
                moved = true;
            }
            // влево
            if (!moved && j > 0 && board[i, j - 1] == 0) {
                board[i, j - 1] = board[i, j];
                board[i, j] = 0;
                neighbour = (Button)gridButtons.Children[index - 1];
                moved = true;
            }
            // вправо
            if (!moved && j < 3 && board[i, j + 1] == 0) {
                board[i, j + 1] = board[i, j];
                board[i, j] = 0;
                neighbour = (Button)gridButtons.Children[index + 1];
                moved = true;
            }
            if (moved) {
                neighbour.Content = ((Button)sender).Content;
                ((Button)sender).Content = 0.ToString();
                ((Button)sender).Visibility = Visibility.Hidden;
                neighbour.Visibility = Visibility.Visible;
            }
        }

        private void UpdateBoard() {
            foreach (Button button in gridButtons.Children) {
                button.Visibility = Visibility.Visible;
                int index = int.Parse((string)button.Tag) - 1;
                button.Content = board[index / 4, index % 4].ToString();
                if (board[index / 4, index % 4] == 0)
                    button.Visibility = Visibility.Hidden;
            }
        }

        async private void findSolution_Click(object sender, RoutedEventArgs e) {
            puzzle = new Puzzle(board);
            if (!puzzle.Solvable) {
                infoTextBox.Text = "Решения не существует";
            } else {
                newGame.IsEnabled = false;
                findSolution.IsEnabled = false;
                gridButtons.IsEnabled = false;
                infoTextBox.Text = "Начат поиск решения. Ждите";
                await Task.Run(puzzle.computeSolution);
                SystemSounds.Asterisk.Play();
                infoTextBox.Text = "Решение найдено";
                showSolution.IsEnabled = true;
                newGame.IsEnabled = true;
                findSolution.IsEnabled = true;
                gridButtons.IsEnabled = true;
            }
        }

        async private void showSolution_Click(object sender, RoutedEventArgs e) {
            newGame.IsEnabled = false;
            findSolution.IsEnabled = false;
            gridButtons.IsEnabled = false;
            showSolution.IsEnabled = false;
            foreach (Position p in puzzle.Solution) {
                board = p.Board;
                UpdateBoard();
                await Task.Delay(delay);
            }
            newGame.IsEnabled = true;
            findSolution.IsEnabled = true;
            gridButtons.IsEnabled = true;
        }
    }
}
