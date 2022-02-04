using Sudoku.PriorityElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class Solver
    {
        private const int MAX_CHANGES = 10;

        public Solver() { }

        public Board Solve(Board board)
        {
           Board newBoard = board.Clone();

            int changes;
            do
            {
                changes = 0;
                changes += StrategyOne(newBoard);
                changes += StrategyTwo(newBoard);
                changes += StrategyThree(newBoard);
            } while (changes != 0);

            if (newBoard.IsCompleted())
                return newBoard;
            return BacktrankingStrategy(newBoard);
        }

        private Board BacktrankingStrategy(Board board)
        {
            Board newBoard = board;
            List<int> possibleNumbersCoord = new List<int>();
            int min = 10;
            int[] coord = new int[] { 0, 0 };
            List<Quadrant> quadrants = board.Quadrants;

            for(int i = 0; i < quadrants.Count; i++)
            {
                List<int[]> emptyTiles = quadrants[i].EmptyTiles;
                for(int j = 0; j < emptyTiles.Count; j++)
                {
                    int[] tile = emptyTiles[j];
                    List<int> possibleNumbers = FindPossibleNumbers(board, tile[0], tile[1]);
                    if (possibleNumbers.Count < min)
                    {
                        possibleNumbersCoord = possibleNumbers;
                        min = possibleNumbers.Count;
                        coord = new int[] { tile[0], tile[1] };
                        if (min == 2)
                        {
                            j = emptyTiles.Count;
                            i = quadrants.Count;
                        }
                    }
                }
            }

            for(int i = 0; i < possibleNumbersCoord.Count && !newBoard.IsCompleted(); i++)
            {
                board.Set(possibleNumbersCoord[i], coord[0], coord[1]);
                newBoard = Solve(board);
            }

            return newBoard;
        }

        private int StrategyOne(Board board)
        {
            int totalChanges = 0;
            int changes;

            do
            {
                changes = 0;
                List<PriorityElement> elements = board.GetPriorityList();

                for (int i = 0; i < elements.Count && changes < MAX_CHANGES; i++)
                {
                    List<int[]> emptyTiles = elements[i].EmptyTiles;
                    for(int j = 0; j < emptyTiles.Count; j++)
                    {
                        int[] coord = emptyTiles[j];
                        TryToFillTile(board, coord[0], coord[1]);
                    }
                }

                totalChanges += changes;
            }
            while (!board.IsCompleted() && changes != 0);

            return totalChanges;
        }

        protected int TryToFillTile(Board board, int i, int j)
        {
            List<int> possibleNumbers = FindPossibleNumbers(board, i, j);
            if (possibleNumbers.Count == 1)
            {
                board.Set(possibleNumbers[0], i, j);
                return 1;
            }

            return 0;
        }

        public List<int> FindPossibleNumbers(Board board, int i, int j)
        {
            if (board.Get(i, j) != 0)
                return new List<int>();

            List<int> possibleNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Quadrant quadrant = board.GetQuadrant(i, j);

            FilterByQuadrant(board, possibleNumbers, quadrant);
            FilterByRow(board, possibleNumbers, i);
            FilterByColumn(board, possibleNumbers, j);

            return possibleNumbers;
        }

        private void FilterByQuadrant(Board board, List<int> possibleNumbers, Quadrant quadrant)
        {
            for (int i = quadrant.IFrom; i < quadrant.ITo; i++)
                for (int j = quadrant.JFrom; j < quadrant.JTo; j++)
                    possibleNumbers.Remove(board.Get(i, j));
        }

        private void FilterByRow(Board board, List<int> possibleNumbers, int row)
        {
            for (int j = 0; j < 9; j++)
                possibleNumbers.Remove(board.Get(row, j));
        }

        private void FilterByColumn(Board board, List<int> possibleNumbers, int column)
        {
            for (int i = 0; i < 9; i++)
                possibleNumbers.Remove(board.Get(i, column));
        }

        private int StrategyTwo(Board board)
        {
            int changes = 0;
            List<Quadrant> quadrants = board.Quadrants;

            for(int i = 0; i < quadrants.Count && changes < MAX_CHANGES; i++)
            {
                Quadrant quadrant = quadrants[i];
                List<int> missingNumbers = quadrant.MissingNumbers;

                for(int j = 0; j < missingNumbers.Count; j++)
                    changes += TryToLocateNumberInQuadrant(board, missingNumbers[j], quadrant);
            }

            return changes;
        }

        private int TryToLocateNumberInQuadrant(Board board, int number, Quadrant quadrant)
        {
            List<int[]> possibleTiles = CloneList(quadrant.EmptyTiles);

            for(int i = quadrant.IFrom; i < quadrant.ITo; i++) 
                for(int j = 0; j < 9; j++)
                {
                    if (board.Get(i, j) == number)
                        RemovePossibleTilesWithRow(possibleTiles, i);
                }

            for (int i = 0; i < 9; i++)
                for (int j = quadrant.IFrom; j < quadrant.ITo; j++)
                {
                    if (board.Get(i, j) == number)
                        RemovePossibleTilesWithColumn(possibleTiles, j);
                }

            if (possibleTiles.Count == 1)
            {
                board.Set(number, possibleTiles[0][0], possibleTiles[0][1]);
                return 1;
            }

            return 0;
        }

        private void RemovePossibleTilesWithRow(List<int[]> tiles, int row)
        {
            for(int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i][0] == row)
                {
                    tiles.RemoveAt(i);
                    i--;
                }
            }
        }

        private void RemovePossibleTilesWithColumn(List<int[]> tiles, int column)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i][1] == column)
                {
                    tiles.RemoveAt(i);
                    i--;
                }
            }
        }

        private List<int[]> CloneList(List<int[]> list)
        {
            List<int[]> newList = new List<int[]>();

            foreach (int[] i in list)
                newList.Add(new int[] { i[0], i[1] });

            return newList;
        }


        private int StrategyThree(Board board)
        {
            int changes = 0;

            for(int j = 0; j < 9; j++)
                changes+= TryToLocateNumberInColumn(board, j);

            for (int i = 0; i < 9; i++)
                changes+= TryToLocateNumberInRow(board, i);

            return changes;
        }

        private int TryToLocateNumberInRow(Board board, int row)
        {
            int changes = 0;

            List<int>[] possibleNumbers = new List<int>[9];

            for(int j = 0; j < 9; j++)
                possibleNumbers[j] = FindPossibleNumbers(board, row, j);

            DeleteDupilcates(possibleNumbers);

            for (int j = 0; j < 9; j++)
                if (possibleNumbers[j].Count == 1)
                {
                    board.Set(possibleNumbers[j][0], row, j);
                    changes++;
                }

            return changes;
        }

        private int TryToLocateNumberInColumn(Board board, int column)
        {
            int changes = 0;

            List<int>[] possibleNumbers = new List<int>[9];

            for (int i = 0; i < 9; i++)
                possibleNumbers[i] = FindPossibleNumbers(board, i, column);

            DeleteDupilcates(possibleNumbers);

            for (int i = 0; i < 9; i++)
                if (possibleNumbers[i].Count == 1)
                {
                    board.Set(possibleNumbers[i][0], i, column);
                    changes++;
                }

            return changes;
        }

        private void DeleteDupilcates(List<int>[] list)
        {
            for(int i = 0; i < list.Length; i++)
            {
                for(int j = 0; j < list.Length; j++)
                {
                    List<int> intersection = list[i].Intersect(list[j]).ToList();
                    list[i] = list[i].Except(intersection).ToList();
                    list[j] = list[j].Except(intersection).ToList();
                }
            }
        }

    }
}
