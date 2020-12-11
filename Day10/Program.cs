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
            long count = 1;
            int node=0;
            for(int index = adapters.Count-1;index>=1;index--)
            {
                node = 0;
                for(int indexPrev = index - 1; (indexPrev >= 0) && (index-indexPrev<=3); indexPrev--)
                {
                    if (adapters[index] - adapters[indexPrev] <= 3)
                        node++;
                }
                count *= node;
            }
            return count;
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
