using System;
using System.Diagnostics;
using AdventOfCode.Advent;

namespace AdventOfCode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch watch = Stopwatch.StartNew();
            
            Day3Advent advent = new Day3Advent(new int[]{1,3,5,7,1}, new int[]{1,1,1,1,2}, "..\\..\\AdventFiles\\Day3Advent.txt", '#');
            advent.Execute();
            
            Console.WriteLine("Performance stopwatch in miliseconds: "+watch.ElapsedMilliseconds);
        }
    }
}