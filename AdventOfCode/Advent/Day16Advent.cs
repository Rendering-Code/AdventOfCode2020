using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent
{
    public enum LecturesType
    {
        Intervals,
        Own,
        Theirs
    }

    public class Intervals
    {
        public int min;
        public int max;

        public Intervals(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public bool IsBetween(int value)
        {
            return value >= min && value <= max;
        }
    }
    
    public class Day16Advent : BaseAdvent
    {
        public Day16Advent(string filePath) : base(filePath)
        {
        }

        public override void Execute()
        {
            string[] lines = File.ReadAllLines(filePath);
            var numberRegex = new Regex("([^\\:]+)..(\\d*)-(\\d*) .* (\\d*)-(\\d*)");

            Dictionary<int, Tuple<string, List<Intervals>>> intervals = new Dictionary<int, Tuple<string, List<Intervals>>>();
            
            LecturesType currentLecture = LecturesType.Intervals;
            int[] myTickets = new int[0];
            List<int[]> otherTickets = new List<int[]>();
            
            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];

                if (string.IsNullOrEmpty(currentLine))
                {
                    currentLecture += 1;
                    continue;
                }

                switch (currentLecture)
                {
                    case LecturesType.Intervals:
                        var matches = numberRegex.Match(currentLine);
                        var intervalList = new List<Intervals>();
                        intervalList.Add(new Intervals(Convert.ToInt32(matches.Groups[2].Value), Convert.ToInt32(matches.Groups[3].Value)));
                        intervalList.Add(new Intervals(Convert.ToInt32(matches.Groups[4].Value), Convert.ToInt32(matches.Groups[5].Value)));
                        intervals.Add(i, new Tuple<string, List<Intervals>>(matches.Groups[1].Value, intervalList));
                        break;
                    case LecturesType.Own:
                        if (!currentLine.Contains("ticket"))
                            myTickets = currentLine.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                        break;
                    case LecturesType.Theirs:
                        if (!currentLine.Contains("ticket"))
                            otherTickets.Add(currentLine.Split(',').Select(x => Convert.ToInt32(x)).ToArray());
                        break;
                }
            }

            int totalValues = 0;
            
            List<int[]> validTickets = new List<int[]>();
            
            for (int i = 0; i < otherTickets.Count; i++)
            {
                var current = otherTickets[i];
                bool validTicket = true;
                for (int j = 0; j < current.Length; j++)
                {
                    bool foundInterval = false;
                    var currentNumber = current[j];
                    foreach (var values in intervals)
                    {
                        foundInterval = values.Value.Item2.Any(x => x.IsBetween(currentNumber));
                        if (foundInterval)
                            break;
                    }

                    if (!foundInterval)
                    {
                        validTicket = false;
                        break;
                    }
                }

                if (validTicket)
                    validTickets.Add(current);
            }
            
            Dictionary<string, List<int>> tagNames = new Dictionary<string, List<int>>();

            foreach (var interval in intervals)
            {
                var currentInterval = interval.Value.Item2;
                
                List<int> validsIndexes = new List<int>();
                for (int i = 0; i < validTickets[0].Length; i++)
                {
                    bool foundInterval = true;
                    for (int j = 0; j < validTickets.Count; j++)
                    {
                        var current = validTickets[j][i];
                        foundInterval &= currentInterval.Any(x => x.IsBetween(current));

                        if (!foundInterval)
                            break;
                    }

                    if (foundInterval)
                        validsIndexes.Add(i);
                }

                tagNames.Add(interval.Value.Item1, validsIndexes);
            }

            var ordereds = tagNames.OrderBy(x => x.Value.Count);
            Dictionary<int, string> analyzedTags = new Dictionary<int, string>();            

            foreach (var item in ordereds)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    var current = item.Value[i];
                    if (!analyzedTags.ContainsKey(current))
                    {
                        analyzedTags.Add(current, item.Key);
                        break;
                    }
                }
            }
            
            long result = 1;
            
            foreach (var tags in analyzedTags)
            {
                if (tags.Value.Contains("departure"))
                    result *= myTickets[tags.Key];
            }

            Console.WriteLine("Result: "+result);
        }
    }
}