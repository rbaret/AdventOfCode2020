
using System;
using System.IO;
using System.Collections.Generic;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day8\\input.txt";
            List<Tuple<String, int, bool>> instructions = GenerateInstructionsList(@path);
            Tuple<int, int> accAndInstr = ExecInstructions(instructions);
            Console.WriteLine("Part 1 : acc value before bootloop :"+accAndInstr.Item1+"\n");
            instructions = new List<Tuple<string, int, bool>>(GenerateInstructionsList(@path));
            accAndInstr = FixCorruptedInstruction(instructions);
            Console.WriteLine("\nPart 2 : new acc value eafter bootloop fix : "+accAndInstr.Item1);
        }

        private static List<Tuple<String, int, bool>> GenerateInstructionsList(string path) // Useful to generate a brand new isntructions list on demand
        {
            string[] lines = File.ReadAllLines(@path);
            List<Tuple<String, int, bool>> instructions = new List<Tuple<string, int, bool>>();
            foreach (string line in lines)
            {
                string action = line.Substring(0, 3);
                int value = int.Parse(line.Substring(4));
                instructions.Add(new Tuple<string, int, bool>(action, value, false));
            }
            return instructions;
        }

        static Tuple<int, int> ExecInstructions(List<Tuple<String, int, bool>> instructions) // Returns accumulator value and index of the last instruction executed
        {
            int index = 0;
            int acc = 0;
            bool hasBeenExecuted = false; // To detect loops
            while (!hasBeenExecuted && (index < instructions.Count) && (index >= 0))
            {
                Tuple<string, int, bool> newInst;
                switch (instructions[index].Item1)
                {
                    case "jmp":
                        newInst = new Tuple<string, int, bool>("jmp", instructions[index].Item2, true);
                        instructions.RemoveAt(index);
                        instructions.Insert(index, newInst);
                        index += instructions[index].Item2;
                        break;
                    case "acc":
                        acc += instructions[index].Item2;
                        newInst = new Tuple<string, int, bool>("acc", instructions[index].Item2, true);
                        instructions.RemoveAt(index);
                        instructions.Insert(index, newInst);
                        index++;
                        break;
                    default:
                        newInst = new Tuple<string, int, bool>(instructions[index].Item1, instructions[index].Item2, true);
                        instructions.RemoveAt(index);
                        instructions.Insert(index, newInst);
                        index++;
                        break;
                }
                if (index == instructions.Count) // Specific to the detection of the last step
                    hasBeenExecuted = true;
                else
                    hasBeenExecuted = instructions[index].Item3; // Checking status of the enxt instruction
            }
            return new Tuple<int, int>(acc, index);
        }
        // Part 2
        static Tuple<int,int> FixCorruptedInstruction(List<Tuple<String, int, bool>> instructions)
        {
            int index = 0;
            bool found = false;
            Tuple<int, int> result;
            do // Dirty ol' bruteforce to test changing all jmp and nop instructions 1 by 1 and run the full program until boot is ok
            {
                List<Tuple<string,int,bool>> tempInstructions = new List<Tuple<string, int, bool>>(instructions);
                Tuple<String, int, bool> newInst;
                switch (instructions[index].Item1) //Instrution inversion
                {
                    case ("jmp"):
                        newInst = new Tuple<String, int, bool>("nop", instructions[index].Item2, false);
                        break;
                    case ("nop"):
                        newInst = new Tuple<String, int, bool>("jmp", instructions[index].Item2, false);
                        break;
                    default:
                        newInst = null;
                        break;
                }
                if (newInst != null){ 
                    tempInstructions.RemoveAt(index);
                    tempInstructions.Insert(index, newInst); 
                }
                result = ExecInstructions(tempInstructions);

                if (result.Item2 == instructions.Count)
                    found = true;
                index++;
            } while (index < instructions.Count && !found);

            if (found)
            {
                Console.WriteLine("Faulty instruction was " + instructions[index - 1].Item1 + " " + instructions[index - 1].Item2+" at line : "+index);
            }
            else Console.WriteLine("Not found :(");
            return result;
        }
    }
}
