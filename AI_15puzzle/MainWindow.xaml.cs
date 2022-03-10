using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_15puzzle {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int[] tiles;
        private int[,] board;
        private Puzzle puzzle;
        public MainWindow() {
            InitializeComponent();
            board = new int[4, 4];
            shuffleTiles();
            puzzle = new Puzzle(tiles);
        }

        private void shuffleTiles() {
            Random rnd = new Random();
            tiles = Enumerable.Range(0, 16).OrderBy(c => rnd.Next()).ToArray();
            foreach (Button button in gridButtons.Children) {
                int index = int.Parse((string)button.Tag) - 1;
                int content = tiles[index];
                button.Content = (content == 0) ? "" : content.ToString();
                board[index / 4, index % 4] = content;
            }
        }

        private void newGame_Click(object sender, RoutedEventArgs e) {
            shuffleTiles();
            puzzle = new Puzzle(tiles);
            puzzle.computeSolution();
        }
    }
}
