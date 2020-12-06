using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace AdventOfCode.Advent
{
    public class Day6Advent : BaseAdvent
    {
        public Day6Advent(string filePath) : base(filePath)
        {
        }

        public override void Execute()
        {
            //var unicCharsPerGroup = ParseDataAnyoneYes(ReadFile());
            var unicCharsPerGroup = ParseDataEveryoneYes(ReadFile());

            int totalUniqueAnsers = 0;
            for (int i = 0; i < unicCharsPerGroup.Count; i++)
                totalUniqueAnsers += unicCharsPerGroup[i].Count;

            Console.WriteLine("Result is: "+totalUniqueAnsers);
        }
        
        private string[] ReadFile()
        {
            return File.ReadAllLines(filePath);
        }

        private List<List<char>> ParseDataAnyoneYes(string[] lines)
        {
            List<List<char>> usersGroup = new List<List<char>>();
            usersGroup.Add(new List<char>());
            
            for (int i = 0; i < lines.Length; i++)
            {
                var current = lines[i];
                if (string.IsNullOrEmpty(current))
                {
                    usersGroup.Add(new List<char>());
                    continue;
                }

                var currentList = usersGroup[usersGroup.Count - 1];
                foreach (var letter in current)
                {
                    if (!currentList.Contains(letter))
                        currentList.Add(letter);
                }
            }

            return usersGroup;
        }
        
        private List<List<char>> ParseDataEveryoneYes(string[] lines)
        {
            List<List<char>> usersGroup = new List<List<char>>();
            List<char> auxiliar = new List<char>();
            int groupMembers = 0;
            
            for (int i = 0; i < lines.Length; i++)
            {
                var current = lines[i];
                if (string.IsNullOrEmpty(current))
                {
                    AnalyzeGroup(usersGroup, auxiliar, ref groupMembers);
                    continue;
                }

                foreach (var letter in current)
                    auxiliar.Add(letter);

                groupMembers++;
            }
            
            AnalyzeGroup(usersGroup, auxiliar, ref groupMembers);

            return usersGroup;
        }

        private void AnalyzeGroup(List<List<char>> usersGroup, List<char> auxiliar, ref int groupMembers)
        {
            List<char> validChars = new List<char>();

            for(int j = 0; j < auxiliar.Count; j++)
            {
                var firstLetter = auxiliar[j];
                var matches = auxiliar.Count(x => x == firstLetter);

                if (matches == groupMembers && !validChars.Contains(firstLetter))
                    validChars.Add(firstLetter);
            }
                    
            usersGroup.Add(validChars);
            auxiliar.Clear();
            groupMembers = 0;
        }
    }
}