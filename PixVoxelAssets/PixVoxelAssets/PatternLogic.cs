using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public struct Pattern
    {
        public byte[] CenterChangeOn;
        public byte[] CenterChangeOff;
        public byte[] EmptyChangeOn;
        public byte[] EmptyChangeOff;
        public byte FrequencyHigh;
        public byte FrequencyWide;
        public byte SpanHigh;
        public byte SpanWide;
        public float Probability;
        public bool RandomShape;
        public Pattern(int centerOn, int emptyOn, int centerOff, int emptyOff, int freqWide, int freqHigh, int spanWide, int spanHigh, float probability, bool randomShape = false)
        {
            CenterChangeOn = new byte[] { (byte)centerOn };
            CenterChangeOff = new byte[] { (byte)centerOff };
            EmptyChangeOn = new byte[] { (byte)emptyOn };
            EmptyChangeOff = new byte[] { (byte)emptyOff };
            FrequencyWide = (byte)freqWide;
            FrequencyHigh = (byte)freqHigh;
            SpanWide = (byte)spanWide;
            SpanHigh = (byte)spanHigh;
            Probability = probability;
            RandomShape = randomShape;
        }
        public Pattern(byte[] centerOn, byte[] emptyOn, byte[] centerOff, byte[] emptyOff, int freqWide, int freqHigh, int spanWide, int spanHigh, float probability, bool randomShape = false)
        {
            CenterChangeOn = centerOn;
            CenterChangeOff = centerOff;
            EmptyChangeOn = emptyOn;
            EmptyChangeOff = emptyOff;
            FrequencyWide = (byte)freqWide;
            FrequencyHigh = (byte)freqHigh;
            SpanWide = (byte)spanWide;
            SpanHigh = (byte)spanHigh;
            Probability = probability;
            RandomShape = randomShape;
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
            byte CenterChangeOn=0, CenterChangeOff=0, EmptyChangeOn=0, EmptyChangeOff=0;
            int seed = 1337;
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
                            int randHigh = ((pat.RandomShape) ? rng.Next(4) - 1 : 0),
                                randWide = ((pat.RandomShape) ? rng.Next(4) - 1 : 0);
                            if(adjEmpty)
                            {
                                colorCount[c]++;
                                Extensions.r = new Random(seed);
                                CenterChangeOn = pat.CenterChangeOn.RandomElement();
                                CenterChangeOff = pat.CenterChangeOff.RandomElement();
                                EmptyChangeOn = pat.EmptyChangeOn.RandomElement();
                                EmptyChangeOff = pat.EmptyChangeOff.RandomElement();
                                Extensions.r = new Random(seed);
                            }
                            if(adjEmpty && z % (pat.FrequencyHigh * Multiplier) < (pat.SpanHigh * Multiplier + randHigh) &&
                                colorCount[c] % (pat.FrequencyWide * Multiplier) < (pat.SpanWide * Multiplier + randWide) &&
                                rng.NextDouble() < pat.Probability)
                            {
                                neo[x, y, z] = CenterChangeOn;
                                if(x > 0 && voxelData[x - 1, y, z] == 0)
                                    neo[x - 1, y, z] = EmptyChangeOn;
                                if(y > 0 && voxelData[x, y - 1, z] == 0)
                                    neo[x, y - 1, z] = EmptyChangeOn;
                                if(z > 0 && voxelData[x, y, z - 1] == 0)
                                    neo[x, y, z - 1] = EmptyChangeOn;
                                if(x < xSize - 1 && voxelData[x + 1, y, z] == 0)
                                    neo[x + 1, y, z] = EmptyChangeOn;
                                if(y < ySize - 1 && voxelData[x, y + 1, z] == 0)
                                    neo[x, y + 1, z] = EmptyChangeOn;
                                if(z < zSize - 1 && voxelData[x, y, z + 1] == 0)
                                    neo[x, y, z + 1] = EmptyChangeOn;
                            }
                            else
                            {
                                neo[x, y, z] = CenterChangeOff;
                                if(adjEmpty)
                                {
                                    if(x > 0 && voxelData[x - 1, y, z] == 0)
                                        neo[x - 1, y, z] = EmptyChangeOff;
                                    if(y > 0 && voxelData[x, y - 1, z] == 0)
                                        neo[x, y - 1, z] = EmptyChangeOff;
                                    if(z > 0 && voxelData[x, y, z - 1] == 0)
                                        neo[x, y, z - 1] = EmptyChangeOff;
                                    if(x < xSize - 1 && voxelData[x + 1, y, z] == 0)
                                        neo[x + 1, y, z] = EmptyChangeOff;
                                    if(y < ySize - 1 && voxelData[x, y + 1, z] == 0)
                                        neo[x, y + 1, z] = EmptyChangeOff;
                                    if(z < zSize - 1 && voxelData[x, y, z + 1] == 0)
                                        neo[x, y, z + 1] = EmptyChangeOff;
                                    seed++;
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
