using System;
using System.Collections.Generic;

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
            solution = null;
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
            solution = null;
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
                    if (seq[j] == 0) {
                        continue;
                    }
                    if (seq[i] > seq[j])
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
                    if (board[i1, j1] == 0) {
                        N += i1 + 1;
                        continue;
                    }
                    if (board[i1, j1] == 1) {
                        continue;
                    }
                    for (int i2 = i1; i2 < 4; i2++) {
                        for (int j2 = (i1 == i2) ? j1 : 0; j2 < 4; j2++) {
                            if (board[i2, j2] == 0)
                                continue;
                            if (board[i1, j1] > board[i2, j2]) {
                                N++;
                            }
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
                    if (x.Equals(finalPosition)) {
                        closed.Add(x);
                        solution = ConstructSolution(closed);
                        solved = true;
                        return true;
                    }
                    List<Position> children = x.GetChildren();
                    foreach (Position child in children) {
                        if (!open.Contains(child) && !closed.Contains(child)) {
                            open.Enqueue(child);
                        }
                    }
                    closed.Add(x);
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

        public bool Solvable {
            get {
                return solvable;
            }
        }

        public bool Solved {
            get {
                return solved;
            }
        }

        private List<Position> ConstructSolution(List<Position> closed) {
            List<Position> result = new List<Position>();
            int i = closed.Count - 1;
            while (i != 0) {
                int j = i - 1;
                List<Position> children = closed[i].GetChildren();
                while (!children.Contains(closed[j])) {
                    j--;
                }
                result.Add(closed[i]);
                i = j;
            }
            // добавить начальное состояние
            //result.Add(closed[i]);
            result.Reverse();
            return result;
        }
    }
}
