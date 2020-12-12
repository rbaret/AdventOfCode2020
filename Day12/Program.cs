using System;
using System.IO;
using System.Collections.Generic;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day12\\input.txt";
            List<Tuple<char, int>> instructions = GenerateInstructions(path);
            int distanceToManhattan = NavigatePart1(instructions);
            Console.WriteLine(distanceToManhattan);
            distanceToManhattan = NavigatePart2(instructions);
            Console.WriteLine(distanceToManhattan);
        }


        // We are working with orthonormal grid coordinates and use degrees with North as 0 in part 1, and radians in part 2
        private static int NavigatePart1(List<Tuple<char, int>> instructions)
        {
            int x = 0;
            int y = 0;
            int angle = 90; // starting facing east, 90 degrees from north. This angle will varie with rotation instructions and be used to know which direction is the boat facing
            int directionIndex = angle / 90;
            Tuple<char, int, int>[] directionCoordinates = new Tuple<char,int, int>[4];
            directionCoordinates[0] = new Tuple<char, int, int>('N', 0, 1);
            directionCoordinates[1] = new Tuple<char, int, int>('E', 1, 0);
            directionCoordinates[2] = new Tuple<char, int, int>('S', 0, -1);
            directionCoordinates[3] = new Tuple<char, int, int>('W', -1, 0);
            foreach(Tuple<char,int> instruction in instructions)
            {
                directionIndex = angle / 90;
                switch (instruction.Item1)
                {
                    case ('F'):
                        x += directionCoordinates[directionIndex].Item2 * instruction.Item2;
                        y += directionCoordinates[directionIndex].Item3 * instruction.Item2;
                        break;
                    case ('L'):
                        angle = (angle + (360-instruction.Item2)) % 360;
                        break;
                    case ('R'):
                        angle = (angle + instruction.Item2) % 360;
                        break;
                    // Move ship in the following direction
                    case ('N'):
                        y += instruction.Item2;
                        break;
                    case ('E'):
                        x += instruction.Item2;
                        break;
                    case ('S'):
                        y -= instruction.Item2;
                        break;
                    case ('W'):
                        x -= instruction.Item2;
                        break;
                    default:
                        break;
                }
                
            }

            return (Math.Abs(x)+Math.Abs(y));
        }
        private static int NavigatePart2(List<Tuple<char, int>> instructions)
        {
            // Boat coordinates
            int x = 0;
            int y = 0;

            // Waypoint coordinates relative to ship, starting with East 10, North 1:
            double wpX = 10;
            double wpY = 1;

            double wpTempX = 0;
            double wpTempY = 0;
            double angle = 0;
            foreach(Tuple<char,int> instruction in instructions)
            {
                switch (instruction.Item1)
                {
                    case ('F'):
                        x += (int)(wpX * instruction.Item2);
                        y += (int)(wpY * instruction.Item2);
                        break;
                    case ('L'): // Rotate the point counterclockwise/trigonometric.
                        angle = instruction.Item2 * Math.PI / 180; // We are working in radians instead of degrees
                        wpTempX = wpX * Math.Cos(angle) - wpY * Math.Sin(angle);
                        wpTempY = wpX * Math.Sin(angle) + wpY * Math.Cos(angle);
                        wpX = Math.Round(wpTempX);
                        wpY = Math.Round(wpTempY);
                        break;
                    case ('R'): // Rotate the waypoint clockwise/antitrigonometric.
                        angle = -instruction.Item2 * Math.PI / 180;
                        wpTempX = wpX * Math.Cos(angle) - wpY * Math.Sin(angle);
                        wpTempY = wpX * Math.Sin(angle) + wpY * Math.Cos(angle);
                        wpX = Math.Round(wpTempX);
                        wpY = Math.Round(wpTempY);
                        break;
                    // Move WP North, East, South, West
                    case ('N'): 
                        wpY += instruction.Item2;
                        break;
                    case ('E'):
                        wpX += instruction.Item2;
                        break;
                    case ('S'):
                        wpY -= instruction.Item2;
                        break;
                    case ('W'):
                        wpX -= instruction.Item2;
                        break;
                    default:
                        break;
                }
                
            }

            return (Math.Abs(x)+Math.Abs(y));
        }

        private static List<Tuple<char, int>> GenerateInstructions(string path)
        {
            List<Tuple<char, int>> instructions = new List<Tuple<char, int>>();
            foreach (string line in File.ReadAllLines(@path))
            {
                char direction = line[0];
                int distance = int.Parse(line[1..]);
                instructions.Add(new Tuple<char, int>(direction, distance));
            }
            return instructions;
        }
    }
}
