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
            
            BaseAdvent advent = new Day5Advent("..\\..\\AdventFiles\\Day5Advent.txt", 128, 8);
            advent.Execute();
            
            Console.WriteLine("Performance stopwatch in miliseconds: "+watch.ElapsedMilliseconds);
        }
    }
}