using System;
using System.IO;
using System.Linq;
namespace Day6
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day6\\input.txt";
            string[] answers = GenerateAnswers(path);
            int sumUniqueGroupAnswers = SumUniqueAnswersPerGroup(answers);
            Console.WriteLine("Sum of questions answered by anyone in a group :" + sumUniqueGroupAnswers);
            int sumAllAswers = SumAllAnswersPerGroup(answers);
            Console.WriteLine("Sum of questions answered by everyone in a group " + sumAllAswers);
        }

        private static string[] GenerateAnswers(string path)
        {
            return File.ReadAllText(@path).Split("\r\n\r\n");
        }

        // Part 1
        private static int SumUniqueAnswersPerGroup(string[] answers)
        {
            int countAnswers = 0;
            foreach (string answer in answers)
            {
                countAnswers += answer.Replace("\r\n", String.Empty).Distinct().Count();
            }
            return countAnswers;
        }

        // Part 2
        private static int SumAllAnswersPerGroup(string[] answers)
        {
            int sumAnswers = 0;
            foreach (string answer in answers)
            {
                string[] groupMembersAnswers = answer.Split("\r\n");
                char[] intersect = groupMembersAnswers[0].ToCharArray();
                foreach (string memberAnswer in groupMembersAnswers)
                {
                    intersect = intersect.Intersect(memberAnswer).ToArray();
                }
                sumAnswers += intersect.Distinct().Count();
            }
            return sumAnswers;
        }
    }
}
