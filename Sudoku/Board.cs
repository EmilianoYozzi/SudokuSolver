using Sudoku.PriorityElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Board
    {
        public const int BOARD_SIZE = 9;
        private int[,] Matrix { get; set; }
        private List<PriorityElement> PriorityElements { get; set; }
        public List<Quadrant> Quadrants { get; set; }

        public List<Row> Rows { get; set; }
        public List<Column> Columns { get; set; }

        public Board()
        {
            Matrix = new int[BOARD_SIZE, BOARD_SIZE];
            InitializePriorityElements();
        }

        private void InitializePriorityElements()
        {
            PriorityElements = new List<PriorityElement>();
            Quadrants = new List<Quadrant>();
            Rows = new List<Row>();
            Columns = new List<Column>();

            for(int i = 0; i < 9; i++) {
                Quadrants.Add(new Quadrant(i));
                PriorityElements.Add(Quadrants[i]);

                Rows.Add(new Row(i));
                PriorityElements.Add(Rows[i]);

                Columns.Add(new Column(i));
                PriorityElements.Add(Columns[i]);
            }
        }

        public void LoadValues(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(@path);

            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                    Set(lines[i][j] - '0', i, j);
        }

        public async Task Export(string path)
        {
            string[] lines = new string[9];

            for(int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    lines[i] += Get(i, j); 
                
            await System.IO.File.WriteAllLinesAsync(@path, lines);

        }

        public void Set(int value, int i, int j)
        {
            if (value == 0)
                return;

            Matrix[i, j] = value;

            int id = GetQuadrantID(i, j);
            Quadrants[id].FillTile(value, i, j);
            Rows[i].FillTile(value, i, j);
            Columns[j].FillTile(value, i, j);
        }

        public int Get(int i, int j)
        {
            return Matrix[i, j];
        }

        public Quadrant GetQuadrant(int i, int j)
        {
            return Quadrants[GetQuadrantID(i, j)];
        }
        
        private int GetQuadrantID(int i, int j)
        {
            return ((int) (i / 3)) * 3 + (int) (j/3);
        }

        public List<PriorityElement> GetPriorityList()
        {
            PriorityElements.Sort();
            return PriorityElements;
        }

        public bool IsCompleted()
        {
            foreach (PriorityElement e in Quadrants)
                if (e.Count < 9)
                    return false;
            return true;
        }

        public override string ToString()
        {
            string matrix = "";

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (i % 3 == 0)
                    matrix += "┼───────┼───────┼───────┼\n";
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (j % 3 == 0)
                        matrix += "│ ";
                    matrix += Matrix[i, j] + " ";
                }
                matrix += "│\n";
            }
            matrix += "┼───────┼───────┼───────┼\n";
            return matrix;
        }

        internal Board Clone()
        {
            Board newBoard = new Board();

            for(int i = 0; i < 9; i++)
                for(int j = 0; j < 9; j++)
                    newBoard.Set(Get(i, j), i, j);

            return newBoard;
        }
    }
}
