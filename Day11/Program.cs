using System;
using System.IO;
using System.Threading;

namespace Day11
{
    class Program
    {
        const int HEIGHT = 90;
        const int WIDTH = 99;
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day11\\input.txt";
            char[][] matrix = GenerateMatrix(path);
            Console.WriteLine(" Part 1 occupied seats : " + CountOccupiedSeats(matrix)+"\n Press any key to go to part 2");
            Console.ReadKey();
            matrix = GenerateMatrix(path);
            Console.WriteLine(" Part 2 occupied seats : " + CountOccupiedSeatsPart2(matrix));
        }

        private static void DisplayMatrix(char[][] matrix)
        {
            Console.Clear();
            foreach (char[] line in matrix)
            {

                Console.WriteLine(line);
            }
            Thread.Sleep(20);
        }

        private static char[][] GenerateMatrix(string path)
        {
            string[] lines = File.ReadAllLines(@path);
            char[][] matrix = new char[90][];
            int index = 0;
            foreach (string line in lines)
            {
                matrix[index] = line.ToCharArray();
                index++;
            }
            return matrix;
        }


        // L are free seats, # are occupied seats, . is floor (but not lava)
        private static int CountOccupiedSeats(char[][] matrix)
        {
            char[][] buffer = new char[HEIGHT][];
            for (int bufferLine = 0; bufferLine < HEIGHT; bufferLine++)
            {
                buffer[bufferLine] = new char[WIDTH];
            }
            int iterations = 0;
            int occupiedSeats = 0;
            DumpMatrix(matrix, buffer);
            do
            {
                DumpMatrix(buffer, matrix);
                DisplayMatrix(matrix);
                for (int indexV = 0; indexV < HEIGHT; indexV++)
                {
                    for (int indexH = 0; indexH < WIDTH; indexH++)
                    {
                        // Check surroundings of the cell
                        int occupiedNeighbors = 0;
                        for (int shiftV = -1; shiftV <= 1; shiftV++)
                        {
                            for (int shiftH = -1; shiftH <= 1; shiftH++)
                            {
                                int row = indexV + shiftV; // row variation
                                int col = indexH + shiftH;


                                if ((row >= 0) &&
                                    (row < HEIGHT) &&
                                    (col >= 0) &&
                                    (col < WIDTH) &&
                                    ((row != indexV) ||
                                    (col != indexH)) && matrix[row][col] == '#') // Ignore out of bound cells and current cell
                                    occupiedNeighbors++;

                            }
                        }

                        // Change seat status based on neighbors
                        switch (matrix[indexV][indexH])
                        {
                            case 'L':
                                if (occupiedNeighbors == 0)
                                {
                                    buffer[indexV][indexH] = '#';
                                    occupiedSeats++;
                                }
                                else buffer[indexV][indexH] = 'L';
                                break;
                            case '#':
                                if (occupiedNeighbors >= 4)
                                {
                                    buffer[indexV][indexH] = 'L';
                                    occupiedSeats--;
                                }
                                else buffer[indexV][indexH] = '#';
                                break;
                            case '.':
                                buffer[indexV][indexH] = '.';
                                break;
                            default:
                                break;
                        }
                    }
                }
                iterations++;
            } while (!AreMatrixEqual(matrix, buffer));
            DumpMatrix(buffer, matrix); // Let's replace the matrix with the new data one last time
            DisplayMatrix(matrix); // And display the final state
            // Count occupied seats after stable state
            return occupiedSeats;
        }
        private static int CountOccupiedSeatsPart2(char[][] matrix)
        {
            char[][] buffer = new char[HEIGHT][];
            for (int bufferLine = 0; bufferLine < HEIGHT; bufferLine++)
            {
                buffer[bufferLine] = new char[WIDTH];
            }
            int lineLength = WIDTH;
            int iterations = 0;
            int occupiedSeats = 0;
            DumpMatrix(matrix, buffer);
            do
            {
                DumpMatrix(buffer, matrix);
                DisplayMatrix(matrix);
                for (int indexV = 0; indexV < HEIGHT; indexV++)
                {
                    for (int indexH = 0; indexH < lineLength; indexH++)
                    {
                        // Check surroundings of the cell in each direction

                        int occupiedNeighbors = 0;
                        int[] horizontalDirections = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 }; // Variations of column index for each direction, starting top to diag upper left, clockwise
                        int[] verticalDirections = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 }; // Variations of line index for each direction, starting top to diag upper left, clockwise
                        int shiftV;
                        int shiftH;

                        for (int indexDirection = 0; indexDirection < 8; indexDirection++)
                        {
                            // Look up
                            shiftV = verticalDirections[indexDirection];
                            shiftH = horizontalDirections[indexDirection];
                            int row = indexV + shiftV;
                            int col = indexH + shiftH;
                            bool seatFound = false;
                            while ((row >= 0) &&
                                  (row < HEIGHT) &&
                                  (col >= 0) &&
                                  (col < lineLength) &&
                                  !seatFound) // Ignore out of bound cells and current cell
                            {
                                switch (matrix[row][col])
                                {
                                    case '#':
                                        occupiedNeighbors++;
                                        seatFound = true;
                                        break;
                                    case 'L':
                                        seatFound = true;
                                        break;
                                    default:
                                        break;
                                }
                                shiftV += verticalDirections[indexDirection];
                                row = indexV + shiftV;
                                shiftH += horizontalDirections[indexDirection];
                                col = indexH + shiftH;
                            }
                        }
                        // Change seat status based on neighbors
                        switch (matrix[indexV][indexH])
                        {
                            case 'L':
                                if (occupiedNeighbors == 0)
                                {
                                    buffer[indexV][indexH] = '#';
                                    occupiedSeats++;
                                }
                                else buffer[indexV][indexH] = 'L';
                                break;
                            case '#':
                                if (occupiedNeighbors >= 5)
                                {
                                    buffer[indexV][indexH] = 'L';
                                    occupiedSeats--;
                                }
                                else buffer[indexV][indexH] = '#';
                                break;
                            case '.':
                                buffer[indexV][indexH] = '.';
                                break;
                            default:
                                break;
                        }
                    }
                }
                iterations++;
            } while (!AreMatrixEqual(matrix, buffer));
            DumpMatrix(buffer, matrix);
            DisplayMatrix(matrix);
            // Count occupied seats after stable state
            return occupiedSeats;
        }

        private static void DumpMatrix(char[][] sourceMatrix, char[][] targetMatrix)
        {
            int lineLength = sourceMatrix[0].Length;
            for (int indexV = 0; indexV < sourceMatrix.Length; indexV++)
                Array.Copy(sourceMatrix[indexV], targetMatrix[indexV], lineLength);
        }

        private static bool AreMatrixEqual(char[][] matrix1, char[][] matrix2)
        {
            int width = matrix1[0].Length;
            int height = matrix1.Length;
            bool areEqual = true;
            for (int row = 0; row < height && areEqual; row++)
            {
                for (int col = 0; col < width && areEqual; col++)
                {
                    if (matrix1[row][col] != matrix2[row][col]) areEqual = false;
                }
            }
            return areEqual;
        }
    }
}