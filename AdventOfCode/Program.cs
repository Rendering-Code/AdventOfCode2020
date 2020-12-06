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
            
            BaseAdvent advent = new Day6Advent("..\\..\\AdventFiles\\Day6Advent.txt");
            advent.Execute();
            
            Console.WriteLine("Performance stopwatch in miliseconds: "+watch.ElapsedMilliseconds);
        }
    }
}