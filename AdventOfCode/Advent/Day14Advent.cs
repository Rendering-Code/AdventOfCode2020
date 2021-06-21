using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Advent
{
    public class Day14Advent : BaseAdvent
    {
        public Day14Advent(string filePath) : base(filePath)
        {
        }

        public override void Execute()
        {
            string[] lines = File.ReadAllLines(filePath);

            string currentMask = string.Empty;
            long baseNumber = 0;
            long mask = 0;
            Dictionary<long, long> bitsMemory = new Dictionary<long, long>();

            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                if (currentLine.Contains("mask"))
                {
                    baseNumber = 0;
                    mask = 0;

                    currentMask = currentLine.Replace("mask = ", string.Empty);
                    
                    StringBuilder builder = new StringBuilder();
                    for (int j = currentMask.Length-1; j >= 0; j--)
                        builder.Append(currentMask[j]);
                    currentMask = builder.ToString();
                }
                else
                {
                    int addressMemory = Convert.ToInt32(currentLine.Substring(4, currentLine.IndexOf("]", StringComparison.Ordinal)-4));
                    StringBuilder addressBuilder = new StringBuilder();
                    
                    for (int j = 0; j < currentMask.Length; j++)
                    {
                        var current = currentMask[j];
                        if (current.Equals('X'))
                        {
                            addressBuilder.Append("X");
                        }
                        else if(current.Equals('1'))
                        {
                            addressBuilder.Append("1");
                        }
                        else if (current.Equals('0'))
                        {
                            long bitValue = (long)Math.Pow(2, j);
                            addressBuilder.Append((addressMemory & bitValue) == bitValue ? "1" : "0");
                        }
                    }

                    List<long> addresses = new List<long>();
                    GetAddressRecursive(0, addressBuilder.ToString().ToCharArray()).ForEach(x => addresses.Add(ParseAddress(x)));
                    
                    int numberToWrite = Convert.ToInt32(currentLine.Substring(currentLine.IndexOf("=", StringComparison.Ordinal)+1));

                    for (int j = 0; j < addresses.Count; j++)
                    {
                        var currentMemory = addresses[j];
                        if (!bitsMemory.ContainsKey(currentMemory))
                            bitsMemory.Add(currentMemory, numberToWrite);
                        else
                            bitsMemory[currentMemory] = numberToWrite;
                    }
                }
            }

            long result = 0;
            foreach (var bit in bitsMemory)
                result += bit.Value;

            Console.WriteLine("Result is: "+result);
        }

        private List<string> GetAddressRecursive(int currentIndex, char[] filteredAddress)
        {
            List<string> addresses = new List<string>();

            char[] copies = new char[filteredAddress.Length];
            filteredAddress.CopyTo(copies, 0);

            bool foundBool = false;

            while (currentIndex < copies.Length)
            {
                if (copies[currentIndex].Equals('X'))
                {
                    foundBool = true;
                    copies[currentIndex] = '0';
                    addresses.AddRange(GetAddressRecursive(currentIndex+1, copies));
                    copies[currentIndex] = '1';
                    addresses.AddRange(GetAddressRecursive(currentIndex+1, copies));
                    break;
                }

                currentIndex++;
            }

            if (!foundBool)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var copy in copies)
                    builder.Append(copy);
                addresses.Add(builder.ToString());
            }

            return addresses;
        }

        private static long ParseAddress(string address)
        {
            long finalValue = 0;
            for (int i = 0; i < address.Length; i++)
            {
                int value = address[i].Equals('1') ? 1 : 0;
                finalValue += value * (long)Math.Pow(2, i);
            }
            return finalValue;
        }
    }
}