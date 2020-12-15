using System;
using System.Collections.Generic;
using System.IO;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day15\\input.txt";
            Console.WriteLine("2020th spoken number : "+ CountSpokenNumber(path, 2020));
            Console.WriteLine("30000000th spoken number"+ CountSpokenNumber(path, 30000000));
        }

        private static int CountSpokenNumber(string path, int nthSpokenNumber)
        {
            int[] part1Array = new int[] { 0, 13, 1, 16, 6, 17 }; // Initial set
            int currentNumber = 0;
            int prevOccurence = 0;
            int prevNumber=0;
            Dictionary<int, Tuple<int, int>> numbersMap = new Dictionary<int, Tuple<int, int>>(); // key is the number value, in the tuple, int is the index of current occurence, second int is value of the previous occurence (if they're different, the number was spoken before)
            for (int i = 0; i < part1Array.Length; i++)
            {
                numbersMap.Add(part1Array[i], new Tuple<int, int>(i+1, i+1)); // We add 1 to have the real nth spoken number
                prevNumber = part1Array[i];
            }

            for (int currentNrIndex = part1Array.Length+1; currentNrIndex <= nthSpokenNumber; currentNrIndex++) // Let's roll the game for the nth requested number spoken
            {
                if (numbersMap.ContainsKey(prevNumber) && numbersMap.GetValueOrDefault(prevNumber).Item1 == numbersMap.GetValueOrDefault(prevNumber).Item2) // last occurence of the previous number was also the first. It was never spoken before
                {
                    currentNumber = 0;
                }
                else if (numbersMap.ContainsKey(prevNumber) && numbersMap.GetValueOrDefault(prevNumber).Item1 != numbersMap.GetValueOrDefault(prevNumber).Item2)  // previous number had aleady been spoken
                {
                    currentNumber = numbersMap.GetValueOrDefault(prevNumber).Item1 - numbersMap.GetValueOrDefault(prevNumber).Item2;
                }

                if (numbersMap.ContainsKey(currentNumber)) // Do we already have the new number in the list ? Yes
                {
                    prevOccurence = numbersMap.GetValueOrDefault(currentNumber).Item1;
                    numbersMap.Remove(currentNumber);
                    numbersMap.Add(currentNumber, new Tuple<int, int>(currentNrIndex, prevOccurence));
                }
                else // No ? Let's add it then !
                {
                    numbersMap.Add(currentNumber, new Tuple<int, int>(currentNrIndex, currentNrIndex));
                }
                prevNumber = currentNumber;
            }
            return prevNumber;
        }
    }
}
