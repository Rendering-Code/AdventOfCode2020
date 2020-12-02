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
            
            Day2Advent advent = new Day2Advent("..\\..\\AdventFiles\\Day2Advent2.txt");
            advent.Execute();
            
            Console.WriteLine("Performance stopwatch in miliseconds: "+watch.ElapsedMilliseconds);
        }
    }
}