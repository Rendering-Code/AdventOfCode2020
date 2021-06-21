using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day18Advent : BaseAdvent
    {
        public Day18Advent(string filePath) : base(filePath)
        {
        }

        public override void Execute()
        {
            string[] lines = File.ReadAllLines(filePath);

            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                int expressionIndex = 0;
                result += EvaluateExpresion(ref expressionIndex, lines[i].Replace(" ", string.Empty));
            }

            Console.WriteLine("Total sum is: "+result);
        }

        private long EvaluateExpresion(ref int currentIndex, string expression)
        {
            List<long> values = new List<long>();
            List<char> currentOperation = new List<char>();

            for (; currentIndex < expression.Length; currentIndex++)
            {
                var current = expression[currentIndex];

                if (current == '+' || current == '*')
                {
                    currentOperation.Add(current);
                }
                else if (current == '(')
                {
                    currentIndex++;
                    values.Add(EvaluateExpresion(ref currentIndex, expression));
                }
                else if (current == ')')
                {
                    break;
                }
                else
                {
                    long currentValue = Convert.ToInt64(Convert.ToString(current));
                    values.Add(currentValue);
                }
            }

            return EvaluateWithPriority(values, currentOperation);
        }

        private long EvaluateWithPriority(List<long> numbers, List<char> operations)
        {
            List<long> secondPass = new List<long>();
            secondPass.Add(numbers[0]);

            for (int i = 0; i < operations.Count; i++)
            {
                if (operations[i] == '*')
                {
                    secondPass.Add(numbers[i+1]);
                }
                if (operations[i] == '+')
                {
                    secondPass[secondPass.Count-1] += numbers[i+1];
                }
            }

            long result = 1;
            foreach (var value in secondPass)
                result *= value;

            return result;
        }
    }
}