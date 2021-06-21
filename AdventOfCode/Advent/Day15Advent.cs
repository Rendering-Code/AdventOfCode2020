using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Advent
{
    public class Day15Advent : BaseAdvent
    {
        private int desiredNumber;
        
        public Day15Advent(string filePath, int desiredNumber) : base(filePath)
        {
            this.desiredNumber = desiredNumber;
        }

        public override void Execute()
        {
            int[] number = File.ReadAllText(filePath).Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            
            Dictionary<int, int> numbersRound = new Dictionary<int, int>();
            for (int i = 0; i < number.Length-1; i++)
                numbersRound.Add(number[i],i+1);

            int currentRound = number.Length;
            int lastSpokenNumber = number[number.Length-1];
            while (currentRound < desiredNumber)
            {
                if (!numbersRound.ContainsKey(lastSpokenNumber))
                {
                    numbersRound.Add(lastSpokenNumber, currentRound);
                    //Console.WriteLine("Round: "+currentRound+". Number: "+lastSpokenNumber+" is new");
                    lastSpokenNumber = 0;
                }
                else
                {
                    int lastRoundSpoken = numbersRound[lastSpokenNumber];
                    numbersRound[lastSpokenNumber] = currentRound;
                    //Console.WriteLine("Round: "+currentRound+". Number: "+lastSpokenNumber+" is repeated, round shown: "+lastRoundSpoken+" passed: "+(currentRound - lastRoundSpoken)+" rounds");
                    lastSpokenNumber = currentRound - lastRoundSpoken;
                }
                
                currentRound++;
            }

            Console.WriteLine("Last spoken number is: "+lastSpokenNumber);
        }
    }
}