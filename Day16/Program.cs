using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day16
{
    class Program
    {
        public static List<string> validLines = new List<string>();
        public static Dictionary<string, Tuple<int, int, int, int>> fields = new Dictionary<string, Tuple<int, int, int, int>>(); // field name, lower low boundary, higher low boundary, lower high boundary, higher high boundary
        public static List<Tuple<string, int>> fieldsPossibleColumns = new List<Tuple<string, int>>();
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day16\\input.txt";
            string[] lines = File.ReadAllLines(path);
            long errorRate = CalculateErrorRate(lines);
            Console.WriteLine("Error rate for part 1 : "+errorRate);
            long departuresProduct = SolvePart2(validLines,lines[22]);
            Console.WriteLine("Part 2 product of 6 departure fields for my ticket : "+departuresProduct);
        }

        private static long SolvePart2(List<string> validLines,string myTicket)
        {
            bool invalidInField = false;
            int[,] nearbyTickets = new int[validLines.Count,fields.Count];
            int fieldValue = 0;
            int fieldIndex;
            int lineNumber = 0;
            int lineFieldIndex = 0;
            for (lineNumber = 0; lineNumber < validLines.Count; lineNumber++) // Let's generate a matrix of all tickets values
            {
                for(lineFieldIndex=0;lineFieldIndex<fields.Count;lineFieldIndex++)
                {
                    nearbyTickets[lineNumber, lineFieldIndex] = int.Parse(validLines[lineNumber].Split(',')[lineFieldIndex]);
                }
            }
            fieldIndex = 0;
            foreach(var field in fields) // Let's try to find the possible candidates for each field
            {
                invalidInField = false;
                for(lineFieldIndex=0;lineFieldIndex<fields.Count;lineFieldIndex++) // Starting by the first column
                {
                    invalidInField = false;
                    for (lineNumber = 0; lineNumber < validLines.Count; lineNumber++) // of each line
                    {
                        fieldValue = nearbyTickets[lineNumber, lineFieldIndex];
                        if(fieldValue<field.Value.Item1 || fieldValue > field.Value.Item4 || (fieldValue > field.Value.Item2 && fieldValue < field.Value.Item3))
                        {
                            invalidInField = true; // as soon as we find an invalid value in the column for the field, we go to the next column
                            break;
                        }
                    }
                    if (!invalidInField) // if at the end of the loop no invalid value was found
                    {
                        fieldsPossibleColumns.Add(new Tuple<string,int>(field.Key, lineFieldIndex)); // we add the current column to candidates for the current field
                    }
                }
                fieldIndex++;
            }

            Dictionary<string, int> finalOrder = new Dictionary<string, int>();
            List<Tuple<string, int>> tempFieldsPossibleColumns = new List<Tuple<string, int>> (fieldsPossibleColumns); // We create a temporary List to be able to loop through a modified version, it receives the current list of candidates
            do // Let's start the search for candidates
            {
                foreach (Tuple<string, int> candidate in fieldsPossibleColumns) // For each couple of values in the list of candidates
                {
                    if (fieldsPossibleColumns.FindAll(e => e.Item1 == candidate.Item1).Count == 1) // we look for the field with only 1 possible column candidate
                    {
                        finalOrder.Add(candidate.Item1, candidate.Item2); // Add the newly found dield to the final list
                        tempFieldsPossibleColumns.RemoveAll(e => e.Item1 == candidate.Item1); // Remove the candidate itself
                        tempFieldsPossibleColumns.RemoveAll(e => e.Item2 == candidate.Item2); // and remove all the values couples with the column of the current field
                    }
                }
                fieldsPossibleColumns = new List<Tuple<string, int>>(tempFieldsPossibleColumns); // the original list becomes the shrinked one
            }
            while (tempFieldsPossibleColumns.Count > 0); // and we loop through the list til no field is left

            List<int> myTicketValues = new List<int>(); // Let's finally parse our own ticket values
            long finalResult = 1;
            foreach(string value in myTicket.Split(','))
            {
                myTicketValues.Add(int.Parse(value));
            }
            foreach(string key in finalOrder.Keys) // fetch all fields names
            {
                if (key.StartsWith("departure"))
                    finalResult *= myTicketValues[finalOrder[key]]; // we multiply only the departure fields
            }
            return finalResult;
        }

        private static long CalculateErrorRate(string[] lines)
        {
            string fieldName = "";
            // Series of variables for ease of comprehention. We could directly use fields values instead
            int lowerLowBoundary = 0;
            int higherLowBoundary = 0;
            int lowerHighBoundary = 0;
            int higherHighBoundary = 0;
            long errorRate = 0;
            bool valid = false;
            string[] temp = new string[4];
            string[] ticketValues;
            

            
            for (int index = 0; index < 20; index++) // Parse the first 20 lines which contain classes info
            {
                fieldName = lines[index].Split(':')[0];
                temp = lines[index].Split(':')[1].Replace(" ", String.Empty).Replace("or", "-").Split('-');
                lowerLowBoundary = int.Parse(temp[0]);
                higherLowBoundary = int.Parse(temp[1]);
                lowerHighBoundary = int.Parse(temp[2]);
                higherHighBoundary = int.Parse(temp[3]);
                fields.Add(fieldName, new Tuple<int, int, int, int>(lowerLowBoundary, higherLowBoundary, lowerHighBoundary, higherHighBoundary));
            }

            // Parse all nearby tickets
            for (int lineIndex = 25; lineIndex < lines.Length; lineIndex++)
            {
                ticketValues = lines[lineIndex].Split(',');
                foreach (string value in ticketValues)
                {
                    int numericValue = int.Parse(value);
                    valid = true;
                    /* Long way to proceed. Should be avoided by calculating the final valid value range(s) by checking overlaps. I did that, thought it was incorrect, deleted the code
                     * it wasn't incorrect, too bad, I'm too lazy to rewrite it (cf lower for dirty hardcoded values
                    foreach (var item in fields)
                    {
                        valid = false;
                        lowerLowBoundary = item.Value.Item1;
                        higherLowBoundary = item.Value.Item2;
                        lowerHighBoundary = item.Value.Item3;
                        higherHighBoundary = item.Value.Item4;
                        if ((numericValue >= lowerLowBoundary && numericValue <=lowerHighBoundary) || (numericValue >= lowerHighBoundary && numericValue <= higherHighBoundary))
                        {
                            valid = true;
                            break;
                        }

                    }*/
                    if (numericValue <25 || numericValue > 974) valid = false; // Yeah yeah, a little speed cheat obtained by looking for the overlapping ranges and finding the final unique range with a code I removed as I thought it was false but actually wasn't...
                    if (!valid)
                    {
                        errorRate += numericValue;
                        break;
                    }
                }
                if (valid)
                    validLines.Add(lines[lineIndex]);
            }
            return errorRate;
        }
    }
}
