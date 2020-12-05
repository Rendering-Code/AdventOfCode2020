using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode.Advent
{
    public class Day5Advent : BaseAdvent
    {
        private const int seatMultiplierIndex = 8;
        
        private const int rowIdentifierLength = 7;
        private const int columnIdentifierLength = 3;

        private const char backRowIdentifier = 'B';
        private const char frontRowIdentifier = 'F';
        
        private const char leftColumnIdentifier = 'L';
        private const char rightColumnIdentifier = 'R';
        
        private int maxRows;
        private int maxColumns;
        
        public Day5Advent(string filePath, int maxRows, int maxColumns) : base(filePath)
        {
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
        }
        
        public override void Execute()
        {
            var lines = ReadFile();

            //SearchSingleSeat(lines);
            SearchEmptySeat(lines);
        }

        private void SearchSingleSeat(string[] lines)
        {
            int higherSeat = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                int seatValue = EvaluateSeat(lines[i]);
                if (seatValue > higherSeat)
                    higherSeat = seatValue;
            }

            Console.WriteLine("Higher seat value is: "+higherSeat);
        }

        private void SearchEmptySeat(string[] lines)
        {
            List<int> allSeatsIndex = new List<int>();

            for (int i = 0; i < lines.Length; i++)
                allSeatsIndex.Add(EvaluateSeat(lines[i]));

            int value = 0;

            for (int i = 0; i < allSeatsIndex.Count; i++)
            {
                if (HasANearSeatEmpty(i, allSeatsIndex, out int seatIndex))
                {
                    value = seatIndex; 
                    break;
                }
            }
            
            Console.WriteLine("Your seat value is: "+value);
        }

        private bool HasANearSeatEmpty(int currentIndex, List<int> allSeats, out int nearIndex)
        {
            int missingSeat = allSeats[currentIndex] + 1;
            int higherSeat = allSeats[currentIndex] + 2;

            bool validSeat = !allSeats.Contains(missingSeat) && allSeats.Contains(higherSeat);

            if (validSeat)
                nearIndex = missingSeat;
            else
                nearIndex = -1;
            
            return validSeat;
        }

        private string[] ReadFile()
        {
            return File.ReadAllLines(filePath);
        }

        private int EvaluateSeat(string seatPositionData)
        {
            int rowIndex = BinarySearchIndex(seatPositionData, 0, rowIdentifierLength, maxRows, frontRowIdentifier, backRowIdentifier);
            int columnIndex = BinarySearchIndex(seatPositionData, rowIdentifierLength, rowIdentifierLength+columnIdentifierLength, maxColumns, leftColumnIdentifier, rightColumnIdentifier);

            return rowIndex * seatMultiplierIndex + columnIndex;
        }

        private int BinarySearchIndex(string seatData, int initIndex, int maxIndex, int highBoundSide, char lowerIdentifier, char higherIdentifier)
        {
            int rowIndex = 0;
            int higherBound = highBoundSide;

            for (int i = initIndex; i < maxIndex; i++)
            {
                char value = seatData[i];
                higherBound /= 2;

                if (value == higherIdentifier)
                    rowIndex += higherBound;
                else if (value != lowerIdentifier)
                    throw new InvalidDataException("Row is not "+frontRowIdentifier+" nor "+backRowIdentifier);
            }

            return rowIndex;
        }
    }
}