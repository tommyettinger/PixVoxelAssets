using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public struct Pattern
    {
        public byte CenterChangeOn;
        public byte CenterChangeOff;
        public byte EmptyChangeOn;
        public byte EmptyChangeOff;
        public byte FrequencyHigh;
        public byte FrequencyWide;
        public byte SpanHigh;
        public byte SpanWide;
        public float Probability;
        public Pattern(int centerOn, int emptyOn, int centerOff, int emptyOff, int freqWide, int freqHigh, int spanWide, int spanHigh, float probability)
        {
            CenterChangeOn = (byte)centerOn;
            CenterChangeOff = (byte)centerOff;
            EmptyChangeOn = (byte)emptyOn;
            EmptyChangeOff = (byte)emptyOff;
            FrequencyWide = (byte)freqWide;
            FrequencyHigh = (byte)freqHigh;
            SpanWide = (byte)spanWide;
            SpanHigh = (byte)spanHigh;
            Probability = probability;
        }
    }
    public class PatternLogic
    {
        public const int Multiplier = OrthoSingle.multiplier, Bonus = OrthoSingle.bonus;
        public static bool HasAdjacentEmpty(byte[,,] voxelData, int x, int y, int z, int xSize, int ySize, int zSize)
        {
            if(x <= 0 || y <= 0 || z <= 0 || x >= xSize - 1 || y >= ySize - 1 || z >= zSize - 1)
                return true;
            else
                return voxelData[x + 1, y, z] == 0 || voxelData[x - 1, y, z] == 0 ||
                            voxelData[x, y + 1, z] == 0 || voxelData[x, y - 1, z] == 0 ||
                            voxelData[x, y, z + 1] == 0 || voxelData[x, y, z - 1] == 0;
        }
        public static byte[,,] ApplyPattern(byte[,,] voxelData, Dictionary<byte, Pattern> patterns)
        {
            Random rng = new Random(0x1337);
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[,,] neo = voxelData.Replicate();
            Pattern pat;
            Dictionary<byte, int> colorCount;
            byte c = 0;
            for(int z = 0; z < zSize; z++)
            {
                colorCount = patterns.ToDictionary(kv => kv.Key, kv => z % 2 * Multiplier);

                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        c = voxelData[x, y, z];
                        if(patterns.ContainsKey(c))
                        {
                            bool adjEmpty = HasAdjacentEmpty(voxelData, x, y, z, xSize, ySize, zSize);
                            pat = patterns[c];
                            if(adjEmpty)
                            {
                                colorCount[c]++;
                            }
                            if(adjEmpty && z % (pat.FrequencyHigh * Multiplier) < (pat.SpanHigh * Multiplier) &&
                                colorCount[c] % (pat.FrequencyWide * Multiplier) < (pat.SpanWide * Multiplier) &&
                                rng.NextDouble() < pat.Probability)
                            {
                                neo[x, y, z] = pat.CenterChangeOn;
                                if(x > 0 && voxelData[x - 1, y, z] == 0)
                                    neo[x - 1, y, z] = pat.EmptyChangeOn;
                                if(y > 0 && voxelData[x, y - 1, z] == 0)
                                    neo[x, y - 1, z] = pat.EmptyChangeOn;
                                if(z > 0 && voxelData[x, y, z - 1] == 0)
                                    neo[x, y, z - 1] = pat.EmptyChangeOn;
                                if(x < xSize - 1 && voxelData[x + 1, y, z] == 0)
                                    neo[x + 1, y, z] = pat.EmptyChangeOn;
                                if(y < ySize - 1 && voxelData[x, y + 1, z] == 0)
                                    neo[x, y + 1, z] = pat.EmptyChangeOn;
                                if(z < zSize - 1 && voxelData[x, y, z + 1] == 0)
                                    neo[x, y, z + 1] = pat.EmptyChangeOn;
                            }
                            else
                            {
                                neo[x, y, z] = pat.CenterChangeOff;
                                if(adjEmpty)
                                {
                                    if(x > 0 && voxelData[x - 1, y, z] == 0)
                                        neo[x - 1, y, z] = pat.EmptyChangeOff;
                                    if(y > 0 && voxelData[x, y - 1, z] == 0)
                                        neo[x, y - 1, z] = pat.EmptyChangeOff;
                                    if(z > 0 && voxelData[x, y, z - 1] == 0)
                                        neo[x, y, z - 1] = pat.EmptyChangeOff;
                                    if(x < xSize - 1 && voxelData[x + 1, y, z] == 0)
                                        neo[x + 1, y, z] = pat.EmptyChangeOff;
                                    if(y < ySize - 1 && voxelData[x, y + 1, z] == 0)
                                        neo[x, y + 1, z] = pat.EmptyChangeOff;
                                    if(z < zSize - 1 && voxelData[x, y, z + 1] == 0)
                                        neo[x, y, z + 1] = pat.EmptyChangeOff;
                                }
                            }
                        }
                    }
                }
            }
            return neo;
        }
    }
}
