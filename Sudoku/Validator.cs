using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    public class Validator
    {

        public Validator()
        {

        }

        public List<string> Validate(Board sudoku)
        {
            List<string> errors = new List<string>();

            for(int i = 0; i < 9; i++)
            {
                int[] numbers = new int[10];
                for(int j = 0; j < 9; j++)
                {
                    numbers[sudoku.Get(i, j)]++;
                }
                for(int j = 1; j < 10; j++)
                {
                    if (numbers[j] != 1)
                        errors.Add("'"+j + "' aprears " + numbers[j] + " times in row " + (i+1));
                }
            }

            for (int i = 0; i < 9; i++)
            {
                int[] numbers = new int[10];
                for (int j = 0; j < 9; j++)
                {
                    numbers[sudoku.Get(j, i)]++;
                }
                for (int j = 1; j < 10; j++)
                {
                    if (numbers[j] != 1)
                        errors.Add("'" + j + "' aprears " + numbers[j] + " times in column " + (i + 1));
                }
            }

            return errors;
        }
    }
}
