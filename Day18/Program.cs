using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day18
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day18\\input.txt";
            Console.WriteLine(SolvePart1(path));
            Console.WriteLine(SolvePart2(path));

        }

        private static long SolvePart1(string path)
        {
            string[] lines = File.ReadAllLines(@path);
            long solution = 0;
            foreach (string line in lines)
            {
                string lineCompressed = line.Replace(" ", String.Empty); // Let's remove useless spaces
                solution += CalculateLine(lineCompressed).Item1;
            }
            return solution;
        }
        
        // Same as above but for P2
        private static long SolvePart2(string path)
        {
            string[] lines = File.ReadAllLines(@path);
            long solution = 0;
            foreach (string line in lines)
            {
                string lineCompressed = line.Replace(" ", String.Empty);
                solution += CalculateLinePart2(lineCompressed).Item1;
            }
            return solution;
        }

        private static Tuple<long, int> CalculateLine(string line)
        {
            long result = 0;
            char curOperator = '+'; // We always start with a '+' as current operation so we can add the first number to the result when starting
            char c;
            int index = 0;
            do
            {
                c = line[index];
                if (Char.IsNumber(c))
                {
                    switch (curOperator) // Classic
                    {
                        case '+':
                            result += int.Parse(c.ToString()); // Operator is + ? Let's add the current number to the result
                            break;
                        case '*':
                            result *= int.Parse(c.ToString()); // Other wise we multiply the result by the number
                            break;
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                        case '*':
                            curOperator = c;
                            break;
                        case '(': // Oh, a parenthesis. Let's start a new calculus with only the end of the string
                            Tuple<long, int> resultCalc = CalculateLine(line.Substring(line.IndexOf('(', index) + 1)); // A tuple ? Yes, I love to cheat
                            switch (curOperator)
                            {

                                case '+':
                                    result += resultCalc.Item1; // We add the result of the maths in the parenthesis
                                    index += resultCalc.Item2 + 1; // And we only continue the parsing after the number of characters that have been calculated in the parenthesis and subsequent ones
                                    break;
                                case '*':
                                    result *= resultCalc.Item1; // Same as above but with multiplication
                                    index += resultCalc.Item2 + 1;
                                    break;
                            }
                            if (index >= line.Length) // end of line reached ? We're done
                                return (new Tuple<long, int>(result, index)); // We return the result + the number of chars we have crawled so the caller can ignore them
                            break;
                        case ')':
                            return (new Tuple<long, int>(result, index));
                        default:
                            break;

                    }
                }
                index++;
            } while (index < line.Length);
            // Still running ? Then we are probably at the end of the whole line, we can return the result
            return (new Tuple<long, int>(result, index));
        }
        // Same as version for P1 but with small adjustments for precedence
        private static Tuple<long, int> CalculateLinePart2(string line)
        {
            long result = 0;
            long multiplicationBuffer=-1; // We will store the temp value in this variable when we are doing a multiplication
            char curOperator = '+';
            char c;
            int index = 0;
            do
            {
                c = line[index];
                if (Char.IsNumber(c))
                {

                        switch (curOperator)
                        {
                            case '+':
                                if (multiplicationBuffer==-1) // No waiting multiplication
                                {
                                    result += int.Parse(c.ToString()); // Directly increase the result

                                }
                                else
                                {
                                    multiplicationBuffer += int.Parse(c.ToString()); // Multiplication waiting for subsequent numbers to be calculated due to precedence rule. Just add the current number to the buffer
                                }
                                break;
                            case '*':
                                    multiplicationBuffer = int.Parse(c.ToString()); // Directly add the value to the buffer and wait for the other ones
                                break;
                        }
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                            curOperator = c;
                            break;
                        case '*':
                            if (multiplicationBuffer != -1) // Buffer is not empty ?
                            {
                                result *= multiplicationBuffer; // The waiting multiplication can be triggered as we have no more additions waiting
                                multiplicationBuffer = -1; // empty the buffer
                            }
                            curOperator = c;
                            break;
                        case '(':
                            Tuple<long, int> resultCalc = CalculateLinePart2(line.Substring(line.IndexOf('(', index) + 1));
                            switch (curOperator)
                            {

                                case '+':
                                    if (multiplicationBuffer == -1) // If we were not in a waiting multiplication
                                        result += resultCalc.Item1; // We can just add the sub-result to the result
                                    else
                                        multiplicationBuffer += resultCalc.Item1; // Otherwise we add it to the buffer of the current waiting multiplication
                                    index += resultCalc.Item2 + 1;
                                    break;
                                case '*':
                                    multiplicationBuffer = resultCalc.Item1; // Just add the sub-result to the buffer for a new waiting multiplication
                                    index += resultCalc.Item2 + 1;
                                    break;
                            }
                            if (index >= line.Length)
                                return (new Tuple<long, int>(result, index));
                            break;
                        case ')':
                            if (multiplicationBuffer != -1) // No more operators ? We clean the buffer
                                result *= multiplicationBuffer; // by multiplyin the result by it
                            return (new Tuple<long, int>(result, index));
                        default:
                            break;

                    }
                }
                index++;
            } while (index < line.Length);
            if (multiplicationBuffer != -1) // End of the line ? let's flush the buffer if not empty
                result *= multiplicationBuffer;
            return (new Tuple<long, int>(result, index));
        }
    }
}
