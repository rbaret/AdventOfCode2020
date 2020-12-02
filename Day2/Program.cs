using System;
using System.Collections.Generic;
using System.IO;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day2\\input.txt";
            int nbValidPwdPart1 = checkPasswordOccurences(path);
            Console.WriteLine("Number of valid passwords for Part 1 : "+nbValidPwdPart1);
            int nbValidPwdPart2 = checkPasswordPos(path);
            Console.WriteLine("Number of valid passwords for Part 2 : " + nbValidPwdPart2);
            Console.ReadKey();

        }

        private static int checkPasswordOccurences(string path)
        {
            IEnumerable<string> lines = File.ReadAllLines(@path);
            int validPwds = 0;
            foreach(String line in lines)
            {
                char[] separators = new char[]{ '-', ' ', ':' };
                string[] fields = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int minOcc = int.Parse(fields[0]);
                int maxOcc = int.Parse(fields[1]);
                char character = char.Parse(fields[2]);
                string password = fields[3];
                int occurences =  0;
                foreach(char letter in password)
                {
                    if (letter == character) occurences++;
                }
                if ((minOcc<=occurences) && (occurences<=maxOcc)) validPwds++;
            }
            return validPwds;
        }
        private static int checkPasswordPos(string path)
        {
            IEnumerable<string> lines = File.ReadAllLines(@path);
            int validPwds = 0;
            foreach (String line in lines)
            {
                char[] separators = new char[] { '-', ' ', ':' };
                string[] fields = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int firstPos = int.Parse(fields[0]);
                int secondPos = int.Parse(fields[1]);
                char character = char.Parse(fields[2]);
                string password = fields[3];
                int position = 1;
                int occurencesRightPos = 0;
                foreach (char letter in password)
                {
                    if ((letter == character) & ((position==firstPos) | (position==secondPos))){
                        occurencesRightPos++;
                    }
                    position++;
                }
                if (occurencesRightPos == 1) validPwds++;
            }
            return validPwds;
        }
    }
}
