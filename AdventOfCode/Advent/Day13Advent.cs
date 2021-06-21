using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Advent;

public class Day13Advent : BaseAdvent
{
    public Day13Advent(string filePath) : base(filePath)
    {
    }
    
    public override void Execute()
    {
        string[] lines = File.ReadAllLines(filePath);
        int time = Convert.ToInt32(lines[0]);
        string[] busses = lines[1].Split(',');
        //AnalyzeBussesSimple(time, busses);
        //AnalyzeBussesComplex(busses);
        
        Console.WriteLine(File.ReadLines(filePath)
            .Last(s => !string.IsNullOrWhiteSpace(s))
            .Split(',')
            .Select((s, i) => new KeyValuePair<int, int>(i, (s == "x") ? 1 : int.Parse(s)))
            .Aggregate(new { T = 0L, Lcm = 1L }, (acc, b) =>
                new
                {
                    T = Enumerable.Range(0, Int32.MaxValue)
                        .Select(n => acc.T + (acc.Lcm * n))
                        .First((n) => (n + b.Key) % b.Value == 0),
                    Lcm = acc.Lcm * b.Value
                }
            )
            .T);
    }

    private void AnalyzeBussesSimple(int time, string[] busses)
    {
        int minTimeToWait = int.MaxValue;
        int busId = -1;
        Console.WriteLine("Time is: " + time);
        for (int i = 0; i < busses.Length; i++)
        {
            var current = busses[i];
            if (current == "x")
                continue;

            var currentId = int.Parse(current);
            var iterations = (time / currentId) + 1;
            var timeToWait = currentId * iterations - time;
            if (timeToWait < minTimeToWait)
            {
                minTimeToWait = timeToWait;
                busId = currentId;
            }
        }
        Console.WriteLine("Result is: " + minTimeToWait * busId);
    }

    private void AnalyzeBussesComplex(string[] busses)
    {
        Dictionary<int, int> bussesId = new Dictionary<int, int>();

        for (int i = 0; i < busses.Length; i++)
        {
            if (int.TryParse(busses[i], out var value))
                bussesId.Add(i, value);
        }

        var keys = bussesId.Keys.ToArray();
        long currentMultiplier = 100000000000000/bussesId[keys[0]];
        bool found = false;
        while (!found)
        {
            found = true;
            currentMultiplier++;
            long multipliedValue = bussesId[keys[0]] * currentMultiplier;

            for (int i = 1; i < keys.Length; i++)
            {
                var currentKey = keys[i];
                bool validNumber = (multipliedValue + currentKey) % bussesId[currentKey] == 0;
                if (!validNumber)
                {
                    found = false;
                    break;
                }
            }
        }

        Console.WriteLine("Closes time is: "+currentMultiplier* bussesId[keys[0]]);
    }
}