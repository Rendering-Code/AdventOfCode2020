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
            
            BaseAdvent advent = new Day9Advent("..\\..\\AdventFiles\\Day9Advent.txt", 25);
            advent.Execute();
            
            Console.WriteLine("Performance stopwatch in miliseconds: "+watch.ElapsedMilliseconds);
        }
    }
}