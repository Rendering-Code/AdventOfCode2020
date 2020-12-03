using System;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day3Advent
    {
        private int[] rightSteps;
        private int[] downSteps;
        private string filePath;
        private char treeChar;
        
        public Day3Advent(int[] rightSteps, int[] downSteps, string filePath, char treeChar)
        {
            this.rightSteps = rightSteps;
            this.downSteps = downSteps;
            this.filePath = filePath;
            this.treeChar = treeChar;
        }

        public void Execute()
        {
            var lines = ReadFile();

            int[] totalTreesFound = new int[rightSteps.Length];
            
            for (int i = 0; i < rightSteps.Length; i++)
                totalTreesFound[i] = EvaluateMapRecursively(lines, rightSteps[i], downSteps[i], 0, 0, 0);

            long multiplied = 1;
            for (int i = 0; i < totalTreesFound.Length; i++)
            {
                multiplied *= totalTreesFound[i];
                Console.WriteLine("Trees found for rule ("+rightSteps[i]+" right / "+downSteps[i]+" down): "+totalTreesFound[i]);
            }

            Console.WriteLine("Result is: "+multiplied);
        }
        
        private string[] ReadFile()
        {
            return File.ReadAllLines(filePath);
        }

        private int EvaluateMapRecursively(string[] lines, int rightStep, int downStep, int xPosition, int yPosition, int treesFound)
        {
            if (yPosition >= lines.Length)
                return treesFound;

            var currentLine = lines[yPosition];
            if (lines[yPosition][xPosition%currentLine.Length] == treeChar)
                treesFound++;

            yPosition += downStep;
            xPosition += rightStep;
            
            return EvaluateMapRecursively(lines, rightStep, downStep, xPosition, yPosition, treesFound);
        }
    }
}