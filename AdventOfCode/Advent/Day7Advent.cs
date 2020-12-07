using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day7Advent : BaseAdvent
    {
        private Dictionary<string, Dictionary<string, int>> bagsDictionary;
        
        public Day7Advent(string filePath) : base(filePath)
        {
            bagsDictionary = new Dictionary<string, Dictionary<string, int>>();
        }

        public override void Execute()
        {
            ReadFileAndParse();

            //var bagsDetected = FindParentBagRecursive("shiny gold", new List<string>());
            var bagsDetected = FindChildBagsRecursive("shiny gold");
            Console.WriteLine("Parent bags: "+bagsDetected);
        }
        
        private void ReadFileAndParse()
        {
            const string contains = " contain ";
            var lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                var current = lines[i];
                var mainBag = current.Substring(0, current.IndexOf(contains, StringComparison.Ordinal)).Replace(" bags", String.Empty).Replace(" bag", string.Empty);

                var otherBags = current.Substring(current.IndexOf(contains, StringComparison.Ordinal) + contains.Length).Replace(".", string.Empty).Replace(", ", ",").Split(',');
                var otherBagsDic = new Dictionary<string, int>();
                
                for (int j = 0; j < otherBags.Length; j++)
                {
                    var currentBag = otherBags[j];
                    if (currentBag.Contains("no other"))
                        continue;

                    var firstSpace = currentBag.IndexOf(" ", StringComparison.Ordinal);
                    int bagsCount = 0;
                    bagsCount = int.Parse(currentBag.Substring(0, firstSpace));
                    string bagName = currentBag.Substring(firstSpace + 1).Replace(" bags", String.Empty).Replace(" bag", string.Empty);
                    otherBagsDic.Add(bagName, bagsCount);
                }
                
                bagsDictionary.Add(mainBag, otherBagsDic);
            }
        }

        private List<string> FindParentBagRecursive(string bagToSearch, List<string> validBags)
        {
            foreach (var bags in bagsDictionary)
            {
                if (bags.Value.ContainsKey(bagToSearch))
                {
                    if (validBags.Contains(bags.Key))
                        continue;

                    validBags.Add(bags.Key);
                    FindParentBagRecursive(bags.Key, validBags);
                }
            }

            return validBags;
        }
        
        private int FindChildBagsRecursive(string bagToSearch)
        {
            int totalSum = 0;

            if (bagsDictionary.TryGetValue(bagToSearch, out var bags))
            {
                foreach (var childBags in bags)
                {
                    totalSum += FindChildBagsRecursive(childBags.Key) * childBags.Value + childBags.Value;
                }
            }

            return totalSum;
        }
    }
}