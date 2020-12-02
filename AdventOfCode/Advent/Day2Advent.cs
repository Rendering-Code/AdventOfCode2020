using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent
{
    public class Day2Advent
    {
        private string filePath;
        
        public Day2Advent(string filePath)
        {
            this.filePath = filePath;
        }
        
        public void Execute()
        {
            var lines = ReadFile();
            EvaluatePasswords(lines);
        }

        private string[] ReadFile()
        {
            return File.ReadAllLines(filePath);
        }

        private void EvaluatePasswords(string[] lines)
        {
            int totalValidPasswords = 0;
            Regex regex = new Regex("([0-9]*)-([0-9]*)\\s*(\\w):\\s*(\\w*)");
            
            for (int i = 0; i < lines.Length; i++)
            {
                var match = regex.Match(lines[i]);
                int minValue = Convert.ToInt32(match.Groups[1].Value);
                int maxValue = Convert.ToInt32(match.Groups[2].Value);
                char letter = Convert.ToChar(match.Groups[3].Value);
                string password = match.Groups[4].Value;

                if (ValidatePolicyNew(password, letter, minValue, maxValue))
                    totalValidPasswords++;
            }

            Console.WriteLine("Valid passwords: "+totalValidPasswords);
        }

        private bool ValidatePolicyOld(string password, char letter, int minValue, int maxValue)
        {
            int matchingLetters = 0;
            for (int j = 0; j < password.Length; j++)
            {
                if (password[j] == letter)
                    matchingLetters++;
            }

            return matchingLetters >= minValue && matchingLetters <= maxValue;
        }

        private bool ValidatePolicyNew(string password, char letter, int firstIndex, int secondIndex)
        {
            char firstLetter = password[firstIndex-1];
            char secondLetter = password[secondIndex-1];
            return firstLetter != secondLetter && (firstLetter == letter || secondLetter == letter);
        }
    }
}