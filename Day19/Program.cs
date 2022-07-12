using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

namespace Day19
{
    static class Program
    {
        static void Main(string[] args)
        {
            string inputPath = ".\\input.txt";
            int solution;
            Dictionary<string, string> ruleset;
            List<string> messages;
            ruleset = new Dictionary<string, string>(createRuleset(inputPath));
            messages = new List<string>(createMessagesList(inputPath));
            solution = solvePart1(ruleset, messages);
            Console.WriteLine(solution);

        }

        // Function to generate the dictionary of ruleset
        public static Dictionary<string, string> createRuleset(string inputPath)
        {
            string[] lines = File.ReadAllLines(@inputPath);
            Dictionary<string, string> ruleSet = new Dictionary<string, string>();
            // read each line
            foreach (string line in lines)
            {
                string cleanedLine;
                if (line.Contains(":"))
                { // We only want the lines with a column as they are the ones describing the rules
                    cleanedLine = Regex.Replace(line, "\"", "");
                    string ruleId = cleanedLine.Substring(0, line.IndexOf(':'));
                    string rule = cleanedLine.Substring(line.IndexOf(':') + 2);
                    ruleSet.Add(ruleId, rule);
                }
            }
            return ruleSet;
        }

        public static List<string> createMessagesList(string path)
        {
            string[] lines = File.ReadAllLines(@path);
            List<string> messages = new List<string>();
            foreach (string line in lines)
            {
                if (line.StartsWith('a') | line.StartsWith('b')) // We only keep the lines starting with a or b as they are the actual messages
                {
                    messages.Add(line);
                }
            }
            return messages;
        }

        public static int solvePart1(Dictionary<string, string> ruleset, List<string> messages)
        {
            string regex;
            string regexBase = buildRegex(ruleset, "0");
            StringBuilder regexSb = new StringBuilder();
            regexSb.Append("^");
            regexSb.Append(regexBase);
            regexSb.Append("$");
            regex = regexSb.ToString();
            Console.WriteLine(regex);
            int validMessages = countValidMessages(messages, regex);
            return validMessages;
        }

        // Recursive unction to build the regex to match messages patterns. Only
        public static string buildRegex(Dictionary<string, string> ruleset, string ruleId)
        {
            StringBuilder sb = new StringBuilder();
            string rule = "";
            if (ruleset.TryGetValue(ruleId, out rule))
            {
                if (rule.Contains('|'))
                {
                    sb.Append("((");
                }
                string[] rulesToMatch = ruleset.GetValueOrDefault(ruleId).Split(' ');
                foreach (string ruleToMatch in rulesToMatch)
                {
                    switch (ruleToMatch)
                    {
                        case "a":
                            sb.Append('a');
                            break;

                        case "b":
                            sb.Append('b');
                            break;

                        case "|":
                            sb.Append(")|(");
                            break;
                        default:
                            sb.Append(buildRegex(ruleset, ruleToMatch));
                            break;
                    }
                }
                if (rule.Contains('|'))
                {
                    sb.Append("))");
                }
            }
            return sb.ToString();
        }

        // Function to count the number of messages fully matching the regex
        public static int countValidMessages(List<string> messages, string regex)
        {
            int validMessages = 0;
            foreach (string message in messages)
            {
                if (Regex.IsMatch(message, regex))
                {
                    validMessages++;
                }
            }
            return validMessages;
        }
    }
}
