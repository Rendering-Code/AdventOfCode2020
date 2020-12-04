using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent
{
    public class Day4Advent
    {
        private const string RegexUntilSpaceCapture = "([^\\s]+)";
        
        private string filePath;
        private bool useSoftPolicy;

        private Tuple<Regex, Predicate<string>>[] mustFields;
        private Tuple<Regex, Predicate<string>>[] optionalFields;
        
        private Regex hairColorRegex;
        private Regex cmHeightRegex;
        private Regex inHeightRegex;

        private string[] validEyeColors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        public Day4Advent(string filePath, bool useSoftPolicy)
        {
            this.filePath = filePath;
            this.useSoftPolicy = useSoftPolicy;

            mustFields = new[]
            {
                new Tuple<Regex, Predicate<string>>(new Regex("byr:"+RegexUntilSpaceCapture), ValidateByr),
                new Tuple<Regex, Predicate<string>>(new Regex("iyr:"+RegexUntilSpaceCapture), ValidateIyr),
                new Tuple<Regex, Predicate<string>>(new Regex("eyr:"+RegexUntilSpaceCapture), ValidateEyr),
                new Tuple<Regex, Predicate<string>>(new Regex("hgt:"+RegexUntilSpaceCapture), ValidateHgt),
                new Tuple<Regex, Predicate<string>>(new Regex("hcl:"+RegexUntilSpaceCapture), ValidateHcl),
                new Tuple<Regex, Predicate<string>>(new Regex("ecl:"+RegexUntilSpaceCapture), ValidateEcl),
                new Tuple<Regex, Predicate<string>>(new Regex("pid:"+RegexUntilSpaceCapture), ValidatePid),
            };
            
            optionalFields = new[]
            {
                new Tuple<Regex, Predicate<string>>(new Regex("cid:"), null),
            };
            
            hairColorRegex = new Regex("#(?:[0-9a-fA-F]{3}){1,2}");
            cmHeightRegex = new Regex("(\\d*)cm");
            inHeightRegex = new Regex("(\\d*)in");
        }

        public void Execute()
        {
            var credentials = ReadFile();

            int validCredentials = 0;
            for (int i = 0; i < credentials.Length; i++)
            {
                if (IsValidCredential(credentials[i], i))
                {
                    validCredentials++;
                }
            }

            Console.WriteLine("Result is: "+validCredentials);
        }
        
        private string[] ReadFile()
        {
            List<string> credentials = new List<string>();
            string[] allLines = File.ReadAllLines(filePath);

            string currentCredential = string.Empty;

            for (int i = 0; i < allLines.Length; i++)
            {
                var currentLine = allLines[i];
                if (string.IsNullOrEmpty(currentLine))
                {
                    credentials.Add(currentCredential);
                    currentCredential = string.Empty;
                    continue;
                }

                currentCredential += currentLine + " ";
            }
            
            credentials.Add(currentCredential);
            
            return credentials.ToArray();
        }

        private bool IsValidCredential(string credential, int index)
        {
            bool isValid = true;

            for (int i = 0; i < mustFields.Length; i++)
            {
                var match = mustFields[i].Item1.Match(credential);
                isValid &= match.Success && mustFields[i].Item2(match.Groups[1].Value);
                if (!isValid)
                    break;
            }
            
            return isValid;
        }
        
        private bool ValidateByr(string value)
        {
            if(useSoftPolicy)
                return true;

            if (value.Length != 4)
                return false;

            return EvaluateNumberInRange(value, 1920, 2002);
        }
        
        private bool ValidateIyr(string value)
        {
            if(useSoftPolicy)
                return true;
                
            if (value.Length != 4)
                return false;

            return EvaluateNumberInRange(value, 2010, 2020);
        }
        
        private bool ValidateEyr(string value)
        {
            if(useSoftPolicy)
                return true;
               
            if (value.Length != 4)
                return false;

            return EvaluateNumberInRange(value, 2020, 2030);
        }

        private bool EvaluateNumberInRange(string value, int minValue, int maxValue)
        {
            bool valid = int.TryParse(value, out var resultValue);
            return valid && resultValue >= minValue && resultValue <= maxValue;
        }
        
        private bool ValidateHgt(string value)
        {
            if(useSoftPolicy)
                return true;

            var matchIn = inHeightRegex.Match(value);
            var matchCm = cmHeightRegex.Match(value);
            if (!matchIn.Success && !matchCm.Success)
                return false;

            if (matchIn.Success)
            {
                string inMatchValue = matchIn.Groups[1].Value;
                return EvaluateNumberInRange(inMatchValue, 59, 76);
            }
            else
            {
                string cmMatchValue = matchCm.Groups[1].Value;
                return EvaluateNumberInRange(cmMatchValue, 150, 193);
            }
        }
        
        private bool ValidateHcl(string value)
        {
            if(useSoftPolicy)
                return true;

            var match = hairColorRegex.Match(value);
            return match.Success && match.Groups[1].Value.Length != 6;
        }
        
        private bool ValidateEcl(string value)
        {
            if(useSoftPolicy)
                return true;
                
            return validEyeColors.Contains(value);
        }
        
        private bool ValidatePid(string value)
        {
            if(useSoftPolicy)
                return true;
                
            if (value.Length != 9)
                return false;
            
            return int.TryParse(value, out int result);
        }
    }
}