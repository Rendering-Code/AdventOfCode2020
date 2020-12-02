using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day1Advent
    {
        private int expectedResult;
        private int evaluationDepth;
        private string filePath;
        
        public Day1Advent(int expectedResult, int evaluationDepth, string filePath)
        {
            this.expectedResult = expectedResult;
            this.evaluationDepth = evaluationDepth;
            this.filePath = filePath;
        }

        public void Execute()
        {
            var numbers = ReadFile();
            if (numbers.Length < evaluationDepth)
            {
                Console.WriteLine("Can't evaluate correctly, depth is bigger than data set count");
                return;
            }
            EvaluateRecursively(numbers, new int[evaluationDepth], 0, 0, evaluationDepth);
        }

        private int[] ReadFile()
        {
            var lines = File.ReadAllLines(filePath);
            int[] numbers = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                numbers[i] = Convert.ToInt32(lines[i]);
            
            return numbers;
        }

        private void EvaluateRecursively(int[] numbers, int[] currentNumbers, int currentIndex, int depth, int maxDepth)
        {
            if (depth+1 == maxDepth)
            {
                var currentOperation = 0;
                for (int i = 0; i < currentNumbers.Length; i++)
                    currentOperation += currentNumbers[i];
                
                for (int i = currentIndex; i < numbers.Length; i++)
                {
                    var current = numbers[i];
                    if (currentOperation + current == expectedResult)
                    {
                        currentNumbers[depth] = current;
                        int multiplicationResult = 1;
                        for (int j = 0; j < currentNumbers.Length; j++)
                            multiplicationResult *= currentNumbers[j];
                        Console.WriteLine(multiplicationResult);
                    }
                }

                return;
            }

            for (int i = currentIndex; i < numbers.Length-1; i++)
            {
                currentNumbers[depth] = numbers[i];
                EvaluateRecursively(numbers, currentNumbers, i + 1, depth+1, maxDepth);
            }
        }
    }
}