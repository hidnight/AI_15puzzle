using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_15puzzle {
    public class Position : IComparable, IEquatable<Position> {
        private int[,] board;
        private int score;
        public Position(int[,] board) {
            this.board = board;
            score = evaluate();
        }

        public int CompareTo(object obj) {
            return score.CompareTo(((Position)obj).Score);
        }

        public bool Equals(Position p) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (p.board[i, j] != board[i, j]) {
                        return false;
                    }
                }
            }
            return true;
        }

        public int Score {
            get {
                return score;
            }
            set {
                score = value;
            }
        }
        public int[,] Board {
            get {
                return board;
            }
            set {
                board = value;
            }
        }
        // Возвращает сумму квадратов Манхэттенских расстояний
        private int evaluate() {
            int score = 0;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    int add;
                    if (board[i, j] == 0) {
                        add = (Math.Abs(3 - i) + Math.Abs(3 - j));
                        score += add * add;
                        continue;
                    }
                    add = (Math.Abs((board[i, j] - 1) / 4 - i) + Math.Abs((board[i, j] - 1) % 4 - j));
                    score += add * add;
                }
            }
            return score;
        }

        public List<Position> GetChildren() {
            List<Position> children = new List<Position>();
            int zeroi = -1, zeroj = -1;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (board[i, j] == 0) {
                        zeroi = i;
                        zeroj = j;
                    }
                }
            }

            // перемещение 0
            // вверх
            if (zeroi > 0) {
                board[zeroi, zeroj] = board[zeroi - 1, zeroj];
                board[zeroi - 1, zeroj] = 0;
                children.Add(new Position(board));
                board[zeroi - 1, zeroj] = board[zeroi, zeroj];
                board[zeroi, zeroj] = 0;
            }
            // вниз
            if (zeroi < 3) {
                board[zeroi, zeroj] = board[zeroi + 1, zeroj];
                board[zeroi + 1, zeroj] = 0;
                children.Add(new Position(board));
                board[zeroi + 1, zeroj] = board[zeroi, zeroj];
                board[zeroi, zeroj] = 0;
            }
            // влево
            if (zeroj > 0) {
                board[zeroi, zeroj] = board[zeroi, zeroj - 1];
                board[zeroi, zeroj - 1] = 0;
                children.Add(new Position(board));
                board[zeroi, zeroj - 1] = board[zeroi, zeroj];
                board[zeroi, zeroj] = 0;
            }
            // вправо
            if (zeroj < 3) {
                board[zeroi, zeroj] = board[zeroi, zeroj + 1];
                board[zeroi, zeroj + 1] = 0;
                children.Add(new Position(board));
                board[zeroi, zeroj + 1] = board[zeroi, zeroj];
                board[zeroi, zeroj] = 0;
            }
            return children;
        }
    }
}
