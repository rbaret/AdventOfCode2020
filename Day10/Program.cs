using System;
using System.IO;
using System.Collections.Generic;


namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day10\\inputsample.txt";
            Console.WriteLine(AdapterChain(path));
            List<int> adaptersList = GenerateAdaptersList(path);
            Console.WriteLine(FindAdaptersCombinations(adaptersList)); 

        }

        private static int AdapterChain(string path)
        {
            int diff1 = 0;
            int diff3 = 0;
            List<int> adapters = GenerateAdaptersList(path);
            for(int index = 1; index < adapters.Count; index++)
            {
                if (adapters[index] - adapters[index - 1] == 1)
                    diff1++;
                else if (adapters[index] - adapters[index - 1] == 3)
                    diff3++;
            }
            return (diff1 * diff3);
        }

        private static long FindAdaptersCombinations(List<int> adapters)
        {
            //long countCombination = 0;
            List<long> countCombinations = new List<long>();
            // Initialize combinations for each adapters to 1
            for (int i = 0; i <= adapters.Count; i++)
                countCombinations.Add(1);

            for(int index = adapters.Count-2;index>=0;index--)
            {
                long combinations = 0;
                for(int indexNext = index + 1; (indexNext <adapters.Count) && (indexNext<=index+3); indexNext++)
                {
                    if (adapters[indexNext] - adapters[index] <= 3)
                        //combinations++;
                        combinations += countCombinations[indexNext];
                }
                //countCombinations += combination;
                countCombinations[index] = combinations;

            }
            return countCombinations[0];
        }

        public static ulong ResolvePart2(List<int> Numbers)
        {
            List<ulong> arrangementCounts = new List<ulong>();
            for (int i = 0; i <= Numbers.Count; i++)
                arrangementCounts.Add(1);

            for (int headIndex = Numbers.Count - 2; headIndex >= 0; headIndex--)
            {
                ulong arrangementCount = 0;
                for (
                    int jump = 1;
                    jump <= 3
                    && headIndex + jump < Numbers.Count
                    && Numbers[headIndex + jump] - Numbers[headIndex] <= 3;
                    jump++)
                    arrangementCount += arrangementCounts[headIndex + jump];

                arrangementCounts[headIndex] = arrangementCount;
            }

            return arrangementCounts[0];
        }
        private static List<int> GenerateAdaptersList(string path)
        {
            List<int> adapters = new List<int>();
            adapters.Add(0); // Add the output of the outlet to the list
            foreach (string line in File.ReadLines(@path))
            {
                adapters.Add(int.Parse(line));
            }
            adapters.Sort();
            adapters.Add(adapters[adapters.Count - 1] + 3);
            return adapters;
        }
    }
}
