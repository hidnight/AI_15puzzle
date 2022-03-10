using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_15puzzle {
    public class Puzzle {
        private Position initialPosition;
        private Position finalPosition;
        private List<Position> solution;
        private bool solvable;
        private bool solved;

        public Puzzle(int[] seq) {
            int[,] initialBoard;
            int[,] finalBoard;
            if (seq.Length != 16) {
                throw new ArgumentException("sequence must contain 16 elements");
            }
            initialBoard = new int[4, 4];
            for (int i = 0; i < 16; i++) {
                initialBoard[i/4,i%4] = seq[i];
            }
            initialPosition = new Position(initialBoard);
            checkSolvability(seq);
            finalBoard = new int[,]
                {
                    {01,02,03,04},
                    {05,06,07,08},
                    {09,10,11,12},
                    {13,14,15,00}
                };
            finalPosition = new Position(finalBoard);
            solution = new List<Position>();
            solved = false;
        }

        public Puzzle(int[,] initialBoard) {
            int[,] finalBoard;
            if (initialBoard.Length != 16) {
                throw new ArgumentException("sequence must contain 16 elements");
            }
            initialPosition = new Position(initialBoard);
            checkSolvability(initialBoard);
            finalBoard = new int[,]
                {
                    {01,02,03,04},
                    {05,06,07,08},
                    {09,10,11,12},
                    {13,14,15,00}
                };
            finalPosition = new Position(finalBoard);
            solution = new List<Position>();
        }

        // https://ru.wikipedia.org/wiki/Игра_в_15#Математическое_описание
        private void checkSolvability(int[] seq) {
            int N = 0;
            for (int i = 0; i < 15; i++) {
                // ряд пустой клетки
                if (seq[i] == 0) {
                    N += (i / 4) + 1;
                    continue;
                }
                if (seq[i] == 1) continue;
                for (int j = i + 1; j < 16; j++) {
                    if (seq[j] > seq[i])
                        N++; 
                }
            }
            solvable = N % 2 == 0;
        }

        private void checkSolvability(int[,] board) {
            int N = 0;
            for (int i1 = 0; i1 < 4; i1++) {
                for (int j1 = 0; j1 < 4; j1++) {
                    // ряд пустой клетки
                    if (board[i1,j1] == 0) {
                        N += i1 + 1;
                        continue;
                    }
                    if (board[i1, j1] == 1) {
                        continue;
                    }
                    for (int i2 = i1; i2 < 4; i2++) {
                        for (int j2 = j1; j2 < 4; j2++) {
                            if (board[i2, j2] > board[i1, j1])
                                N++;
                        }
                    }
                }
            }
            solvable = N % 2 == 0;
        }

        public bool computeSolution() {
            if (solvable) {
                PriorityQueue<Position> open = new PriorityQueue<Position>();
                open.Enqueue(initialPosition);
                List<Position> closed = new List<Position>();
                while (true) {
                    if (open.isEmpty()) {
                        return false;
                    }
                    Position x = open.Dequeue();
                    closed.Add(x);
                    if (x.Equals(finalPosition)) {
                        solution = closed;
                        return true;
                    }
                    List<Position> children = x.GetChildren();
                    foreach (Position child in children) {
                        if (!open.Contains(child) && !closed.Contains(child)) {
                            open.Enqueue(child);
                        }
                    }
                }
            }
            return solvable;
        }

        public List<Position> Solution {
            get {
                if (!solved)
                    throw new InvalidOperationException("Solution not found");
                return solution;
            }
        }

        public class Position : IComparable {
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
                return p.board == board;
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
                        if (board[i, j] == 0) {
                            score += (Math.Abs(3 - i) + Math.Abs(3 - j)) * (Math.Abs(3 - i) + Math.Abs(3 - j));
                            continue;
                        }
                        score += (Math.Abs(board[i,j] / 4 - i) + Math.Abs(board[i, j] % 4 - 1 - j)) * (Math.Abs(board[i, j] / 4 - i) + Math.Abs(board[i, j] % 4 - 1 - j));
                    }
                }
                return score;
            }

            public List<Position> GetChildren() {
                List<Position> children = new List<Position>();
                int zeroi = -1, zeroj = -1;
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        if (board[i,j] == 0) {
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
}
