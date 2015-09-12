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
            int plugX = -1, plugY = -1, plugZ = -1, socketX = -1, socketY = -1, socketZ = -1;
            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugX = x;
                            plugY = y;
                            plugZ = z;
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugX < 0)
                return socket;

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketX = x;
                            socketY = y;
                            socketZ = z;
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugX + socketX >= 0 && y - plugY + socketY >= 0 && z - plugZ + socketZ >= 0 &&
                           x - plugX + socketX < xSize && y - plugY + socketY < ySize && z - plugZ + socketZ < zSize)
                        {
                            working[(x - plugX + socketX),
                                    (y - plugY + socketY),
                                    (z - plugZ + socketZ)] = plug[x, y, z];
                        }
                    }
                }
            }
            return working;

        }
        public static byte[,,] MergeVoxels(byte[,,] plug, byte[,,] socket, int matchColor, int removeColor, int replaceColor)
        {
            int plugX = -1, plugY = -1, plugZ = -1, socketX = -1, socketY = -1, socketZ = -1;
            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugX = x;
                            plugY = y;
                            plugZ = z;
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugX < 0)
                return socket;

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketX = x;
                            socketY = y;
                            socketZ = z;
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugX + socketX >= 0 && y - plugY + socketY >= 0 && z - plugZ + socketZ >= 0 &&
                           x - plugX + socketX < xSize && y - plugY + socketY < ySize && z - plugZ + socketZ < zSize)
                        {
                            working[(x - plugX + socketX),
                                    (y - plugY + socketY),
                                    (z - plugZ + socketZ)] = plug[x, y, z];
                        }
                    }
                }
            }
            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
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

        public static byte[,,] VoxListToArray(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
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

        public static byte[,,] VoxListToLargerArray(IEnumerable<MagicaVoxelData> voxelData, int multiplier, int xSize, int ySize, int zSize)
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
                            if(mvd.color != CURedux.emitter0 && mvd.color != CURedux.trail0 && mvd.color != CURedux.emitter1 && mvd.color != CURedux.trail1 && mvd.color != VoxelLogic.clear)
                                data[mvd.x * multiplier + x, mvd.y * multiplier + y, mvd.z * multiplier + z] = mvd.color;

                        }
                    }
                }
            }
            return data;
        }
        public static byte[,,] Translate(byte[,,] voxelData, int xMove, int yMove, int zMove)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] vs = new byte[xSize, ySize, zSize];

            int xmin = 0, ymin = 0, zmin = 0,
                xmax = xSize, ymax = ySize, zmax = zSize;
            if(xMove < 0)
                xmin -= xMove;
            else if(xMove > 0)
                xmax -= xMove;
            if(yMove < 0)
                ymin -= yMove;
            else if(yMove > 0)
                ymax -= yMove;
            if(zMove < 0)
                zmin -= zMove;
            else if(zMove > 0)
                zmax -= zMove;

            for(int x = xmin; x < xmax; x++)
            {
                for(int y = ymin; y < ymax; y++)
                {
                    for(int z = zmin; z < zmax; z++)
                    {
                        vs[x + xMove, y + yMove, z + zMove] = voxelData[x, y, z];
                    }
                }
            }

            return vs;

        }
        public static byte[,,] RunCA(byte[,,] voxelData, int smoothLevel)
        {
            if(smoothLevel <= 1)
                return voxelData;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
            byte[][,,] vs = new byte[smoothLevel][,,];
            vs[0] = voxelData.Replicate();
            for(int v = 1; v < smoothLevel; v++)
            {
                vs[v] = new byte[xSize, ySize, zSize];
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        for(int z = 0; z < zSize; z++)
                        {
                            colorCount.Clear();
                            int emptyCount = 0;
                            if(x == 0 || y == 0 || z == 0 || x == xSize - 1 || y == zSize - 1 || z == zSize - 1
                                || (254 - vs[v - 1][x, y, z]) % 4 == 0)
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
            return vs[smoothLevel - 1];
        }
        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int multiplier)
        {
            return VoxLargerArrayToList(voxelData, multiplier, multiplier, multiplier);
        }

        public static byte[,,] SealGaps(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[,,] vls = voxelData.Replicate();
            for(int z = 0; z < zSize - 1; z++)
            {
                for(int y = 1; y < ySize - 1; y++)
                {
                    for(int x = 1; x < xSize - 1; x++)
                    {
                        if(voxelData[x - 1, y, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x - 1, y, z + 1] == 0)
                            vls[x, y, z] = voxelData[x - 1, y, z];
                        else if(voxelData[x + 1, y, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x + 1, y, z + 1] == 0)
                            vls[x, y, z] = voxelData[x + 1, y, z];
                        else if(voxelData[x, y - 1, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x, y - 1, z + 1] == 0)
                            vls[x, y, z] = voxelData[x, y - 1, z];
                        else if(voxelData[x, y + 1, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x, y + 1, z + 1] == 0)
                            vls[x, y, z] = voxelData[x, y + 1, z];
                    }
                }
            }
            return vls;
        }


        public static byte[,,] Shrink(byte[,,] voxelData, int multiplier)
        {
            return Shrink(voxelData, multiplier, multiplier, multiplier);
        }
        public static byte[,,] Shrink(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();

            byte[,,] vfinal = new byte[xSize / xmultiplier, ySize / ymultiplier, zSize / zmultiplier];
            Dictionary<byte, MagicaVoxelData> specialColors = new Dictionary<byte, MagicaVoxelData>();
            for(int x = 0; x < xSize / xmultiplier; x++)
            {
                for(int y = 0; y < ySize / ymultiplier; y++)
                {
                    for(int z = 0; z < zSize / zmultiplier; z++)
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
                                    byte smallColor = voxelData[x * xmultiplier + xx, y * ymultiplier + yy, z * zmultiplier + zz];
                                    if((254 - smallColor) % 4 == 0)
                                    {
                                        specialCount++;
                                        if(specialColors.ContainsKey(smallColor))
                                        {
                                            if(specialColors[smallColor].color < specialCount)
                                            {
                                                specialColors[smallColor] = new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = (byte)specialCount };
                                            }
                                        }
                                        else
                                        {
                                            specialColors[smallColor] = new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = (byte)specialCount };
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
            return vfinal;
        }
        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            return VoxArrayToList(RunCA(voxelData, 3));
        }

        public static byte[,,] TransformStartLarge(IEnumerable<MagicaVoxelData> voxels)
        {
            return VoxListToLargerArray(voxels, 4, 60, 60, 60);
        }
        public static byte[,,] TransformStartHuge(IEnumerable<MagicaVoxelData> voxels)
        {
            return VoxListToLargerArray(voxels, 4, 120, 120, 80);
        }
        public static byte[,,] TransformStartLarge(IEnumerable<MagicaVoxelData> voxels, int multiplier)
        {
            return VoxListToLargerArray(voxels, multiplier, 60, 60, 60);
        }
        public static byte[,,] TransformStartHuge(IEnumerable<MagicaVoxelData> voxels, int multiplier)
        {
            return VoxListToLargerArray(voxels, multiplier, 120, 120, 80);
        }
        public static byte[,,] TransformStart(IEnumerable<MagicaVoxelData> voxels, int xSize, int ySize, int zSize)
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

            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempY = (y - (ySize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempY) + (xSize / 2));
                    int y2 = (int)Math.Round((-sn * tempX) + (cs * tempY) + (ySize / 2));

                    for(int z = 0; z < zSize; z++)
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

            for(int x = 0; x < xSize; x++)
            {
                for(int z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(int y = 0; y < ySize; y++)
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
            for(int x = 0; x < xSize; x++)
            {
                for(int z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(int y = 0; y < ySize; y++)
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
                for(int x = 0; x < xSize; x++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        int tempX = (x - (xSize / 2));
                        int tempZ = (z - (zSize / 2));
                        int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                        int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                        for(int y = 0; y < ySize; y++)
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

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    int tempY = (y - (ySize / 2));
                    int tempZ = (z - (zSize / 2));
                    int y2 = (int)Math.Round((-sn * tempZ) + (cs * tempY) + (ySize / 2));
                    int z2 = (int)Math.Round((cs * tempZ) + (sn * tempY) + (zSize / 2));

                    for(int x = 0; x < xSize; x++)
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
            byte[,,] vls = new byte[(int)(xSize * xScale), (int)(ySize * yScale), (int)(zSize * zScale)];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
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
