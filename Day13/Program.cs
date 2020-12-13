using System;
using System.IO;
using System.Collections.Generic;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day13\\input.txt";
            int earliestDeparture = DeparturePart1(path);
            Console.WriteLine("Earliest Departure : " + earliestDeparture);
            ulong megaDeparture = DeparturePart2(path);
            Console.WriteLine("Part 2 : " + megaDeparture);
        }

        private static int DeparturePart1(string path)
        {
            String[] lines = File.ReadAllLines(@path);
            int timestamp = int.Parse(lines[0]);
            int departure = timestamp;
            int busId=0;
            bool found = false;
            List<int> buses = new List<int>();
            foreach(string bus in lines[1].Split(','))
            {
                if (bus != "x")
                {
                    buses.Add(int.Parse(bus));
                    Console.WriteLine(buses[buses.Count-1]);
                }
                if (timestamp + buses[buses.Count - 1] > departure) departure = timestamp + buses[buses.Count - 1];
            }
            foreach(int bus in buses)
            {
                
                int timeToWait = 0;
                found = false;
                do
                {
                    int plannedDeparture = timestamp + timeToWait;
                    if ((plannedDeparture) % bus == 0)
                    {
                        found = true;
                        if (plannedDeparture <= departure)
                        {
                            busId = bus;
                            departure = timestamp + timeToWait;
                        }
                    }
                    timeToWait++;
                } while (!found);

            }
            Console.WriteLine("BusId : " + busId + "and bus timestamp departure : " + departure);
            return busId*(departure-timestamp);
        }
        private static ulong DeparturePart2(string path) // Dirty brutefoce - not tested till the end
        {
            String[] lines = File.ReadAllLines(@path);
            String[] totalBuses = lines[1].Split(',');
            ulong timestamp = 0;
            bool found = true;
            List<int> buses = new List<int>();
            List<int> busesPlusShift = new List<int>();
            List<int> busesPlusShiftUnique = new List<int>();
            ulong lowerBound = 1;
            ulong higherBound = 1;
            ulong index = 0;
            foreach (string busId in lines[1].Split(','))
            {
                if (busId != "x")
                {
                    int multiplier = int.Parse(busId) + (totalBuses.Length-1-(int)index);
                    buses.Add(int.Parse(busId));
                    busesPlusShift.Add(multiplier); 
                    Console.WriteLine(buses[buses.Count - 1]);
                }
                index++;
            }
            foreach(int multiplier in busesPlusShift)
            {
                if (!busesPlusShiftUnique.Contains(multiplier)) busesPlusShiftUnique.Add(multiplier);
            }
            foreach(int multiplier in busesPlusShiftUnique)
            {
                lowerBound *= (ulong)multiplier;
            }
            foreach (int multiplier in busesPlusShift)
            {
                higherBound *= (ulong)multiplier;
            }

            for(index=lowerBound+1;index<higherBound; index ++)
            {
                timestamp = index;
                int indexBus = 0;
                found = true;
                foreach(int bus in buses)
                {
                    int shift = busesPlusShift[indexBus] - bus;
                    if ((timestamp-(ulong)shift)%(ulong)bus != 0)
                    {
                        found = false;
                        break;
                    }
                    indexBus++;
                }
                if (found) break;
            }
            return timestamp;
        }
    }
}
