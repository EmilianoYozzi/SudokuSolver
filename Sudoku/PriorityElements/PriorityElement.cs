using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.PriorityElements
{
    public abstract class PriorityElement : IComparable
    {
        public int Count { 
            get {
                return 9 - EmptyTiles.Count;
            } 
        }
        public int ID { get; set; }

        public List<int> MissingNumbers { get; protected set; }

        public List<int[]> EmptyTiles { get; protected set; }

        public bool CompareTo(PriorityElement element)
        {
            return GetPriority() >= element.GetPriority();
        }

        public int CompareTo(object obj)
        {
            return ((PriorityElement) obj).GetPriority() - GetPriority();
        }

        public abstract int GetPriority();

        protected abstract void SetTiles();

        public void FillTile(int value, int i, int j)
        {
            for(int k = 0; k < EmptyTiles.Count; k++)
            {
                int[] coord = EmptyTiles[k];
                if (coord[0] == i && coord[1] == j)
                {
                    EmptyTiles.RemoveAt(k);
                    break;
                }
            }
                    
            MissingNumbers.Remove(value);
        }

        public override string ToString()
        {
            return ID + " - " + Count;
        }
    }
}
