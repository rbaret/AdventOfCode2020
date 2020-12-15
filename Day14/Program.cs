using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day14\\input.txt";
            long sum = ExecutePart1(path);
            Console.WriteLine(sum);
            sum = ExecutePart2(path);
            Console.WriteLine(sum);
        }

        private static long ExecutePart1(string path)
        {
            long sum = 0;
            string[] lines = File.ReadAllLines(@path);
            int memSize = 0;
            
            char[] mask = new char[36];
            int memAddress;
            long memValue;
            foreach (string line in lines)
            {
                if (line.StartsWith("mem")) {
                    memAddress = int.Parse((line).Substring(4, line.IndexOf(']') - 4)); // fetchee value between "[]"
                    if ( memAddress >= memSize) memSize = memAddress;
                }
            }
            long[] memArray = new long[memSize+1];

            foreach (string line in lines)
            {

                
                string[] values = line.Split(" = ");
                string memValueMasked;
                if (values[0].StartsWith("mask"))
                {
                    mask = values[1].ToCharArray();
                }
                else
                {
                    memAddress = int.Parse(values[0].Substring(4,values[0].IndexOf(']')-4));
                    memValue = Convert.ToInt64(values[1]);
                    memValueMasked = Convert.ToString(memValue,2).PadLeft(36,'0');
                    StringBuilder sb = new StringBuilder(memValueMasked);
                    for (int maskIndex =0; maskIndex < 36; maskIndex++)
                    {
                        switch (mask[maskIndex])
                        {
                            case '0':
                                sb[maskIndex] = '0';
                                break;
                            case '1':
                                sb[maskIndex] = '1';
                                break;
                        }
                    }
                    memValueMasked = sb.ToString();
                    memArray[memAddress] = Convert.ToInt64(memValueMasked, 2);
                }
            }
            foreach(long memCell in memArray)
            {
                sum += memCell;
            }
            return sum;
        }

        private static long ExecutePart2(string path)
        {
            long sum = 0;
            string[] lines = File.ReadAllLines(@path);
            char[] mask = new char[36];
            Dictionary<long, long> addressMap = new Dictionary<long, long>(); // We store all memory values we alter in a memory map instead of a useless giant array with a size of 2^36
            List<int> floatingBitsIndexes = new List<int>(); ;
            string memAddress;
            long memAddressNumeric = 0;
            long memValue;
            string memAddressMasked;
            foreach (string line in lines)
            {
                string[] values = line.Split(" = ");
                memAddressMasked = "";
                if (values[0].StartsWith("mask"))
                {
                    floatingBitsIndexes.Clear();
                    mask = values[1].ToCharArray();
                    int bitIndex = 0;
                    foreach(char bit in mask)
                    {
                        if (bit == 'X') floatingBitsIndexes.Add(bitIndex); // We get a list of all floating bits and their index in the mask
                        bitIndex++;
                    }
                }
                else
                {
                    memAddress = Convert.ToString(int.Parse(values[0].Substring(4, values[0].IndexOf(']') - 4)), 2).PadLeft(36, '0');
                    memValue = long.Parse(values[1]);
                    int nbFloatingBits = floatingBitsIndexes.Count; // How many floating bits do we have ? We will have 2^nbFloatingBits possible combinations
                    StringBuilder sb = new StringBuilder(memAddress);
                    for (int maskIndex = 0; maskIndex < 36; maskIndex++)
                    {
                        switch (mask[maskIndex])
                        {
                            case '1':
                                sb[maskIndex] = '1';
                                break;
                            case 'X':
                                sb[maskIndex] = '0';
                                break;
                        }
                    }
                    memAddress = sb.ToString();
                    long powNbFloatingBits = (long)Math.Pow(2, nbFloatingBits);
                    /* We count all the floating bits, number of possible combinations is 2^n with n= number of floating bits
                     * we run a loop for each value between 0 and 2^n. We convert it to binary which will give us a number with maximum n bits
                     * we add padding 0 to have enough bits. And we take each bit of this number and assign its value to the corresponding floating bit index.
                     * This way we can try all possible variations of floating bits in the memory address ! */

                    for (long index = 0; index < powNbFloatingBits; index++)
                    {
                        string binaryIndex = Convert.ToString(index, 2).PadLeft(nbFloatingBits, '0');// We will run this loop for all possible values of floating bits in the mask
                        sb = new StringBuilder(memAddress);
                        int floatingBitCount=0;
                        foreach (int bitIndex in floatingBitsIndexes)
                        {
                            sb[bitIndex] = binaryIndex[floatingBitCount];
                            floatingBitCount++;
                        }
                        memAddressMasked = sb.ToString();
                        memAddressNumeric = Convert.ToInt64(memAddressMasked,2); // We get the final value of the altered memory address
                        if (!addressMap.ContainsKey(memAddressNumeric))
                            addressMap.TryAdd(memAddressNumeric, memValue);
                        else
                        {
                            addressMap.Remove(memAddressNumeric);
                            addressMap.TryAdd(memAddressNumeric, memValue);
                        }
                    }
                }
            }
            foreach (long value in addressMap.Values)
            {
                sum += value;
            }

            return sum;
        }
    }
}
