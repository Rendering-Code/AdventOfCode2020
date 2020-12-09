using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Advent
{
    public class Day9Advent : BaseAdvent
    {
        private int preambleLength;
        
        public Day9Advent(string filePath, int preambleLength) : base(filePath)
        {
            this.preambleLength = preambleLength;
        }

        public override void Execute()
        {
            long[] numbers = ReadFileAndParse();
            long nonMatchingNumber = AnalyzeNumbers(numbers);
            var result = AnalyzeWeakness(numbers, nonMatchingNumber);
            Console.WriteLine("Result weakness is: "+result);
        }

        private long[] ReadFileAndParse()
        {
            var lines = File.ReadAllLines(filePath);
            long[] numbers = new long[lines.Length];
            for(int i = 0; i < lines.Length; i++)
                numbers[i] = long.Parse(lines[i]);

            return numbers;
        }

        private long AnalyzeNumbers(long[] numbers)
        {
            for (long i = 0; i + preambleLength < numbers.Length; i++)
            {
                var current = numbers[i + preambleLength];

                bool numberFound = false;
                for (long j = 0; j < preambleLength; j++)
                {
                    var numberToFind = current - numbers[j + i];
                    numberFound |= SearchNumberInRange(numberToFind, numbers, i, i+preambleLength, j+i);
                    if (numberFound)
                        break;
                }

                if (!numberFound)
                {
                    Console.WriteLine("The number: "+current+" has not been found");
                    return current;
                }
            }

            return 0;
        }

        private bool SearchNumberInRange(long numberToFind, long[] numbers, long minIndex, long maxIndex, long indexToSkip)
        {
            for (long i = minIndex; i < maxIndex; i++)
            {
                var current = numbers[i];
                if (i == indexToSkip)
                    continue;

                bool numberFound = numberToFind == current;
                if (numberFound)
                    return true;
            }

            return false;
        }

        private long AnalyzeWeakness(long[] numbers, long numberToFind)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                long currentNumber = numbers[i];
                if (currentNumber == numberToFind)
                    return -1;

                var currentIndex = i;
                while (currentNumber < numberToFind)
                {
                    currentIndex++;
                    currentNumber += numbers[currentIndex];
                }

                if (currentNumber == numberToFind)
                    return FindMinNumber(numbers, i, currentIndex) + FindMaxNumber(numbers, i, currentIndex);
            }

            return -1;
        }

        private long FindMinNumber(long[] number, long minIndex, long maxIndex)
        {
            long minNumber = long.MaxValue;
            
            for (long i = minIndex; i <= maxIndex; i++)
            {
                var current = number[i];
                if (current<minNumber)
                    minNumber = current;
            }

            return minNumber;
        }

        private long FindMaxNumber(long[] number, long minIndex, long maxIndex)
        {
            long maxNumber = long.MinValue;
            
            for (long i = minIndex; i <= maxIndex; i++)
            {
                var current = number[i];
                if (current>maxNumber)
                    maxNumber = current;
            }

            return maxNumber;
        }
    }
}