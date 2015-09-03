using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public class TransformLogic
    {

        public static byte[,,] MergeVoxels(byte[,,] plug, byte[,,] socket, int matchColor)
        {
            MagicaVoxelData plugMatcher = new MagicaVoxelData { color = 0 }, socketMatcher = new MagicaVoxelData { color = 0 };

            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugMatcher = new MagicaVoxelData { x = x, y = y, z = z, color = plug[x, y, z] };
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugMatcher.color == 0)
                return socket;

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketMatcher = new MagicaVoxelData { x = x, y = y, z = z, color = socket[x, y, z] };
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugMatcher.x + socketMatcher.x >= 0 && y - plugMatcher.y + socketMatcher.y >= 0 && z - plugMatcher.z + socketMatcher.z >= 0 &&
                           x - plugMatcher.x + socketMatcher.x < xSize && y - plugMatcher.y + socketMatcher.y < ySize && z - plugMatcher.z + socketMatcher.z < zSize)
                        {
                            working[(x - plugMatcher.x + socketMatcher.x),
                                    (y - plugMatcher.y + socketMatcher.y),
                                    (z - plugMatcher.z + socketMatcher.z)] = plug[x, y, z];
                        }
                    }
                }
            }
            return working;

        }
        public static byte[,,] MergeVoxels(byte[,,] plug, byte[,,] socket, int matchColor, int removeColor, int replaceColor)
        {
            MagicaVoxelData plugMatcher = new MagicaVoxelData { color = 0 }, socketMatcher = new MagicaVoxelData { color = 0 };

            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugMatcher = new MagicaVoxelData { x = x, y = y, z = z, color = plug[x, y, z] };
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugMatcher.color == 0)
                return socket;


            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketMatcher = new MagicaVoxelData { x = x, y = y, z = z, color = socket[x, y, z] };
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugMatcher.x + socketMatcher.x >= 0 && y - plugMatcher.y + socketMatcher.y >= 0 && z - plugMatcher.z + socketMatcher.z >= 0 &&
                           x - plugMatcher.x + socketMatcher.x < xSize && y - plugMatcher.y + socketMatcher.y < ySize && z - plugMatcher.z + socketMatcher.z < zSize)
                        {
                            working[(x - plugMatcher.x + socketMatcher.x),
                                    (y - plugMatcher.y + socketMatcher.y),
                                    (z - plugMatcher.z + socketMatcher.z)] = plug[x, y, z];
                        }
                    }
                }
            }
            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        if(254L - working[x, y, z] == removeColor * 4L)
                        {
                            working[x, y, z] = (byte)replaceColor;
                        }
                    }
                }
            }
            return working;

        }

        public static byte[,,] VoxListToArray(List<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                data[mvd.x, mvd.y, mvd.z] = mvd.color;
            }
            return data;
        }
        public static List<MagicaVoxelData> VoxArrayToList(byte[,,] voxelData)
        {
            List<MagicaVoxelData> vlist = new List<MagicaVoxelData>(voxelData.Length);
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(voxelData[x, y, z] > 0)
                            vlist.Add(new MagicaVoxelData { x = x, y = y, z = z, color = voxelData[x, y, z] });
                    }
                }
            }
            return vlist;
        }

        public static byte[,,] VoxListToLargerArray(List<MagicaVoxelData> voxelData, int multiplier, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize * multiplier, ySize * multiplier, zSize * multiplier];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                for(int x = 0; x < multiplier; x++)
                {
                    for(int y = 0; y < multiplier; y++)
                    {
                        for(int z = 0; z < multiplier; z++)
                        {
                            data[mvd.x * multiplier + x, mvd.y * multiplier + y, mvd.z * multiplier + z] = mvd.color;

                        }
                    }
                }
            }
            return data;
        }

        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int multiplier)
        {
            return VoxLargerArrayToList(voxelData, multiplier, multiplier, multiplier);
        }
        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
            byte[][,,] vs = new byte[4][,,];
            vs[0] = voxelData.Replicate();
            for(int v = 1; v < 4; v++)
            {
                vs[v] = new byte[xSize, ySize, zSize];
                for(byte x = 1; x < xSize - 1; x++)
                {
                    for(byte y = 1; y < ySize - 1; y++)
                    {
                        for(byte z = 1; z < zSize - 1; z++)
                        {
                            colorCount.Clear();
                            int emptyCount = 0;
                            byte best = 0;
                            if((254 - vs[v - 1][x, y, z]) % 4 == 0)
                            {
                                colorCount[vs[v - 1][x, y, z]] = 100;
                                goto END_INNER;
                            }
                            for(int xx = -1; xx < 2; xx++)
                            {
                                for(int yy = -1; yy < 2; yy++)
                                {
                                    for(int zz = -1; zz < 2; zz++)
                                    {
                                        byte smallColor = vs[v - 1][x + xx, y + yy, z + zz];
                                        if(smallColor == 0)
                                        {
                                            emptyCount++;
                                        }
                                        else if(colorCount.ContainsKey(smallColor))
                                        {
                                            colorCount[smallColor]++;
                                        }
                                        else
                                        {
                                            colorCount[smallColor] = 1;
                                        }
                                    }
                                }
                            }
                            END_INNER:
                            if(emptyCount > 17)
                                vs[v][x, y, z] = 0;
                            else
                                vs[v][x, y, z] = colorCount.OrderByDescending(kv => kv.Value).First().Key;


                        }
                    }
                }
            }
            byte[,,] vfinal = new byte[xSize / xmultiplier, ySize / ymultiplier, zSize / zmultiplier];
            Dictionary<byte, MagicaVoxelData> specialColors = new Dictionary<byte, MagicaVoxelData>();
            for(byte x = 0; x < xSize / xmultiplier; x++)
            {
                for(byte y = 0; y < ySize / ymultiplier; y++)
                {
                    for(byte z = 0; z < zSize / zmultiplier; z++)
                    {
                        colorCount = new Dictionary<byte, int>();
                        int fullCount = 0;
                        int emptyCount = 0;
                        int specialCount = 0;
                        for(int xx = 0; xx < xmultiplier; xx++)
                        {
                            for(int yy = 0; yy < ymultiplier; yy++)
                            {
                                for(int zz = 0; zz < zmultiplier; zz++)
                                {
                                    byte smallColor = vs[3][x * xmultiplier + xx, y * ymultiplier + yy, z * zmultiplier + zz];
                                    if((254 - smallColor) % 4 == 0)
                                    {
                                        specialCount++;
                                        if(specialColors.ContainsKey(smallColor))
                                        {
                                            if(specialColors[smallColor].color < specialCount)
                                            {
                                                specialColors[smallColor] = new MagicaVoxelData { x = x, y = y, z = z, color = (byte)specialCount };
                                            }
                                        }
                                        else
                                        {
                                            specialColors[smallColor] = new MagicaVoxelData { x = x, y = y, z = z, color = (byte)specialCount };
                                        }
                                    }
                                    if(smallColor > 0)
                                    {
                                        if(colorCount.ContainsKey(smallColor))
                                        {
                                            colorCount[smallColor] = colorCount[smallColor] + 16;
                                        }
                                        else
                                        {
                                            colorCount[smallColor] = 16;
                                        }
                                        fullCount += 16;
                                    }
                                    else
                                    {
                                        emptyCount += 5;
                                    }
                                }
                            }
                        }
                        byte best = 0;
                        if(fullCount >= emptyCount)
                            best = colorCount.OrderByDescending(kv => kv.Value).First().Key;
                        if(best > 0)
                        {
                            vfinal[x, y, z] = best;
                        }
                    }
                }
            }
            foreach(var kv in specialColors)
            {
                vfinal[kv.Value.x, kv.Value.y, kv.Value.z] = kv.Key;
            }
            return VoxArrayToList(vfinal);
        }

        public static byte[,,] TransformStartLarge(List<MagicaVoxelData> voxels)
        {
            return VoxListToLargerArray(voxels, 4, 60, 60, 60);
        }
        public static byte[,,] TransformStart(List<MagicaVoxelData> voxels, int xSize, int ySize, int zSize)
        {
            return VoxListToLargerArray(voxels, 4, xSize, ySize, zSize);
        }
        public static List<MagicaVoxelData> TransformEnd(byte[,,] colors)
        {
            return VoxLargerArrayToList(colors, 4);
        }

        public static byte[,,] RotateYawPartial(byte[,,] colors, int degrees)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];

            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempY = (y - (ySize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempY) + (xSize / 2));
                    int y2 = (int)Math.Round((-sn * tempX) + (cs * tempY) + (ySize / 2));

                    for(byte z = 0; z < zSize; z++)
                    {
                        if(x2 >= 0 && y2 >= 0 && x2 < xSize && y2 < ySize && colors[x, y, z] > 0)
                            vls[x2, y2, z] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }
        public static byte[,,] RotatePitchPartial(byte[,,] colors, int degrees)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];

            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(byte y = 0; y < ySize; y++)
                    {
                        if(x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && colors[x, y, z] > 0)
                            vls[x2, y, z2] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }
        public static byte[,,] RotatePitchPartialSpread(byte[,,] colors, int degrees, int effectStartDegrees, byte[] spreadColor)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            double finalAngle = (Math.PI / 180) * ((degrees) % 360), effectStartAngle = (Math.PI / 180) * ((effectStartDegrees) % 360);
            double increm = Math.PI / 36;

            if((degrees - effectStartDegrees + 1440) % 360 < 180)
            {
                increm *= -1.0;
            }

            double sn = 0.0, cs = 0.0;

            sn = Math.Sin(finalAngle);
            cs = Math.Cos(finalAngle);
            for(byte x = 0; x < xSize; x++)
            {
                for(byte z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(byte y = 0; y < ySize; y++)
                    {
                        if(x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && colors[x, y, z] > 0)
                            vls[x2, y, z2] = colors[x, y, z];
                    }
                }
            }
            int sclen = 1;
            if(spreadColor != null) sclen = spreadColor.Length;

            for(double angle = finalAngle; Math.Abs(effectStartAngle - angle) >= Math.Abs(increm); angle += increm)
            {
                sn = Math.Sin(angle);
                cs = Math.Cos(angle);
                for(byte x = 0; x < xSize; x++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        int tempX = (x - (xSize / 2));
                        int tempZ = (z - (zSize / 2));
                        int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                        int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                        for(byte y = 0; y < ySize; y++)
                        {
                            for(int it = 0; it < sclen; it++)
                            {
                                if((spreadColor == null || colors[x, y, z] == spreadColor[it]) && colors[x, y, z] > 0 && x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && vls[x2, y, z2] == 0)
                                    vls[x2, y, z2] = colors[x, y, z];
                            }
                        }
                    }
                }
            }
            return vls;
        }
        public static byte[,,] RotateRollPartial(byte[,,] colors, int degrees)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    int tempY = (y - (ySize / 2));
                    int tempZ = (z - (zSize / 2));
                    int y2 = (int)Math.Round((-sn * tempZ) + (cs * tempY) + (ySize / 2));
                    int z2 = (int)Math.Round((cs * tempZ) + (sn * tempY) + (zSize / 2));

                    for(byte x = 0; x < xSize; x++)
                    {
                        if(z2 >= 0 && y2 >= 0 && z2 < zSize && y2 < ySize && colors[x, y, z] > 0)
                            vls[x, y2, z2] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }

        public static byte[,,] ScalePartial(byte[,,] colors, double xScale, double yScale, double zScale)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            if(xScale <= 0 || yScale <= 0 || zScale <= 0)
                return colors;
            byte[,,] vls = new byte[xSize, ySize, zSize];

            for(byte z = 0; z < zSize; z++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte x = 0; x < xSize; x++)
                    {
                        for(double xsc = 0.0; xsc < xScale; xsc += 1.0)
                        {
                            for(double ysc = 0.0; ysc < yScale; ysc += 1.0)
                            {
                                for(double zsc = 0.0; zsc < zScale; zsc += 1.0)
                                {
                                    int tempX = (x - (xSize / 2));
                                    int tempY = (y - (ySize / 2));
                                    int tempZ = (z - (zSize / 2));
                                    int x2 = (int)Math.Round((xScale * tempX) + (xSize / 2) + ((tempX < 0) ? xsc : -xsc));
                                    int y2 = (int)Math.Round((yScale * tempY) + (ySize / 2) + ((tempY < 0) ? ysc : -ysc));
                                    int z2 = (int)Math.Round((zScale * tempZ) + (zSize / 2) + ((tempZ < 0) ? zsc : -zsc));

                                    if(colors[x, y, z] > 0 && x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize && y2 < ySize && z2 < zSize)
                                        vls[x2, y2, z2] = colors[x, y, z];
                                }
                            }
                        }
                    }
                }
            }
            return vls;
        }

    }
}
