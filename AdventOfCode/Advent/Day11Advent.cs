using System;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day11Advent: BaseAdvent
    {
        private int maxAdaptersDifference;
        private int iterations;

        private int occupiedSeatsTolerance;
        
        public Day11Advent(string filePath, int occupiedSeatsTolerance) : base(filePath)
        {
            this.occupiedSeatsTolerance = occupiedSeatsTolerance;
        }

        public override void Execute()
        {
            var map = ReadFile();
            PrintMap(map);
            AnalyzeMap(map);
        }

        private char[][] ReadFile()
        {
            string[] lines = File.ReadAllLines(filePath);
            
            char[][] map = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                var current = lines[i];
                map[i] = new char[current.Length];
                for (int j = 0; j < current.Length; j++)
                    map[i][j] = current[j];
            }

            return map;
        }

        private void PrintMap(char[][] map)
        {
            return;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Map iteration "+iterations);
            
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
        }

        private void AnalyzeMap(char[][] map)
        {
            iterations++;
            var copyMap = CopyMap(map);

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    copyMap[i][j] = EvaluateCell(i, j, map);
                }
            }

            PrintMap(copyMap);
            
            if (IsSameMap(copyMap, map))
            {
                int occupiedSeats = 0;
                for (int i = 0; i < copyMap.Length; i++)
                {
                    for (int j = 0; j < copyMap[i].Length; j++)
                    {
                        if (copyMap[i][j].Equals('#'))
                        {
                            occupiedSeats++;
                        }
                    }
                }

                Console.WriteLine("Ocuppied seats are "+occupiedSeats);
                Console.WriteLine("iterations "+iterations);
            }
            else
            {
                AnalyzeMap(copyMap);
            }
        }

        private char[][] CopyMap(char[][] originalMap)
        {
            char[][] newMap = new char[originalMap.GetLength(0)][];

            for (int i = 0; i < originalMap.Length; i++)
            {
                newMap[i] = new char[originalMap[i].Length];
                for (int j = 0; j < originalMap[i].Length; j++)
                {
                    newMap[i][j] = originalMap[i][j];
                }
            }

            return newMap;
        }

        private bool IsSameMap(char[][] mapA, char[][] mapB)
        {
            for (int i = 0; i < mapA.Length; i++)
            {
                for (int j = 0; j < mapA[i].Length; j++)
                {
                    if (!mapA[i][j].Equals(mapB[i][j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private char EvaluateCell(int x, int y, char[][] map)
        {
            int xInitial = Math.Max(x - 1, 0);
            int yInitial = Math.Max(y - 1, 0);
            int xFinal = Math.Min(x + 1, map.Length-1);
            int yFinal = Math.Min(y + 1, map[0].Length-1);
            char cellValue = map[x][y];

            int emptySeatCount = 0;
            int ocuppiedSeatCount = 0;
            int groundSpacesCount = 0;
            
            for (int i = xInitial; i <= xFinal; i++)
            {
                for (int j = yInitial; j <= yFinal; j++)
                {
                    if (i == x && j == y)
                        continue;

                    //var current = CheckCell(i, j, map);
                    var current = EvaluateAllDirectionsCells(x, y, i, j, map);
                    if (current.Equals('L'))
                    {
                        emptySeatCount++;
                    }
                    else if (current.Equals('#'))
                    {
                        ocuppiedSeatCount++;
                    }
                    else if (current.Equals('.'))
                    {
                        groundSpacesCount++;
                    }
                }
            }

            if (cellValue.Equals('L') && ocuppiedSeatCount == 0)
                return '#';
            
            if (cellValue.Equals('#') && ocuppiedSeatCount >= occupiedSeatsTolerance)
                return 'L';
                
            return cellValue;
        }

        private char CheckCell(int xNewPos, int yNewPos, char[][] map)
        {
            var current = map[xNewPos][yNewPos];
            return current;
        }
        
        private char EvaluateAllDirectionsCells(int xOrigin, int yOrigin, int xNewPos, int yNewPos, char[][] map)
        {
            /*var adjacent = CheckCell(xNewPos, yNewPos, map);
            if (adjacent.Equals('L') || adjacent.Equals('#'))
                return adjacent;*/

            int xDirection = xNewPos - xOrigin;
            int yDirection = yNewPos - yOrigin;

            char cell = '.';
            
            while (IsInRange(xNewPos, 0, map.Length-1) && IsInRange(yNewPos, 0, map[0].Length-1) && cell.Equals('.'))
            {
                cell = CheckCell(xNewPos, yNewPos, map);
                
                xNewPos += xDirection;
                yNewPos += yDirection;
            }

            return cell;
        }

        private bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}