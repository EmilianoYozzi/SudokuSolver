using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.PriorityElements
{
    public class Quadrant : PriorityElement
    {
        public int IFrom { get; set; }
        public int ITo { get; set; }
        public int JFrom { get; set; }
        public int JTo { get; set; }

        public Quadrant(int id)
        {
            ID = id;
            MissingNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            SetValues(id);
            SetTiles();
        }

        private void SetValues(int id)
        {
            JFrom = (id % 3) * 3;
            JTo = JFrom + 3;
            IFrom = ((int) (id / 3)) * 3;
            ITo = IFrom + 3;
        }

        protected override void SetTiles()
        {
            EmptyTiles = new List<int[]>();

            for(int i = IFrom; i < ITo; i++)
                for (int j = JFrom; j < JTo; j++)
                    EmptyTiles.Add(new int[] { i, j });
        }

        public override int GetPriority()
        {
            return Count;
        }

        public override string ToString()
        {
            return "Quad: " + base.ToString();
        }

    }
}
