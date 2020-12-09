using System;
using System.IO;
using System.Collections.Generic;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day9\\input.txt";
            long impostor = FindImpostor(path);
            Console.WriteLine("Part 1 - Impostor : "+impostor);
            long weakness = FindWeakness(impostor, path);
            Console.WriteLine("Part 2 - Weakness found : " +weakness);
        }

        private static List<long> GenerateNumbersList(string path) // Generate a list of numbers on demand
        {
            List<long> numbers = new List<long>();
            foreach (string line in File.ReadAllLines(@path))
            {
                numbers.Add(long.Parse(line));
            }
            return numbers;
        }

        // Part 1
        private static long FindImpostor(string path)
        {
            List<long> numbers = GenerateNumbersList(path);
            List<long> sum25 = new List<long>();
            int index = 0;
            

            for(index = 25; index < numbers.Count; index++) // starting at index 25 to have at least 25 previous numbers
            {
                // Calculating the sum of 2 in all 25 previous numbers
                for(int sumNr1 = index-25; sumNr1 < (index-1); sumNr1++)
                {
                    for(int sumNr2 = index-24; sumNr2 < index;sumNr2++)
                    {
                        sum25.Add((long)(numbers[sumNr1] + numbers[sumNr2]));
                    }
                }
                if (!sum25.Contains(numbers[index])){ // The sum of 25 previous numbers doesn't match, we found the impostor !
                    return numbers[index];
                }
            }
            return 0;
        }

        // Part 2
        private static long FindWeakness(long impostor,string path)
        {
            List<long> numbers = GenerateNumbersList(path);
            for(int startindex = 0; startindex < numbers.Count-1; startindex++) // Adding all consecutive numbers starting from the first one and increasing the starting point
            {
                long sum = 0;
                int nbSum = 0;
                while (sum < impostor) // Adding next number til while the sum is below impostor value
                {
                    sum += numbers[startindex + nbSum];
                    nbSum++;
                }
                if ((sum == impostor) && (nbSum>=2)) // SUm found 
                {
                    List<long> consecutiveNumbers = numbers.GetRange(startindex, nbSum);
                    consecutiveNumbers.Sort(); // Sorting the n numbers used in the sum
                    return (consecutiveNumbers[0]+ consecutiveNumbers[nbSum-1]); // Returning the sum of first and last in the list
                }
            }
            return 0;
        }
    }
}
