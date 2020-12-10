using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Advent
{
    public class Day10Advent : BaseAdvent
    {
        private int maxAdaptersDifference;
        
        public Day10Advent(string filePath, int maxAdaptersDifference) : base(filePath)
        {
            this.maxAdaptersDifference = maxAdaptersDifference;
        }

        public override void Execute()
        {
            var adapters = ReadFile();
            //AnalyzeChargers(adapters);
            long totalBranches = FindReferencesRecursively(adapters, maxAdaptersDifference);
            Console.WriteLine("Total branches: "+totalBranches);
        }

        private int[] ReadFile()
        {
            List<int> adapters = new List<int>(){0};
            adapters.AddRange(File.ReadAllLines(filePath).Select(int.Parse).OrderBy(x => x).ToList());
            adapters.Add(adapters.Max()+maxAdaptersDifference);
            return adapters.ToArray();
        }

        private void AnalyzeChargers(int[] adapters)
        {
            List<int> adaptersDifference = new List<int>();

            for (int i = 0; i < adapters.Length-1; i++)
                adaptersDifference.Add(adapters[i+1]-adapters[i]);

            var oneDifference = adaptersDifference.Count(x => x == 1);
            var threeDifference = adaptersDifference.Count(x => x == 3);
            Console.WriteLine("Result is: "+ oneDifference*threeDifference);
        }

        private long FindReferencesRecursively(int[] adapters, int maxAdapterSize)
        {
            Dictionary<int, long> branches = new Dictionary<int, long>();

            int maxAdapter = adapters[adapters.Length - 1];

            for (int i = adapters.Length-2; i >= 0; i--)
            {
                var currentAdapter = adapters[i];
                long totalBranches = 0;
                
                for (int j = 1; j <= maxAdapterSize; j++)
                {
                    var currentInnerAdapter = currentAdapter + j;
                    if (currentInnerAdapter >= maxAdapter)
                    {
                        totalBranches++;
                        break;
                    }

                    if (branches.TryGetValue(currentInnerAdapter, out long value))
                        totalBranches += value;
                }

                branches.Add(currentAdapter, totalBranches);
            }
            
            return branches[0];
        }
    }
}