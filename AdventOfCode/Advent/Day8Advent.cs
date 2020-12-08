using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day8Advent: BaseAdvent
    {
        public enum InstructionType
        {
            acc,
            jmp,
            nop
        }

        public class Instruction
        {
            public InstructionType type;
            public int value;

            public Instruction(InstructionType type, int value)
            {
                this.type = type;
                this.value = value;
            }
        }

        private List<Instruction> instructions;

        private Dictionary<string, InstructionType> pairedInstructionType = new Dictionary<string, InstructionType>()
        {
            {InstructionType.acc.ToString(), InstructionType.acc},
            {InstructionType.jmp.ToString(), InstructionType.jmp},
            {InstructionType.nop.ToString(), InstructionType.nop},
        };
        
        public Day8Advent(string filePath) : base(filePath)
        {
            instructions = new List<Instruction>();
        }

        public override void Execute()
        {
            ReadFileAndParse();
            EvaluateInstructions();
        }

        private void ReadFileAndParse()
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                var current = lines[i];
                InstructionType type = pairedInstructionType[current.Substring(0, 3)];
                int value = int.Parse(current.Substring(current.IndexOf(" ", StringComparison.Ordinal) + 1));
                
                instructions.Add(new Instruction(type, value));
            }
        }

        private void EvaluateInstructions()
        {
            int accumulation = 0;

            //bool successfulFinished = AnalyzeInstructions(ref accumulation);

            for (int i = 0; i < instructions.Count; i++)
            {
                accumulation = 0;
                
                Instruction currentInstruction = instructions[i];
                if (currentInstruction.type == InstructionType.acc)
                    continue;

                var lastType = currentInstruction.type;
                if (currentInstruction.type == InstructionType.jmp)
                    currentInstruction.type = InstructionType.nop;
                else
                    currentInstruction.type = InstructionType.jmp;
                
                bool successfulFinished = AnalyzeInstructions(ref accumulation);

                currentInstruction.type = lastType;

                if (successfulFinished)
                {
                    Console.WriteLine("Fixed problem");
                    break;
                }
            }

            Console.WriteLine("Total accumulation is: "+accumulation);
        }

        private bool AnalyzeInstructions(ref int accumulation)
        {
            int instructionIndex = 0;
            List<int> executedInstructions = new List<int>();
            Instruction currentInstruction = instructions[instructionIndex];
            
            while (!executedInstructions.Contains(instructionIndex) && instructionIndex < instructions.Count) 
            {
                executedInstructions.Add(instructionIndex);
                EvaluateInstruction(currentInstruction, ref accumulation, ref instructionIndex);
                
                if (instructionIndex < instructions.Count)
                    currentInstruction = instructions[instructionIndex];
            }

            return instructionIndex >= instructions.Count;
        }
    
        private void EvaluateInstruction(Instruction currentInstruction, ref int accumulation, ref int instructionIndex)
        {
            switch (currentInstruction.type)
            {
                case InstructionType.acc:
                    accumulation += currentInstruction.value;
                    instructionIndex++;
                    break;
                case InstructionType.jmp:
                    instructionIndex += currentInstruction.value;
                    break;
                case InstructionType.nop:
                    instructionIndex++;
                    break;
            }
        }
    }
}