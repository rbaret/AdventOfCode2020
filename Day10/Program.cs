using System;
using System.IO;
using System.Collections.Generic;


namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day10\\input.txt";
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

        // Implemented after I have well understood the dynamic programming concept for x(i) = x(i-1)+x(i-2)...+x(i-n) without recursion
        private static long FindAdaptersCombinations(List<int> adapters)
        {
            int nbAdapters = adapters.Count;
            long[] adapterCombinations = new long[nbAdapters];
            adapterCombinations[nbAdapters-1] = 1;
            long totalCombinations = 0;
            for (int index = nbAdapters-2;index>=0;index--)
            {
                
                for(int indexNext = index + 1; (indexNext <adapters.Count) && (indexNext<=index+3); indexNext++)
                {
                    if (adapters[indexNext] - adapters[index] <= 3)
                        adapterCombinations[index] += adapterCombinations[indexNext];                      
                }
                totalCombinations += adapterCombinations[index];

            }
            return adapterCombinations[0];
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
