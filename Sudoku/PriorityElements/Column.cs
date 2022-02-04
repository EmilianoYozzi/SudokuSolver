using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.PriorityElements
{
    public class Column : PriorityElement
    {
        public Column(int id)
        {
            ID = id;
            MissingNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            SetTiles();
        }

        protected override void SetTiles()
        {
            EmptyTiles = new List<int[]>();

            for (int i = 0; i < 9; i++)
                EmptyTiles.Add(new int[] { i, ID });
        }

        public override int GetPriority()
        {
            return Count + 1;
        }

        public override string ToString()
        {
            return "Column: " + base.ToString();
        }
    }
}
