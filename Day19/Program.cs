using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day19
{
    public class MatrixTools
    {
        public static int[,] rotateMatrix(int[,] matrix)
        {
            // This functions allows to rotate a matrix clockwise
            int lengthi = matrix.GetLength(0);
            int lengthj = matrix.GetLength(1);
            int[,] newMatrix = new int[lengthj,lengthi];
            for (int i = 0; i < lengthi; i++)
            {
                for (int j = 0; j<lengthj ;j++)
                {
                    newMatrix[j,lengthi-i-1] = matrix[i,j];
                }
            }
            return newMatrix;
        }

        public static void printMatrix(int[,] matrix)
        {
            int lengthi = matrix.GetLength(0);
            int lengthj = matrix.GetLength(1);
            for (int i = 0; i < lengthi; i++)
            {
                for (int j = 0; j < lengthj; j++)
                {
                    Console.Write(matrix[i,j].ToString()," ");
                }
                Console.WriteLine();
            }
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            int[,] matrix = new int[,] { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 }, { 11, 12, 13, 14, 15 }, { 16, 17, 18, 19, 20 }, { 21, 22, 23, 24, 25 } };
            MatrixTools.printMatrix(matrix);
            Console.WriteLine();
            
            int [,] newMatrix = MatrixTools.rotateMatrix(matrix);
            MatrixTools.printMatrix(newMatrix);
        }
    }
}