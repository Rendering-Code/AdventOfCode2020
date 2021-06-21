using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Advent
{
    public class Day17Advent: BaseAdvent
    {
        public struct Vector4 : IEquatable<Vector4>
        {
            public readonly int x;
            public readonly int y;
            public readonly int z;
            public readonly int w;
            
            public Vector4(int x, int y, int z, int w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public bool Equals(Vector4 other)
            {
                return x == other.x && y == other.y && z == other.z && w == other.w;
            }

            public override bool Equals(object obj)
            {
                return obj is Vector4 other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = x;
                    hashCode = (hashCode * 397) ^ y;
                    hashCode = (hashCode * 397) ^ z;
                    hashCode = (hashCode * 397) ^ w;
                    return hashCode;
                }
            }

            public static bool operator ==(Vector4 v1, Vector4 v2) => Equals(v1, v2);

            public static bool operator !=(Vector4 v1, Vector4 v2) => !(v1 == v2);
        }

        private int cycles;
        
        public Day17Advent(string filePath, int cycles) : base(filePath)
        {
            this.cycles = cycles;
        }

        public override void Execute()
        {
            string[] lines = File.ReadAllLines(filePath);
            List<Vector4> map = new List<Vector4>();

            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                for (int j = 0; j < currentLine.Length; j++)
                {
                    if (currentLine[j].Equals('#'))
                        map.Add(new Vector4(i,j, 0, 0));
                }
            }

            for (int i = 0; i < cycles; i++)
            {
                List<Vector4> temporalMap = new List<Vector4>();

                foreach (var position in map)
                {
                    if (!temporalMap.Contains(position) && EvaluateCell(position, map))
                        temporalMap.Add(position);

                    foreach (var nearCell in GetNearCellsPosition(position))
                    {
                        if (!temporalMap.Contains(nearCell) && EvaluateCell(nearCell, map))
                            temporalMap.Add(nearCell);
                    }
                }

                map = temporalMap;
            }

            Console.WriteLine("Result is: "+map.Count);
        }

        private bool EvaluateCell(Vector4 position, List<Vector4> map)
        {
            var nearActiveCells = GetNearActiveCells(position, map);
            bool isActiveCell = map.Contains(position);

            return (isActiveCell && nearActiveCells.Count == 2 || nearActiveCells.Count == 3) || (!isActiveCell && nearActiveCells.Count == 3);
        }

        private List<Vector4> GetNearActiveCells(Vector4 position, List<Vector4> map)
        {
            var allNearCells = GetNearCellsPosition(position);
            var activeCells = new List<Vector4>();
            
            foreach (var cell in allNearCells)
            {
                if (map.Contains(cell))
                    activeCells.Add(cell);
            }

            return activeCells;
        }

        private List<Vector4> GetNearCellsPosition(Vector4 position)
        {
            const int length = 1;
            
            List<Vector4> nearCells = new List<Vector4>();

            for (int i = position.x-length; i <= position.x+length; i++)
            {
                for (int j = position.y-length; j <= position.y+length; j++)
                {
                    for (int k = position.z-length; k <= position.z+length; k++)
                    {
                        for (int l = position.w-length; l <= position.w+length; l++)
                        {
                            Vector4 newPosition = new Vector4(i,j,k,l);

                            if (position == newPosition)
                                continue;
                            
                            nearCells.Add(newPosition);
                        }
                    }
                }
            }

            return nearCells;
        }
    }
}