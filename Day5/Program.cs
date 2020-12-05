using System;
using System.IO;
using System.Collections.Generic;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day5\\input.txt";
            int highestSeatId = 0;
            int lowestSeatId = 1023; // We take the biggest value possible for 10 bit -> 1111111111
            string[] boardingPasses = File.ReadAllLines(@path);
            // The binary sorting is actually assigning a binary value to each seat. Easy to work with, just have to convert F,B,L and R to 0 or 1 and do the maths. Values should range from 0000000000 to 1111111111 (in theory, 128 rows, 8 seats per row, 1024 values on 10-bit)


            // Naive approach by applying blindly the rule for seatId = row*8 + col. I leave it to remind myself how dumb I can be even when I spot the trick to ease the job... (using binary)
            /*
            foreach(string pass in boardingPasses)
            {
                int row = Convert.ToInt32(pass.Replace('F', '0').Replace('B', '1').Substring(0, 7), 2);
                int col = Convert.ToInt32(pass.Replace('L', '0').Replace('R', '1').Substring(7, 3), 2);
                int seatId = row * 8 + col;
                if (seatId >= highestSeatId) highestSeatId = seatId;
            }*/

            // another approach (SeatID is actually the whole value of the full binary number, no need to calculate row * 8 + col :
            int[] seats = new int[boardingPasses.Length];
            int seatIndex = 0;
            foreach (string pass in boardingPasses)
            {
                // Enough for part 1
                int seatId = Convert.ToInt32(pass.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'), 2);
                // this is for part 2
                if (seatId >= highestSeatId) highestSeatId = seatId;
                if (seatId <= lowestSeatId) lowestSeatId = seatId;
                seats[seatIndex] = seatId;
                seatIndex++;
            }

            // find the seat
            Array.Sort(seats);

            int mySeatId = 0;
            int size = seats.Length;
            int prevInferior = seats[0];
            int prevSuperior = seats[size - 1];
            int sumLowestHighestSeatId = lowestSeatId + highestSeatId; // The sum we should find when adding extremities of the array and going towards the center
            seatIndex = 0;
            // All seatIDs are supposed to be present except front or back
            // calculate the sum of them and run through the array by both ends at the same time until the sum is not equal to the sum of highest and lowest values
            // By having a list of all contiguous integers sorted in ascending order, the idea is we get always the same number 
            // by adding both of the range and moving towards the center. The moment that sum changes is when there is a missing value
            do
            {
                int sumSeatIDs = seats[seatIndex] + seats[(seats.Length - 1) - seatIndex]; //initial value
                if (sumSeatIDs != sumLowestHighestSeatId) // The effective sum doesn't match !
                {
                    // Looking for which value was missing
                    if (seats[seatIndex] - prevInferior > 1)
                    {
                        mySeatId = prevInferior + 1;
                    }
                    else mySeatId = prevSuperior - 1;
                }
                prevSuperior = seats[(seats.Length - 1) - seatIndex]; // keeping a track of the values of the previous iteration in case we need to find the missing value
                prevInferior = seats[seatIndex];
                seatIndex++;
            } while ((seatIndex <= size / 2) && (mySeatId == 0)); // We stop the loop when

            // Alternative method :
            // Based on the formula to calculate the sum of n integers from 1 to n : n*(n+1)/2 but transposed to our current situation. Here we just take the sum of first and last item of the array and multiply by half the length (rounding the lenght to the next even number if odd by adding 1 to the length)
            int halfArrayLength = 0;
            if (seats.Length % 2 == 0) halfArrayLength = seats.Length / 2; else halfArrayLength = (seats.Length + 1) / 2; 
            int theoriticalSum = (highestSeatId + lowestSeatId) * (halfArrayLength); // The theoritical number we should find by adding all the seats ID
            
            int effectiveSum = 0;
            for (seatIndex = 0; seatIndex < seats.Length; seatIndex++) // Adding all the values of the array to get the effective sum
            {
                effectiveSum += seats[seatIndex];
            }
            int mySeatIdAlt = (theoriticalSum - effectiveSum); // The seatID corresponds to the missing number between practical an theoritical values of the sum of all the seatIDs in the array

            Console.WriteLine("Part 1 : \nHighest seatid : " + highestSeatId + " -- lowest seatid : " + lowestSeatId);
            Console.WriteLine("Part 2 : \nMy seat ID : " + mySeatId);
            Console.WriteLine("Part 2 alternate method : \nMy seat ID: " + mySeatIdAlt);
        }
    }
}
