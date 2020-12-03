using System;
using System.IO;
using System.Collections.Generic;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day3\\input.txt";
            Tuple<IEnumerable<string>, int, int> matrix = GetMatrix(inputPath);
            char[][] slope = GenerateSlope(matrix);
            int height = matrix.Item2;
            int width = matrix.Item3;
            int[] hSlopes = { 1, 3, 5, 7, 1 }; // Slopes horizontal movements
            int[] vSlopes = { 1, 1, 1, 1, 2 }; // Slopes vertical movements
            ulong encounteredTrees = RideSlope(slope, height, width, hSlopes[1], vSlopes[1]); // index 1 corresponds to the values of Part 1
            ulong treesProduct = testSlopes(slope, height, width, hSlopes, vSlopes); // Part 2
            Console.WriteLine("Trees encountered in part 1 : " + encounteredTrees.ToString());
            Console.WriteLine("Trees encountered product for part 2 : " + treesProduct.ToString());
            Console.ReadKey();
        }

        private static ulong testSlopes(char[][] slope, int height, int width, int[] hSlopes, int[] vSlopes)
        {
            ulong treesProduct=1;
            for (int slopeIndex = 0; slopeIndex<hSlopes.Length;slopeIndex++)
            {
                treesProduct *= RideSlope(slope, height, width, hSlopes[slopeIndex], vSlopes[slopeIndex]);
            }
            return treesProduct;
        }

        private static char[][] GenerateSlope(Tuple<IEnumerable<string>, int, int> matrix)
        {
            int height = matrix.Item2;
            int width = matrix.Item3;
            IEnumerable<string> slope = matrix.Item1;
            char[][] grid = new char[height][];
            int lineNr = 0;
            foreach (string line in slope)
            {
                grid[lineNr] = line.ToCharArray();
                lineNr++;
            }
            return grid;
        }
        private static ulong RideSlope(char[][] grid, int height, int width, int hSlope, int vSlope)
        {
            int vIndex = 0;
            int hIndex = 0;
            ulong treesEncountered = 0;
            
            // Let's slide !
            while (vIndex < height)
            {
                if (grid[vIndex][hIndex] == '#') treesEncountered++;
                vIndex += vSlope;
                hIndex += hSlope;
                if (hIndex >= width) hIndex %= width;
            }
            return treesEncountered;
        }

        private static Tuple<IEnumerable<string>, int,int> GetMatrix(string path)
        {
            StreamReader file = new StreamReader(path);
            IEnumerable<string> text = File.ReadAllLines(@path);
            int linesNb = 0;
            int columnsNb= 0;
            string line;
            if ((line = file.ReadLine()) != null) //read first line of the file if there's one
            {
                linesNb++;
                columnsNb = line.Length;
            }
            while ((line = file.ReadLine()) != null){ // and then the rest of the lines
                linesNb++;
            }
            return new Tuple<IEnumerable<string>, int,int>(text,linesNb,columnsNb);
        }
    }
}
