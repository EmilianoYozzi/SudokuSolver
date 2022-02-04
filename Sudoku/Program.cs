using System;
using System.Collections.Generic;
using System.IO;

namespace Sudoku
{
    public class Program
    {
        private const string inputPath = "sudoku.txt";
        private const string outputPath = "sudoku(solved).txt";

        public static void Main(string[] args)
        {
            Board sudoku = new Board();

            try
            {
                sudoku.LoadValues(inputPath);
            } catch (FileNotFoundException)
            {
                Console.WriteLine("I need a sudoku.txt to work!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(sudoku.ToString());

            int input;
            do {
                Console.WriteLine("Validate(1) or solve(2)?");
                input = Int32.Parse(Console.ReadLine());
            } while (input != 1 && input != 2);

            if (input == 1) {
                Validator validator = new Validator();

                List<string> errors = validator.Validate(sudoku);

                foreach (string s in errors)
                    Console.WriteLine(s);
            } else
            {
                Solver solver = new Solver();

                Board solvedSudoku = solver.Solve(sudoku);
                Console.WriteLine(solvedSudoku);

                solvedSudoku.Export(outputPath);
            }
            Console.ReadLine();
        }
    }
}
