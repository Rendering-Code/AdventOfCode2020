using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent
{
    public abstract class BaseRule
    {
    }

    public class SingleCharRule : BaseRule
    {
        public string letter;

        public SingleCharRule(string letter)
        {
            this.letter = letter;
        }
    }

    public class SolvePointingRule : BaseRule
    {
        public List<int[]> dependentRules;

        public SolvePointingRule()
        {
            dependentRules = new List<int[]>();            
        }
    }
    
    public class Day19Advent: BaseAdvent
    {
        public Day19Advent(string filePath) : base(filePath)
        {
        }

        public override void Execute()
        {
            var lines = File.ReadAllLines(filePath);
            
            Regex ruleRegex = new Regex("(\\d*): (.*)");
            Dictionary<int, BaseRule> rulesSet = new Dictionary<int, BaseRule>();
            List<string> combinationsToMatch = new List<string>();

            bool recordingCombinations = false;
            
            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                
                if (string.IsNullOrEmpty(currentLine))
                {
                    recordingCombinations = true;
                    continue;
                }

                if (!recordingCombinations)
                {
                    var match = ruleRegex.Match(currentLine);
                    int ruleNumber = Convert.ToInt32(match.Groups[1].Value);
                    if (match.Groups[2].Value.Contains("\""))
                    {
                        rulesSet.Add(ruleNumber, new SingleCharRule(match.Groups[2].Value.Replace(" ", string.Empty).Replace("\"", string.Empty)));
                    }
                    else
                    {
                        string[] ruleDependencies = match.Groups[2].Value.Split('|');
                        var dependantRule = new SolvePointingRule();
                        for (int j = 0; j < ruleDependencies.Length; j++)
                            dependantRule.dependentRules.Add(ruleDependencies[j].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToArray());
                        rulesSet.Add(ruleNumber, dependantRule);
                    }
                }
                else
                {
                    combinationsToMatch.Add(currentLine);
                }
            }
            
            Dictionary<int, List<string>> solvedValues = new Dictionary<int, List<string>>();

            List<string> allValidRules = EvaluateRule(0, rulesSet, solvedValues);

            int validMatches = 0;
            foreach (var combinations in combinationsToMatch)
            {
                if (allValidRules.Contains(combinations))
                    validMatches++;
            }

            Console.WriteLine("Result: "+validMatches);
        }

        private List<string> EvaluateRule(int ruleIndex, Dictionary<int, BaseRule> rulesSet, Dictionary<int, List<string>> solvedValues)
        {
            var currentRule = rulesSet[ruleIndex];
            
            List<string> puzzle = new List<string>();
            
            if (currentRule is SolvePointingRule solveRule)
            {
                for (int i = 0; i < solveRule.dependentRules.Count; i++)
                {
                    var current = solveRule.dependentRules[i];
                    foreach (var expr in rulesSet) //TODO: Modify
                    {
                        StringBuilder builder = new StringBuilder();

                        
                    }
                }
                
            }
        }
    }
}